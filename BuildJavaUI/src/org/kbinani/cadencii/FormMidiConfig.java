package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BGroupBox;
import org.kbinani.windows.forms.BNumericUpDown;
import java.awt.Dimension;
import javax.swing.JPanel;
import java.awt.GridBagLayout;
import javax.swing.JLabel;
import java.awt.GridBagConstraints;
import java.awt.Insets;
import javax.swing.JComboBox;
import javax.swing.JCheckBox;
import javax.swing.JToggleButton;
import org.kbinani.windows.forms.BButton;

//SECTION-END-IMPORT
public class FormMidiConfig extends BDialog {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private JPanel jPanel = null;
    private BGroupBox groupMetronome = null;
    private JLabel lblDeviceMetronome = null;
    private JComboBox comboDeviceMetronome = null;
    private JLabel lblProgramNormal = null;
    private BNumericUpDown numProgramNormal = null;
    private JLabel lblProgramBell = null;
    private BNumericUpDown numProgramBell = null;
    private JLabel lblNoteNormal = null;
    private BNumericUpDown numNoteNormal = null;
    private JLabel lblNoteBell = null;
    private BNumericUpDown numNoteBell = null;
    private JLabel lblPreUtterance = null;
    private BNumericUpDown numPreUtterance = null;
    private JCheckBox chkRingBell = null;
    private JToggleButton chkPreview = null;
    private JLabel lblMillisec = null;
    private JLabel jLabel = null;
    private JComboBox jComboBox = null;
    private JPanel jPanel3 = null;
    private BButton btnCancel = null;
    private BButton btnOk = null;

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public FormMidiConfig() {
    	super();
    	initialize();
    }

