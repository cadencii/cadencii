package org.kbinani.cadencii;
//SECTION-BEGIN-IMPORT

import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JPanel;
import javax.swing.SwingConstants;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BComboBox;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BGroupBox;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BListBox;
import org.kbinani.windows.forms.BPictureBox;
import org.kbinani.windows.forms.BTextBox;
import org.kbinani.windows.forms.RadioButtonManager;
import javax.swing.JList;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JTextField;
import javax.swing.BorderFactory;
import java.awt.Color;
import org.kbinani.windows.forms.BRadioButton;

//SECTION-END-IMPORT
public class FormSequenceConfig extends BForm {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 5210609912644248288L;
    private JPanel jPanel1 = null;
    private JPanel jPanel3 = null;
    private BButton btnCancel = null;
    private BButton btnOK = null;
    private BGroupBox groupWaveFileOutput = null;
    private JPanel jPanel = null;
    private BLabel lblChannel = null;
    private BComboBox comboChannel = null;
    private JPanel jPanel2 = null;
    private BRadioButton radioMasterTrack = null;
    private BRadioButton radioCurrentTrack = null;
    private BGroupBox groupSequence = null;
    private JPanel jPanel4 = null;
    private BLabel labelPreMeasure = null;
    private BComboBox comboPreMeasure = null;
    private JPanel jPanel21 = null;
    private BLabel labelSampleRate = null;
    private BComboBox comboSampleRate = null;
    private RadioButtonManager mManager = null;
    
    //SECTION-END-FIELD
    public FormSequenceConfig() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    private void initialize() {
        this.setSize(new Dimension(343, 295));
        this.setTitle("Sequence config");
        this.setContentPane(getJPanel1());
    	mManager = new RadioButtonManager();
    	mManager.add( radioCurrentTrack );
    	mManager.add( radioMasterTrack );
    }

