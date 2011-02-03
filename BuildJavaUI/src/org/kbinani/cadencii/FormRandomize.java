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
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BLabel;

//SECTION-END-IMPORT
public class FormRandomize extends BDialog {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 5210609912644248288L;
    private JPanel jPanel = null;
    private BLabel lblStartBar = null;
    private BLabel lblStart = null;
    private NumericUpDownEx numStartBar = null;
    private BLabel lblStartBeat = null;
    private NumericUpDownEx numStartBeat = null;
    private BLabel jLabel11 = null;
    private BLabel lblEnd = null;
    private NumericUpDownEx numEndBar = null;
    private BLabel lblEndBar = null;
    private NumericUpDownEx numEndBeat = null;
    private BLabel lblEndBeat = null;
    private BCheckBox chkShift = null;
    private JPanel jPanel1 = null;
    private JPanel jPanel2 = null;
    private BLabel lblShiftValue = null;
    private BComboBox comboShiftValue = null;
    private BCheckBox chkPit = null;
    private BLabel lblResolution = null;
    private NumericUpDownEx numResolution = null;
    private JPanel jPanel21 = null;
    private BLabel lblPitPattern = null;
    private BComboBox comboPitPattern = null;
    private JPanel jPanel22 = null;
    private BLabel lblPitValue = null;
    private BComboBox comboPitValue = null;
    private JPanel jPanel3 = null;
    private BButton btnCancel = null;
    private BButton btnOK = null;
    private BLabel jLabel1212 = null;
    private BLabel lblRightValue = null;
    //SECTION-END-FIELD
    public FormRandomize() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    private void initialize() {
        this.setSize(new Dimension(361, 344));
        this.setTitle("Randomize");
        this.setContentPane(getJPanel1());
   		setCancelButton( btnCancel );
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints112 = new GridBagConstraints();
            gridBagConstraints112.gridx = 7;
            gridBagConstraints112.weightx = 1.0D;
            gridBagConstraints112.gridy = 1;
            jLabel1212 = new BLabel();
            jLabel1212.setText(" ");
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.gridx = 6;
            gridBagConstraints9.gridy = 0;
            lblEndBeat = new BLabel();
            lblEndBeat.setText("beat");
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.gridx = 6;
            gridBagConstraints8.insets = new Insets(4, 4, 4, 8);
            gridBagConstraints8.gridy = 1;
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.gridx = 5;
            gridBagConstraints7.gridy = 0;
            lblEndBar = new BLabel();
            lblEndBar.setText("bar");
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 5;
            gridBagConstraints5.insets = new Insets(4, 8, 4, 4);
            gridBagConstraints5.gridy = 1;
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 4;
            gridBagConstraints4.gridy = 1;
            lblEnd = new BLabel();
            lblEnd.setText("end");
            GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
            gridBagConstraints31.gridx = 3;
            gridBagConstraints31.gridy = 1;
            GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
            gridBagConstraints21.gridx = 2;
            gridBagConstraints21.insets = new Insets(4, 4, 4, 8);
            gridBagConstraints21.gridy = 1;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 2;
            gridBagConstraints1.gridy = 0;
            lblStartBeat = new BLabel();
            lblStartBeat.setText("beat");
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints3.gridy = 1;
            gridBagConstraints3.weightx = 0.0D;
            gridBagConstraints3.insets = new Insets(4, 8, 4, 4);
            gridBagConstraints3.gridx = 1;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints2.gridy = 1;
            lblStart = new BLabel();
            lblStart.setText("start");
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 1;
            gridBagConstraints.gridy = 0;
            lblStartBar = new BLabel();
            lblStartBar.setText("bar");
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(lblStartBar, gridBagConstraints);
            jPanel.add(lblStart, gridBagConstraints2);
            jPanel.add(getNumStartBar(), gridBagConstraints3);
            jPanel.add(lblStartBeat, gridBagConstraints1);
            jPanel.add(getNumStartBeat(), gridBagConstraints21);
            jPanel.add(jLabel11, gridBagConstraints31);
            jPanel.add(lblEnd, gridBagConstraints4);
            jPanel.add(getNumEndBar(), gridBagConstraints5);
            jPanel.add(lblEndBar, gridBagConstraints7);
            jPanel.add(getNumEndBeat(), gridBagConstraints8);
            jPanel.add(lblEndBeat, gridBagConstraints9);
            jPanel.add(jLabel1212, gridBagConstraints112);
        }
        return jPanel;
    }

    /**
     * This method initializes numStartBar	
     * 	
     * @return javax.swing.JTextField	
     */
    private NumericUpDownEx getNumStartBar() {
        if (numStartBar == null) {
            numStartBar = new NumericUpDownEx();
            numStartBar.setPreferredSize(new Dimension(54, 28));
        }
        return numStartBar;
    }

    /**
     * This method initializes numStartBeat	
     * 	
     * @return org.kbinani.cadencii.NumericUpDownEx	
     */
    private NumericUpDownEx getNumStartBeat() {
        if (numStartBeat == null) {
            jLabel11 = new BLabel();
            jLabel11.setText("-");
            numStartBeat = new NumericUpDownEx();
            numStartBeat.setPreferredSize(new Dimension(54, 28));
        }
        return numStartBeat;
    }

    /**
     * This method initializes numEndBar	
     * 	
     * @return org.kbinani.cadencii.NumericUpDownEx	
     */
    private NumericUpDownEx getNumEndBar() {
        if (numEndBar == null) {
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = -1;
            gridBagConstraints6.gridy = -1;
            numEndBar = new NumericUpDownEx();
            numEndBar.setPreferredSize(new Dimension(54, 28));
        }
        return numEndBar;
    }

    /**
     * This method initializes numEndBeat	
     * 	
     * @return org.kbinani.cadencii.NumericUpDownEx	
     */
    private NumericUpDownEx getNumEndBeat() {
        if (numEndBeat == null) {
            numEndBeat = new NumericUpDownEx();
            numEndBeat.setPreferredSize(new Dimension(54, 28));
        }
        return numEndBeat;
    }

    /**
     * This method initializes chkShift	
     * 	
     * @return javax.swing.JCheckBox	
     */
    private BCheckBox getChkShift() {
        if (chkShift == null) {
            chkShift = new BCheckBox();
            chkShift.setText("Note Shift");
        }
        return chkShift;
    }

    /**
     * This method initializes jPanel1	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
            gridBagConstraints20.gridx = 0;
            gridBagConstraints20.gridwidth = 3;
            gridBagConstraints20.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints20.anchor = GridBagConstraints.EAST;
            gridBagConstraints20.weightx = 0.0D;
            gridBagConstraints20.weighty = 1.0D;
            gridBagConstraints20.gridy = 6;
            GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
            gridBagConstraints19.gridx = 0;
            gridBagConstraints19.gridwidth = 3;
            gridBagConstraints19.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints19.insets = new Insets(4, 0, 4, 0);
            gridBagConstraints19.gridy = 5;
            GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
            gridBagConstraints18.gridx = 0;
            gridBagConstraints18.gridwidth = 3;
            gridBagConstraints18.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints18.insets = new Insets(4, 0, 4, 0);
            gridBagConstraints18.gridy = 4;
            GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
            gridBagConstraints17.gridx = 2;
            gridBagConstraints17.anchor = GridBagConstraints.EAST;
            gridBagConstraints17.insets = new Insets(16, 0, 0, 16);
            gridBagConstraints17.weightx = 1.0D;
            gridBagConstraints17.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints17.gridy = 3;
            GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
            gridBagConstraints16.gridx = 1;
            gridBagConstraints16.anchor = GridBagConstraints.EAST;
            gridBagConstraints16.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints16.weightx = 1.0D;
            gridBagConstraints16.insets = new Insets(16, 0, 0, 4);
            gridBagConstraints16.gridy = 3;
            lblResolution = new BLabel();
            lblResolution.setText("Resolution");
            lblResolution.setHorizontalTextPosition(SwingConstants.RIGHT);
            lblResolution.setHorizontalAlignment(SwingConstants.RIGHT);
            lblResolution.setVerticalAlignment(SwingConstants.CENTER);
            GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
            gridBagConstraints15.gridx = 0;
            gridBagConstraints15.insets = new Insets(16, 16, 0, 0);
            gridBagConstraints15.gridy = 3;
            GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
            gridBagConstraints14.gridx = 0;
            gridBagConstraints14.weightx = 1.0D;
            gridBagConstraints14.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints14.anchor = GridBagConstraints.WEST;
            gridBagConstraints14.gridwidth = 3;
            gridBagConstraints14.insets = new Insets(4, 0, 4, 0);
            gridBagConstraints14.gridy = 2;
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.gridx = 0;
            gridBagConstraints11.anchor = GridBagConstraints.WEST;
            gridBagConstraints11.insets = new Insets(16, 16, 0, 0);
            gridBagConstraints11.gridy = 1;
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 0;
            gridBagConstraints10.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints10.weightx = 1.0D;
            gridBagConstraints10.gridwidth = 3;
            gridBagConstraints10.insets = new Insets(16, 16, 0, 0);
            gridBagConstraints10.gridy = 0;
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getJPanel(), gridBagConstraints10);
            jPanel1.add(getChkShift(), gridBagConstraints11);
            jPanel1.add(getJPanel2(), gridBagConstraints14);
            jPanel1.add(getChkPit(), gridBagConstraints15);
            jPanel1.add(lblResolution, gridBagConstraints16);
            jPanel1.add(getNumResolution(), gridBagConstraints17);
            jPanel1.add(getJPanel21(), gridBagConstraints18);
            jPanel1.add(getJPanel22(), gridBagConstraints19);
            jPanel1.add(getJPanel3(), gridBagConstraints20);
        }
        return jPanel1;
    }

    /**
     * This method initializes jPanel2	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel2() {
        if (jPanel2 == null) {
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.fill = GridBagConstraints.NONE;
            gridBagConstraints13.gridy = 0;
            gridBagConstraints13.weightx = 1.0;
            gridBagConstraints13.anchor = GridBagConstraints.EAST;
            gridBagConstraints13.insets = new Insets(0, 8, 0, 16);
            gridBagConstraints13.gridx = 1;
            GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
            gridBagConstraints12.gridx = 0;
            gridBagConstraints12.insets = new Insets(0, 32, 0, 0);
            gridBagConstraints12.anchor = GridBagConstraints.WEST;
            gridBagConstraints12.gridy = 0;
            lblShiftValue = new BLabel();
            lblShiftValue.setText("Value");
            jPanel2 = new JPanel();
            jPanel2.setLayout(new GridBagLayout());
            jPanel2.add(lblShiftValue, gridBagConstraints12);
            jPanel2.add(getComboShiftValue(), gridBagConstraints13);
        }
        return jPanel2;
    }

    /**
     * This method initializes comboShiftValue	
     * 	
     * @return javax.swing.JComboBox	
     */
    private BComboBox getComboShiftValue() {
        if (comboShiftValue == null) {
            comboShiftValue = new BComboBox();
            comboShiftValue.setPreferredSize(new Dimension(218, 27));
        }
        return comboShiftValue;
    }

    /**
     * This method initializes chkPit	
     * 	
     * @return javax.swing.JCheckBox	
     */
    private BCheckBox getChkPit() {
        if (chkPit == null) {
            chkPit = new BCheckBox();
            chkPit.setText("Pitch Fluctuation");
        }
        return chkPit;
    }

    /**
     * This method initializes numResolution	
     * 	
     * @return org.kbinani.cadencii.NumericUpDownEx	
     */
    private NumericUpDownEx getNumResolution() {
        if (numResolution == null) {
            numResolution = new NumericUpDownEx();
            numResolution.setPreferredSize(new Dimension(54, 28));
        }
        return numResolution;
    }

    /**
     * This method initializes jPanel21	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel21() {
        if (jPanel21 == null) {
            GridBagConstraints gridBagConstraints131 = new GridBagConstraints();
            gridBagConstraints131.anchor = GridBagConstraints.EAST;
            gridBagConstraints131.insets = new Insets(0, 8, 0, 16);
            gridBagConstraints131.gridx = 1;
            gridBagConstraints131.gridy = 0;
            gridBagConstraints131.weightx = 1.0;
            gridBagConstraints131.fill = GridBagConstraints.NONE;
            GridBagConstraints gridBagConstraints121 = new GridBagConstraints();
            gridBagConstraints121.anchor = GridBagConstraints.WEST;
            gridBagConstraints121.gridx = 0;
            gridBagConstraints121.gridy = 0;
            gridBagConstraints121.insets = new Insets(0, 32, 0, 0);
            lblPitPattern = new BLabel();
            lblPitPattern.setText("Pattern");
            jPanel21 = new JPanel();
            jPanel21.setLayout(new GridBagLayout());
            jPanel21.add(lblPitPattern, gridBagConstraints121);
            jPanel21.add(getComboPitPattern(), gridBagConstraints131);
        }
        return jPanel21;
    }

    /**
     * This method initializes comboPitPattern	
     * 	
     * @return javax.swing.JComboBox	
     */
    private BComboBox getComboPitPattern() {
        if (comboPitPattern == null) {
            comboPitPattern = new BComboBox();
            comboPitPattern.setPreferredSize(new Dimension(218, 27));
        }
        return comboPitPattern;
    }

    /**
     * This method initializes jPanel22	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel22() {
        if (jPanel22 == null) {
            GridBagConstraints gridBagConstraints132 = new GridBagConstraints();
            gridBagConstraints132.anchor = GridBagConstraints.EAST;
            gridBagConstraints132.insets = new Insets(0, 8, 0, 16);
            gridBagConstraints132.gridx = 1;
            gridBagConstraints132.gridy = 0;
            gridBagConstraints132.weightx = 1.0;
            gridBagConstraints132.fill = GridBagConstraints.NONE;
            GridBagConstraints gridBagConstraints122 = new GridBagConstraints();
            gridBagConstraints122.anchor = GridBagConstraints.WEST;
            gridBagConstraints122.gridx = 0;
            gridBagConstraints122.gridy = 0;
            gridBagConstraints122.insets = new Insets(0, 32, 0, 0);
            lblPitValue = new BLabel();
            lblPitValue.setText("Value");
            jPanel22 = new JPanel();
            jPanel22.setLayout(new GridBagLayout());
            jPanel22.add(lblPitValue, gridBagConstraints122);
            jPanel22.add(getComboPitValue(), gridBagConstraints132);
        }
        return jPanel22;
    }

    /**
     * This method initializes comboPitValue	
     * 	
     * @return javax.swing.JComboBox	
     */
    private BComboBox getComboPitValue() {
        if (comboPitValue == null) {
            comboPitValue = new BComboBox();
            comboPitValue.setPreferredSize(new Dimension(218, 27));
        }
        return comboPitValue;
    }

    /**
     * This method initializes jPanel3	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel3() {
        if (jPanel3 == null) {
            GridBagConstraints gridBagConstraints22 = new GridBagConstraints();
            gridBagConstraints22.gridx = 0;
            gridBagConstraints22.weightx = 1.0D;
            gridBagConstraints22.gridy = 0;
            lblRightValue = new BLabel();
            lblRightValue.setPreferredSize(new Dimension(4, 4));
            lblRightValue.setText("");
            GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
            gridBagConstraints111.insets = new Insets(0, 0, 0, 12);
            gridBagConstraints111.gridy = 0;
            gridBagConstraints111.gridx = 2;
            GridBagConstraints gridBagConstraints1211 = new GridBagConstraints();
            gridBagConstraints1211.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints1211.gridy = 0;
            gridBagConstraints1211.gridx = 1;
            jPanel3 = new JPanel();
            jPanel3.setLayout(new GridBagLayout());
            jPanel3.add(getBtnCancel(), gridBagConstraints1211);
            jPanel3.add(getBtnOK(), gridBagConstraints111);
            jPanel3.add(lblRightValue, gridBagConstraints22);
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

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