    //SECTION-BEGIN-METHOD
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(349, 423));
        this.setTitle("Metronome Config");
        this.setContentPane(getJPanel());
    		
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
            gridBagConstraints18.gridx = 1;
            gridBagConstraints18.anchor = GridBagConstraints.NORTHEAST;
            gridBagConstraints18.insets = new Insets(12, 0, 12, 0);
            gridBagConstraints18.weighty = 1.0D;
            gridBagConstraints18.gridy = 2;
            GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
            gridBagConstraints17.fill = GridBagConstraints.NONE;
            gridBagConstraints17.gridy = 0;
            gridBagConstraints17.weightx = 1.0;
            gridBagConstraints17.anchor = GridBagConstraints.WEST;
            gridBagConstraints17.insets = new Insets(24, 12, 24, 0);
            gridBagConstraints17.gridx = 1;
            GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
            gridBagConstraints16.gridx = 0;
            gridBagConstraints16.insets = new Insets(24, 12, 24, 0);
            gridBagConstraints16.gridy = 0;
            jLabel = new JLabel();
            jLabel.setText("MIDI Device");
            GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
            gridBagConstraints15.gridx = 0;
            gridBagConstraints15.gridwidth = 2;
            gridBagConstraints15.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints15.insets = new Insets(0, 12, 0, 12);
            gridBagConstraints15.gridy = 1;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getGroupMetronome(), gridBagConstraints15);
            jPanel.add(jLabel, gridBagConstraints16);
            jPanel.add(getJComboBox(), gridBagConstraints17);
            jPanel.add(getJPanel3(), gridBagConstraints18);
        }
        return jPanel;
    }

    /**
     * This method initializes groupMetronome	
     * 	
     * @return javax.swing.JPanel	
     */
    private BGroupBox getGroupMetronome() {
        if (groupMetronome == null) {
            GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
            gridBagConstraints14.gridx = 2;
            gridBagConstraints14.gridy = 5;
            lblMillisec = new JLabel();
            lblMillisec.setText("millisec");
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.gridx = 0;
            gridBagConstraints13.anchor = GridBagConstraints.NORTHWEST;
            gridBagConstraints13.gridwidth = 2;
            gridBagConstraints13.weighty = 1.0D;
            gridBagConstraints13.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints13.gridy = 7;
            GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
            gridBagConstraints12.gridx = 0;
            gridBagConstraints12.gridwidth = 2;
            gridBagConstraints12.anchor = GridBagConstraints.WEST;
            gridBagConstraints12.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints12.gridy = 6;
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.fill = GridBagConstraints.NONE;
            gridBagConstraints11.gridy = 5;
            gridBagConstraints11.weightx = 1.0;
            gridBagConstraints11.anchor = GridBagConstraints.WEST;
            gridBagConstraints11.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints11.gridx = 1;
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 0;
            gridBagConstraints10.anchor = GridBagConstraints.WEST;
            gridBagConstraints10.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints10.gridy = 5;
            lblPreUtterance = new JLabel();
            lblPreUtterance.setText("Pre Utterance");
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.fill = GridBagConstraints.NONE;
            gridBagConstraints9.gridy = 4;
            gridBagConstraints9.weightx = 1.0;
            gridBagConstraints9.anchor = GridBagConstraints.WEST;
            gridBagConstraints9.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints9.gridwidth = 2;
            gridBagConstraints9.gridx = 1;
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.gridx = 0;
            gridBagConstraints8.anchor = GridBagConstraints.WEST;
            gridBagConstraints8.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints8.gridy = 4;
            lblNoteBell = new JLabel();
            lblNoteBell.setText("Note# (Bell)");
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.fill = GridBagConstraints.NONE;
            gridBagConstraints7.gridy = 3;
            gridBagConstraints7.weightx = 1.0;
            gridBagConstraints7.anchor = GridBagConstraints.WEST;
            gridBagConstraints7.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints7.gridwidth = 2;
            gridBagConstraints7.gridx = 1;
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 0;
            gridBagConstraints6.anchor = GridBagConstraints.WEST;
            gridBagConstraints6.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints6.gridy = 3;
            lblNoteNormal = new JLabel();
            lblNoteNormal.setText("Note#");
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.fill = GridBagConstraints.NONE;
            gridBagConstraints5.gridy = 2;
            gridBagConstraints5.weightx = 1.0;
            gridBagConstraints5.anchor = GridBagConstraints.WEST;
            gridBagConstraints5.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints5.gridwidth = 2;
            gridBagConstraints5.gridx = 1;
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.anchor = GridBagConstraints.WEST;
            gridBagConstraints4.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints4.gridy = 2;
            lblProgramBell = new JLabel();
            lblProgramBell.setText("Program# (Bell)");
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.fill = GridBagConstraints.NONE;
            gridBagConstraints3.gridy = 1;
            gridBagConstraints3.weightx = 1.0;
            gridBagConstraints3.anchor = GridBagConstraints.WEST;
            gridBagConstraints3.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints3.gridwidth = 2;
            gridBagConstraints3.gridx = 1;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints2.gridy = 1;
            lblProgramNormal = new JLabel();
            lblProgramNormal.setText("Program#");
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.NONE;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weightx = 1.0D;
            gridBagConstraints1.anchor = GridBagConstraints.WEST;
            gridBagConstraints1.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints1.gridwidth = 2;
            gridBagConstraints1.gridx = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints.gridy = 0;
            gridBagConstraints.anchor = GridBagConstraints.WEST;
            gridBagConstraints.gridx = 0;
            lblDeviceMetronome = new JLabel();
            lblDeviceMetronome.setText("MIDI Device");
            groupMetronome = new BGroupBox();
            groupMetronome.setLayout(new GridBagLayout());
            groupMetronome.setTitle("Metronome");
            groupMetronome.add(lblDeviceMetronome, gridBagConstraints);
            groupMetronome.add(getComboDeviceMetronome(), gridBagConstraints1);
            groupMetronome.add(lblProgramNormal, gridBagConstraints2);
            groupMetronome.add(getNumProgramNormal(), gridBagConstraints3);
            groupMetronome.add(lblProgramBell, gridBagConstraints4);
            groupMetronome.add(getNumProgramBell(), gridBagConstraints5);
            groupMetronome.add(lblNoteNormal, gridBagConstraints6);
            groupMetronome.add(getNumNoteNormal(), gridBagConstraints7);
            groupMetronome.add(lblNoteBell, gridBagConstraints8);
            groupMetronome.add(getNumNoteBell(), gridBagConstraints9);
            groupMetronome.add(lblPreUtterance, gridBagConstraints10);
            groupMetronome.add(getNumPreUtterance(), gridBagConstraints11);
            groupMetronome.add(getChkRingBell(), gridBagConstraints12);
            groupMetronome.add(getChkPreview(), gridBagConstraints13);
            groupMetronome.add(lblMillisec, gridBagConstraints14);
        }
        return groupMetronome;
    }

    /**
     * This method initializes comboDeviceMetronome	
     * 	
     * @return javax.swing.JComboBox	
     */
    private JComboBox getComboDeviceMetronome() {
        if (comboDeviceMetronome == null) {
            comboDeviceMetronome = new JComboBox();
            comboDeviceMetronome.setPreferredSize(new Dimension(188, 20));
            comboDeviceMetronome.setPreferredSize(new Dimension(134, 21));
        }
        return comboDeviceMetronome;
    }

    /**
     * This method initializes numProgramNormal	
     * 	
     * @return javax.swing.JComboBox	
     */
    private BNumericUpDown getNumProgramNormal() {
        if (numProgramNormal == null) {
            numProgramNormal = new BNumericUpDown();
            numProgramNormal.setPreferredSize(new Dimension(100, 19));
        }
        return numProgramNormal;
    }

    /**
     * This method initializes numProgramBell	
     * 	
     * @return javax.swing.JComboBox	
     */
    private BNumericUpDown getNumProgramBell() {
        if (numProgramBell == null) {
            numProgramBell = new BNumericUpDown();
            numProgramBell.setPreferredSize(new Dimension(100, 19));
        }
        return numProgramBell;
    }

    /**
     * This method initializes numNoteNormal	
     * 	
     * @return javax.swing.JComboBox	
     */
    private BNumericUpDown getNumNoteNormal() {
        if (numNoteNormal == null) {
            numNoteNormal = new BNumericUpDown();
            numNoteNormal.setPreferredSize(new Dimension(100, 19));
        }
        return numNoteNormal;
    }

    /**
     * This method initializes numNoteBell	
     * 	
     * @return javax.swing.JComboBox	
     */
    private BNumericUpDown getNumNoteBell() {
        if (numNoteBell == null) {
            numNoteBell = new BNumericUpDown();
            numNoteBell.setPreferredSize(new Dimension(100, 19));
        }
        return numNoteBell;
    }

    /**
     * This method initializes numPreUtterance	
     * 	
     * @return javax.swing.JComboBox	
     */
    private BNumericUpDown getNumPreUtterance() {
        if (numPreUtterance == null) {
            numPreUtterance = new BNumericUpDown();
            numPreUtterance.setPreferredSize(new Dimension(120, 19));
        }
        return numPreUtterance;
    }

    /**
     * This method initializes chkRingBell	
     * 	
     * @return javax.swing.JCheckBox	
     */
    private JCheckBox getChkRingBell() {
        if (chkRingBell == null) {
            chkRingBell = new JCheckBox();
            chkRingBell.setText("Ring Bell");
        }
        return chkRingBell;
    }

    /**
     * This method initializes chkPreview	
     * 	
     * @return javax.swing.JToggleButton	
     */
    private JToggleButton getChkPreview() {
        if (chkPreview == null) {
            chkPreview = new JToggleButton();
            chkPreview.setText("Preview");
            chkPreview.setPreferredSize(new Dimension(80, 23));
        }
        return chkPreview;
    }

    /**
     * This method initializes jComboBox	
     * 	
     * @return javax.swing.JComboBox	
     */
    private JComboBox getJComboBox() {
        if (jComboBox == null) {
            jComboBox = new JComboBox();
            jComboBox.setPreferredSize(new Dimension(188, 20));
            jComboBox.setPreferredSize(new Dimension(188, 20));
        }
        return jComboBox;
    }

    /**
     * This method initializes jPanel3	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel3() {
        if (jPanel3 == null) {
            GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
            gridBagConstraints111.insets = new Insets(0, 0, 0, 16);
            gridBagConstraints111.gridy = 0;
            gridBagConstraints111.gridx = 0;
            GridBagConstraints gridBagConstraints121 = new GridBagConstraints();
            gridBagConstraints121.insets = new Insets(0, 0, 0, 16);
            gridBagConstraints121.gridy = 0;
            gridBagConstraints121.gridx = 2;
            jPanel3 = new JPanel();
            jPanel3.setLayout(new GridBagLayout());
            jPanel3.add(getBtnCancel(), gridBagConstraints121);
            jPanel3.add(getBtnOk(), gridBagConstraints111);
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
        }
        return btnCancel;
    }

    /**
     * This method initializes btnOk	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnOk() {
        if (btnOk == null) {
            btnOk = new BButton();
            btnOk.setText("OK");
        }
        return btnOk;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
