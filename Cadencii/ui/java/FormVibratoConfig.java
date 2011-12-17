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
import javax.swing.SwingConstants;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BComboBox;
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BGroupBox;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BPanel;
import org.kbinani.windows.forms.BRadioButton;
import org.kbinani.windows.forms.BTextBox;
import org.kbinani.windows.forms.RadioButtonManager;

//SECTION-END-IMPORT
public class FormVibratoConfig extends BDialog {
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
    private BGroupBox groupSelect = null;
    private BRadioButton radioVocaloid1 = null;
    private BRadioButton radioVocaloid2 = null;
    private BRadioButton radioUserDefined = null;
    private RadioButtonManager mManager = null;
    private BLabel lblSpacer = null;
    
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
		this.setSize(398, 225);
		this.setContentPane(getJContentPane());
		this.setTitle("JFrame");
		mManager = new RadioButtonManager();
		mManager.add( radioUserDefined );
		mManager.add( radioVocaloid1 );
		mManager.add( radioVocaloid2 );
        setCancelButton( btnCancel );
	}

	/**
	 * This method initializes jContentPane
	 * 
	 * @return javax.swing.JPanel
	 */
	private JPanel getJContentPane() {
		if (jContentPane == null) {
			GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			gridBagConstraints31.gridx = 0;
			gridBagConstraints31.insets = new Insets(6, 12, 0, 12);
			gridBagConstraints31.gridwidth = 3;
			gridBagConstraints31.fill = GridBagConstraints.BOTH;
			gridBagConstraints31.weighty = 1.0D;
			gridBagConstraints31.gridy = 2;
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.gridx = 0;
			gridBagConstraints5.gridwidth = 3;
			gridBagConstraints5.weighty = 0.0D;
			gridBagConstraints5.anchor = GridBagConstraints.EAST;
			gridBagConstraints5.insets = new Insets(16, 0, 16, 12);
			gridBagConstraints5.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints5.gridy = 3;
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
			gridBagConstraints3.anchor = GridBagConstraints.WEST;
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
			jContentPane.add(getGroupSelect(), gridBagConstraints31);
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
			GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			gridBagConstraints9.gridx = 0;
			gridBagConstraints9.weightx = 1.0D;
			gridBagConstraints9.gridy = 0;
			lblSpacer = new BLabel();
			lblSpacer.setPreferredSize(new Dimension(4, 4));
			lblSpacer.setText("");
			GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			gridBagConstraints52.anchor = GridBagConstraints.SOUTHWEST;
			gridBagConstraints52.gridx = 1;
			gridBagConstraints52.gridy = 0;
			gridBagConstraints52.insets = new Insets(0, 0, 0, 0);
			GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
			gridBagConstraints42.anchor = GridBagConstraints.WEST;
			gridBagConstraints42.gridx = 2;
			gridBagConstraints42.gridy = 0;
			gridBagConstraints42.insets = new Insets(0, 0, 0, 0);
			jPanel2 = new BPanel();
			jPanel2.setLayout(new GridBagLayout());
			jPanel2.add(getBtnOK(), gridBagConstraints42);
			jPanel2.add(getBtnCancel(), gridBagConstraints52);
			jPanel2.add(lblSpacer, gridBagConstraints9);
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
			btnOK.setPreferredSize(new Dimension(100, 29));
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
			btnCancel.setPreferredSize(new Dimension(100, 29));
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
			txtVibratoLength.setPreferredSize(new Dimension(70, 19));
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
			comboVibratoType.setPreferredSize(new Dimension(167, 27));
		}
		return comboVibratoType;
	}

    /**
     * This method initializes groupSelect	
     * 	
     * @return org.kbinani.windows.forms.BGroupBox	
     */
    private BGroupBox getGroupSelect() {
        if (groupSelect == null) {
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.gridx = 2;
            gridBagConstraints8.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints8.weightx = 1.0D;
            gridBagConstraints8.gridy = 0;
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.gridx = 1;
            gridBagConstraints7.weightx = 1.0D;
            gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints7.gridy = 0;
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 0;
            gridBagConstraints6.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints6.weightx = 1.0D;
            gridBagConstraints6.gridy = 0;
            groupSelect = new BGroupBox();
            groupSelect.setLayout(new GridBagLayout());
            groupSelect.setTitle("Select from");
            groupSelect.add(getRadioVocaloid1(), gridBagConstraints6);
            groupSelect.add(getRadioVocaloid2(), gridBagConstraints7);
            groupSelect.add(getRadioUserDefined(), gridBagConstraints8);
        }
        return groupSelect;
    }

    /**
     * This method initializes radioVocaloid1	
     * 	
     * @return javax.swing.JRadioButton	
     */
    private BRadioButton getRadioVocaloid1() {
        if (radioVocaloid1 == null) {
            radioVocaloid1 = new BRadioButton();
            radioVocaloid1.setText("VOCALOID1");
            radioVocaloid1.setHorizontalAlignment(SwingConstants.CENTER);
        }
        return radioVocaloid1;
    }

    /**
     * This method initializes radioVocaloid2	
     * 	
     * @return javax.swing.JRadioButton	
     */
    private BRadioButton getRadioVocaloid2() {
        if (radioVocaloid2 == null) {
            radioVocaloid2 = new BRadioButton();
            radioVocaloid2.setText("VOCALOID2");
            radioVocaloid2.setSelected(true);
            radioVocaloid2.setHorizontalAlignment(SwingConstants.CENTER);
        }
        return radioVocaloid2;
    }

    /**
     * This method initializes radioUserDefined	
     * 	
     * @return javax.swing.JRadioButton	
     */
    private BRadioButton getRadioUserDefined() {
        if (radioUserDefined == null) {
            radioUserDefined = new BRadioButton();
            radioUserDefined.setText("User defined");
            radioUserDefined.setHorizontalAlignment(SwingConstants.CENTER);
        }
        return radioUserDefined;
    }

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