    /**
     * This method initializes jPanel1	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 1;
            gridBagConstraints1.weightx = 1.0D;
            gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1.insets = new Insets(3, 12, 0, 12);
            gridBagConstraints1.gridy = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 1;
            gridBagConstraints.weightx = 1.0D;
            gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints.insets = new Insets(12, 12, 3, 12);
            gridBagConstraints.gridy = 0;
            GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
            gridBagConstraints20.gridy = 2;
            gridBagConstraints20.gridheight = 1;
            gridBagConstraints20.gridwidth = 2;
            gridBagConstraints20.gridx = 0;
            gridBagConstraints20.gridx = 0;
            gridBagConstraints20.gridwidth = 3;
            gridBagConstraints20.fill = GridBagConstraints.NONE;
            gridBagConstraints20.anchor = GridBagConstraints.EAST;
            gridBagConstraints20.weightx = 0.0D;
            gridBagConstraints20.insets = new Insets(16, 0, 16, 12);
            gridBagConstraints20.weighty = 0.0D;
            gridBagConstraints20.gridy = 6;
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getJPanel3(), gridBagConstraints20);
            jPanel1.add(getGroupWaveFileOutput(), gridBagConstraints);
            jPanel1.add(getGroupSequence(), gridBagConstraints1);
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
            GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
            gridBagConstraints111.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints111.gridy = 0;
            gridBagConstraints111.gridx = 1;
            GridBagConstraints gridBagConstraints1211 = new GridBagConstraints();
            gridBagConstraints1211.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints1211.gridy = 0;
            gridBagConstraints1211.gridx = 0;
            jPanel3 = new JPanel();
            jPanel3.setLayout(new GridBagLayout());
            jPanel3.add(getBtnCancel(), gridBagConstraints1211);
            jPanel3.add(getBtnOK(), gridBagConstraints111);
        }
        return jPanel3;
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
     * This method initializes groupWaveFileOutput	
     * 	
     * @return org.kbinani.windows.forms.BGroupBox	
     */
    private BGroupBox getGroupWaveFileOutput() {
        if (groupWaveFileOutput == null) {
            GridBagConstraints gridBagConstraints134 = new GridBagConstraints();
            gridBagConstraints134.anchor = GridBagConstraints.WEST;
            gridBagConstraints134.gridx = 0;
            gridBagConstraints134.gridy = 1;
            gridBagConstraints134.weightx = 1.0D;
            gridBagConstraints134.fill = GridBagConstraints.HORIZONTAL;
            GridBagConstraints gridBagConstraints130 = new GridBagConstraints();
            gridBagConstraints130.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints130.gridy = 0;
            gridBagConstraints130.weightx = 1.0D;
            gridBagConstraints130.gridx = 0;
            groupWaveFileOutput = new BGroupBox();
            groupWaveFileOutput.setLayout(new GridBagLayout());
            groupWaveFileOutput.setTitle("Wave File Output");
            groupWaveFileOutput.add(getJPanel(), gridBagConstraints130);
            groupWaveFileOutput.add(getJPanel2(), gridBagConstraints134);
        }
        return groupWaveFileOutput;
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.fill = GridBagConstraints.NONE;
            gridBagConstraints3.gridy = 1;
            gridBagConstraints3.weightx = 1.0;
            gridBagConstraints3.insets = new Insets(3, 12, 3, 0);
            gridBagConstraints3.anchor = GridBagConstraints.WEST;
            gridBagConstraints3.gridx = 1;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.insets = new Insets(3, 12, 3, 0);
            gridBagConstraints2.gridy = 1;
            labelSampleRate = new BLabel();
            labelSampleRate.setText("Sample rate");
            GridBagConstraints gridBagConstraints126 = new GridBagConstraints();
            gridBagConstraints126.anchor = GridBagConstraints.WEST;
            gridBagConstraints126.insets = new Insets(3, 12, 3, 0);
            gridBagConstraints126.gridx = 1;
            gridBagConstraints126.gridy = 0;
            gridBagConstraints126.weightx = 1.0;
            gridBagConstraints126.fill = GridBagConstraints.NONE;
            GridBagConstraints gridBagConstraints127 = new GridBagConstraints();
            gridBagConstraints127.anchor = GridBagConstraints.WEST;
            gridBagConstraints127.gridx = 0;
            gridBagConstraints127.gridy = 0;
            gridBagConstraints127.insets = new Insets(3, 12, 3, 0);
            lblChannel = new BLabel();
            lblChannel.setText("Channel");
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(lblChannel, gridBagConstraints127);
            jPanel.add(getComboChannel(), gridBagConstraints126);
            jPanel.add(labelSampleRate, gridBagConstraints2);
            jPanel.add(getComboSampleRate(), gridBagConstraints3);
        }
        return jPanel;
    }

    /**
     * This method initializes comboChannel	
     * 	
     * @return org.kbinani.windows.forms.BComboBox	
     */
    private BComboBox getComboChannel() {
        if (comboChannel == null) {
            comboChannel = new BComboBox();
            comboChannel.setPreferredSize(new Dimension(120, 27));
        }
        return comboChannel;
    }

    /**
     * This method initializes jPanel2	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel2() {
        if (jPanel2 == null) {
            GridBagConstraints gridBagConstraints129 = new GridBagConstraints();
            gridBagConstraints129.anchor = GridBagConstraints.WEST;
            gridBagConstraints129.gridx = 1;
            gridBagConstraints129.gridy = 0;
            gridBagConstraints129.weightx = 1.0D;
            gridBagConstraints129.insets = new Insets(3, 12, 3, 0);
            GridBagConstraints gridBagConstraints128 = new GridBagConstraints();
            gridBagConstraints128.anchor = GridBagConstraints.WEST;
            gridBagConstraints128.gridx = 0;
            gridBagConstraints128.gridy = 0;
            gridBagConstraints128.insets = new Insets(3, 12, 3, 0);
            jPanel2 = new JPanel();
            jPanel2.setLayout(new GridBagLayout());
            jPanel2.add(getRadioMasterTrack(), gridBagConstraints128);
            jPanel2.add(getRadioCurrentTrack(), gridBagConstraints129);
        }
        return jPanel2;
    }

    /**
     * This method initializes radioMasterTrack	
     * 	
     * @return org.kbinani.windows.forms.BRadioButton	
     */
    private BRadioButton getRadioMasterTrack() {
        if (radioMasterTrack == null) {
            radioMasterTrack = new BRadioButton();
            radioMasterTrack.setText("Master Track");
        }
        return radioMasterTrack;
    }

    /**
     * This method initializes radioCurrentTrack	
     * 	
     * @return org.kbinani.windows.forms.BRadioButton	
     */
    private BRadioButton getRadioCurrentTrack() {
        if (radioCurrentTrack == null) {
            radioCurrentTrack = new BRadioButton();
            radioCurrentTrack.setText("Current");
            radioCurrentTrack.setSelected(true);
        }
        return radioCurrentTrack;
    }

    /**
     * This method initializes groupSequence	
     * 	
     * @return org.kbinani.windows.forms.BGroupBox	
     */
    private BGroupBox getGroupSequence() {
        if (groupSequence == null) {
            GridBagConstraints gridBagConstraints1341 = new GridBagConstraints();
            gridBagConstraints1341.anchor = GridBagConstraints.WEST;
            gridBagConstraints1341.gridx = 0;
            gridBagConstraints1341.gridy = 1;
            gridBagConstraints1341.weightx = 1.0D;
            gridBagConstraints1341.fill = GridBagConstraints.HORIZONTAL;
            GridBagConstraints gridBagConstraints1301 = new GridBagConstraints();
            gridBagConstraints1301.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1301.gridy = 0;
            gridBagConstraints1301.weightx = 1.0D;
            gridBagConstraints1301.gridx = 0;
            groupSequence = new BGroupBox();
            groupSequence.setLayout(new GridBagLayout());
            groupSequence.setTitle("Sequence");
            groupSequence.add(getJPanel4(), gridBagConstraints1301);
            groupSequence.add(getJPanel21(), gridBagConstraints1341);
        }
        return groupSequence;
    }

    /**
     * This method initializes jPanel4	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel4() {
        if (jPanel4 == null) {
            GridBagConstraints gridBagConstraints1261 = new GridBagConstraints();
            gridBagConstraints1261.anchor = GridBagConstraints.WEST;
            gridBagConstraints1261.insets = new Insets(3, 12, 3, 0);
            gridBagConstraints1261.gridx = 1;
            gridBagConstraints1261.gridy = 0;
            gridBagConstraints1261.weightx = 1.0;
            gridBagConstraints1261.fill = GridBagConstraints.NONE;
            GridBagConstraints gridBagConstraints1271 = new GridBagConstraints();
            gridBagConstraints1271.anchor = GridBagConstraints.WEST;
            gridBagConstraints1271.gridx = 0;
            gridBagConstraints1271.gridy = 0;
            gridBagConstraints1271.insets = new Insets(3, 12, 3, 0);
            labelPreMeasure = new BLabel();
            labelPreMeasure.setText("Pre-measure");
            jPanel4 = new JPanel();
            jPanel4.setLayout(new GridBagLayout());
            jPanel4.add(labelPreMeasure, gridBagConstraints1271);
            jPanel4.add(getComboPreMeasure(), gridBagConstraints1261);
        }
        return jPanel4;
    }

    /**
     * This method initializes comboPreMeasure	
     * 	
     * @return org.kbinani.windows.forms.BComboBox	
     */
    private BComboBox getComboPreMeasure() {
        if (comboPreMeasure == null) {
            comboPreMeasure = new BComboBox();
            comboPreMeasure.setPreferredSize(new Dimension(97, 27));
        }
        return comboPreMeasure;
    }

    /**
     * This method initializes jPanel21	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel21() {
        if (jPanel21 == null) {
            jPanel21 = new JPanel();
            jPanel21.setLayout(new GridBagLayout());
        }
        return jPanel21;
    }

    /**
     * This method initializes comboSampleRate	
     * 	
     * @return org.kbinani.windows.forms.BComboBox	
     */
    private BComboBox getComboSampleRate() {
        if (comboSampleRate == null) {
            comboSampleRate = new BComboBox();
            comboSampleRate.setPreferredSize(new Dimension(97, 27));
        }
        return comboSampleRate;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="25,10"
