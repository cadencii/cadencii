package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JPanel;
import javax.swing.JTextField;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BComboBox;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BPanel;
import org.kbinani.windows.forms.BTextBox;

//SECTION-END-IMPORT
public class FormVibratoConfig extends BForm {
    //SECTION-BEGIN-FIELD
    
	private static final long serialVersionUID = 1L;
	private BPanel jContentPane = null;
	private BPanel jPanel2 = null;
	private BButton btnOK = null;
	private BButton btnCancel = null;
	private BLabel lblVibratoLength = null;
	private BTextBox txtVibratoLength = null;
	private BLabel jLabel1 = null;
	private BLabel lblVibratoType = null;
	private BComboBox comboVibratoType = null;

	//SECTION-END-FIELD
	/**
	 * This is the default constructor
	 */
	public FormVibratoConfig() {
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
		this.setSize(339, 157);
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
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.gridx = 0;
			gridBagConstraints5.gridwidth = 3;
			gridBagConstraints5.weighty = 1.0D;
			gridBagConstraints5.anchor = GridBagConstraints.EAST;
			gridBagConstraints5.gridy = 2;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.fill = GridBagConstraints.NONE;
			gridBagConstraints4.gridy = 1;
			gridBagConstraints4.weightx = 0.0D;
			gridBagConstraints4.anchor = GridBagConstraints.WEST;
			gridBagConstraints4.gridwidth = 2;
			gridBagConstraints4.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints4.gridx = 1;
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.gridx = 0;
			gridBagConstraints3.anchor = GridBagConstraints.SOUTHWEST;
			gridBagConstraints3.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints3.gridy = 1;
			lblVibratoType = new BLabel();
			lblVibratoType.setText("Vibrato Type");
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 2;
			gridBagConstraints2.insets = new Insets(12, 3, 0, 0);
			gridBagConstraints2.anchor = GridBagConstraints.WEST;
			gridBagConstraints2.weightx = 1.0D;
			gridBagConstraints2.gridy = 0;
			jLabel1 = new BLabel();
			jLabel1.setText("% (0-100)");
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.fill = GridBagConstraints.NONE;
			gridBagConstraints1.gridy = 0;
			gridBagConstraints1.weightx = 0.0D;
			gridBagConstraints1.anchor = GridBagConstraints.WEST;
			gridBagConstraints1.insets = new Insets(12, 12, 3, 0);
			gridBagConstraints1.gridx = 1;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.anchor = GridBagConstraints.WEST;
			gridBagConstraints.insets = new Insets(12, 12, 3, 0);
			gridBagConstraints.gridy = 0;
			lblVibratoLength = new BLabel();
			lblVibratoLength.setText("Vibrato Length");
			jContentPane = new BPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.add(lblVibratoLength, gridBagConstraints);
			jContentPane.add(getTxtVibratoLength(), gridBagConstraints1);
			jContentPane.add(jLabel1, gridBagConstraints2);
			jContentPane.add(lblVibratoType, gridBagConstraints3);
			jContentPane.add(getComboVibratoType(), gridBagConstraints4);
			jContentPane.add(getJPanel2(), gridBagConstraints5);
		}
		return jContentPane;
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
			jPanel2 = new BPanel();
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
			btnOK = new BButton();
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
			btnCancel = new BButton();
			btnCancel.setText("Cancel");
		}
		return btnCancel;
	}

	/**
	 * This method initializes txtVibratoLength	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtVibratoLength() {
		if (txtVibratoLength == null) {
			txtVibratoLength = new BTextBox();
			txtVibratoLength.setPreferredSize(new Dimension(61, 19));
		}
		return txtVibratoLength;
	}

	/**
	 * This method initializes comboVibratoType	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboVibratoType() {
		if (comboVibratoType == null) {
			comboVibratoType = new BComboBox();
			comboVibratoType.setPreferredSize(new Dimension(167, 20));
		}
		return comboVibratoType;
	}

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
