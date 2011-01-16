package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BComboBox;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BGroupBox;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BNumericUpDown;
import org.kbinani.windows.forms.BPanel;

//SECTION-END-IMPORT
public class FormBeatConfig extends BForm {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 4414859292940722020L;
    private BPanel jContentPane = null;
    private BGroupBox groupPosition = null;
    private BLabel lblStart = null;
    private BNumericUpDown numStart = null;
    private BLabel lblBar1 = null;
    private BCheckBox chkEnd = null;
    private BNumericUpDown numEnd = null;
    private BLabel lblBar2 = null;
    private BGroupBox groupBeat = null;
    private BNumericUpDown numNumerator = null;
    private BLabel jLabel = null;
    private BLabel jLabel1 = null;
    private BComboBox comboDenominator = null;
    private BPanel jPanel1 = null;
    private BLabel jLabel2 = null;
    private BButton btnOK = null;
    private BButton btnCancel = null;
    private BLabel jLabel4 = null;
    private BLabel jLabel7 = null;
    private BLabel jLabel8 = null;
    //SECTION-END-FIELD
    public FormBeatConfig(){
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
        this.setTitle("Beat Change");
        this.setSize(314, 287);
        this.setContentPane(getJContentPane());
        this.setTitle("JFrame");
    }

    /**
     * This method initializes jContentPane
     * 
     * @return javax.swing.BPanel
     */
    private BPanel getJContentPane() {
        if (jContentPane == null) {
            GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
            gridBagConstraints18.insets = new Insets(0, 0, 1, 12);
            gridBagConstraints18.gridy = 2;
            gridBagConstraints18.ipadx = 180;
            gridBagConstraints18.ipady = 42;
            gridBagConstraints18.weightx = 1.0D;
            gridBagConstraints18.weighty = 0.0D;
            gridBagConstraints18.anchor = GridBagConstraints.NORTH;
            gridBagConstraints18.fill = GridBagConstraints.BOTH;
            gridBagConstraints18.gridx = 0;
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.gridx = 0;
            gridBagConstraints8.ipadx = 141;
            gridBagConstraints8.ipady = 42;
            gridBagConstraints8.fill = GridBagConstraints.BOTH;
            gridBagConstraints8.insets = new Insets(0, 12, 0, 12);
            gridBagConstraints8.weightx = 1.0D;
            gridBagConstraints8.anchor = GridBagConstraints.NORTH;
            gridBagConstraints8.weighty = 0.5D;
            gridBagConstraints8.gridy = 1;
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 0;
            gridBagConstraints6.ipadx = 147;
            gridBagConstraints6.ipady = 41;
            gridBagConstraints6.insets = new Insets(12, 12, 12, 12);
            gridBagConstraints6.fill = GridBagConstraints.BOTH;
            gridBagConstraints6.weightx = 1.0D;
            gridBagConstraints6.anchor = GridBagConstraints.NORTH;
            gridBagConstraints6.weighty = 0.5D;
            gridBagConstraints6.gridy = 0;
            jContentPane = new BPanel();
            jContentPane.setLayout(new GridBagLayout());
            jContentPane.add(getGroupPosition(), gridBagConstraints6);
            jContentPane.add(getGroupBeat(), gridBagConstraints8);
            jContentPane.add(getBPanel1(), gridBagConstraints18);
        }
        return jContentPane;
    }

    /**
     * This method initializes groupPosition    
     *  
     * @return javax.swing.BPanel   
     */
    private BGroupBox getGroupPosition() {
        if (groupPosition == null) {
            GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
            gridBagConstraints61.gridx = 3;
            gridBagConstraints61.gridy = 2;
            jLabel8 = new BLabel();
            jLabel8.setText("     ");
            GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
            gridBagConstraints51.gridx = 3;
            gridBagConstraints51.gridy = 0;
            jLabel7 = new BLabel();
            jLabel7.setText("     ");
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints9.gridy = -1;
            gridBagConstraints9.weightx = 1.0;
            gridBagConstraints9.gridx = -1;
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 2;
            gridBagConstraints5.anchor = GridBagConstraints.WEST;
            gridBagConstraints5.insets = new Insets(0, 9, 0, 0);
            gridBagConstraints5.gridy = 2;
            lblBar2 = new BLabel();
            lblBar2.setText("Beat");
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints4.gridy = 2;
            gridBagConstraints4.weightx = 1.0;
            gridBagConstraints4.insets = new Insets(3, 0, 3, 0);
            gridBagConstraints4.gridx = 1;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.insets = new Insets(0, 16, 0, 0);
            gridBagConstraints3.gridy = 2;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 2;
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.insets = new Insets(0, 9, 0, 0);
            gridBagConstraints2.gridy = 0;
            lblBar1 = new BLabel();
            lblBar1.setText("Measure");
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weightx = 0.0D;
            gridBagConstraints1.insets = new Insets(3, 0, 3, 0);
            gridBagConstraints1.gridx = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.insets = new Insets(0, 16, 0, 0);
            gridBagConstraints.gridy = 0;
            lblStart = new BLabel();
            lblStart.setText("From");
            groupPosition = new BGroupBox();
            groupPosition.setLayout(new GridBagLayout());
            groupPosition.setTitle("Position");
            groupPosition.add(lblStart, gridBagConstraints);
            groupPosition.add(getNumStart(), gridBagConstraints1);
            groupPosition.add(lblBar1, gridBagConstraints2);
            groupPosition.add(getChkEnd(), gridBagConstraints3);
            groupPosition.add(getNumEnd(), gridBagConstraints4);
            groupPosition.add(lblBar2, gridBagConstraints5);
            groupPosition.add(jLabel7, gridBagConstraints51);
            groupPosition.add(jLabel8, gridBagConstraints61);
        }
        return groupPosition;
    }

    /**
     * This method initializes numStart 
     *  
     * @return javax.swing.BComboBox    
     */
    private BNumericUpDown getNumStart() {
        if (numStart == null) {
            numStart = new BNumericUpDown();
            numStart.setPreferredSize(new Dimension(31, 29));
        }
        return numStart;
    }

    /**
     * This method initializes chkEnd   
     *  
     * @return javax.swing.BCheckBox    
     */
    private BCheckBox getChkEnd() {
        if (chkEnd == null) {
            chkEnd = new BCheckBox();
            chkEnd.setText("To");
        }
        return chkEnd;
    }

    /**
     * This method initializes numEnd   
     *  
     * @return javax.swing.BComboBox    
     */
    private BNumericUpDown getNumEnd() {
        if (numEnd == null) {
            numEnd = new BNumericUpDown();
            numEnd.setPreferredSize(new Dimension(31, 29));
        }
        return numEnd;
    }

    /**
     * This method initializes groupBeat    
     *  
     * @return javax.swing.BPanel   
     */
    private BGroupBox getGroupBeat() {
        if (groupBeat == null) {
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.gridx = 4;
            gridBagConstraints13.gridy = 0;
            jLabel2 = new BLabel();
            jLabel2.setText("     ");
            GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
            gridBagConstraints12.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints12.gridy = 0;
            gridBagConstraints12.weightx = 0.5D;
            gridBagConstraints12.insets = new Insets(3, 0, 3, 0);
            gridBagConstraints12.gridx = 3;
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.gridx = 2;
            gridBagConstraints11.gridy = 0;
            jLabel1 = new BLabel();
            jLabel1.setText(" /    ");
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 1;
            gridBagConstraints10.gridy = 0;
            jLabel = new BLabel();
            jLabel.setText(" (1-255) ");
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints7.gridy = 0;
            gridBagConstraints7.weightx = 0.5D;
            gridBagConstraints7.insets = new Insets(3, 16, 3, 0);
            gridBagConstraints7.gridx = 0;
            groupBeat = new BGroupBox();
            groupBeat.setLayout(new GridBagLayout());
            groupBeat.setTitle("Position");
            groupBeat.add(getNumNumerator(), gridBagConstraints7);
            groupBeat.add(jLabel, gridBagConstraints10);
            groupBeat.add(jLabel1, gridBagConstraints11);
            groupBeat.add(getComboDenominator(), gridBagConstraints12);
            groupBeat.add(jLabel2, gridBagConstraints13);
        }
        return groupBeat;
    }

    /**
     * This method initializes numNumerator 
     *  
     * @return javax.swing.BComboBox    
     */
    private BNumericUpDown getNumNumerator() {
        if (numNumerator == null) {
            numNumerator = new BNumericUpDown();
            numNumerator.setPreferredSize(new Dimension(31, 29));
        }
        return numNumerator;
    }

    /**
     * This method initializes comboDenominator   
     *  
     * @return javax.swing.BComboBox    
     */
    private BComboBox getComboDenominator() {
        if (comboDenominator == null) {
            comboDenominator = new BComboBox();
            comboDenominator.setPreferredSize(new Dimension(31, 27));
        }
        return comboDenominator;
    }

    /**
     * This method initializes jPanel1  
     *  
     * @return javax.swing.BPanel   
     */
    private BPanel getBPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
            gridBagConstraints17.gridx = 0;
            gridBagConstraints17.fill = GridBagConstraints.BOTH;
            gridBagConstraints17.weightx = 1.0D;
            gridBagConstraints17.gridy = 0;
            jLabel4 = new BLabel();
            jLabel4.setText(" ");
            GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
            gridBagConstraints16.gridx = 2;
            gridBagConstraints16.anchor = GridBagConstraints.EAST;
            gridBagConstraints16.gridy = 0;
            GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
            gridBagConstraints15.gridx = 1;
            gridBagConstraints15.anchor = GridBagConstraints.EAST;
            gridBagConstraints15.gridy = 0;
            jPanel1 = new BPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getBtnOk(), gridBagConstraints15);
            jPanel1.add(getBtnCancel(), gridBagConstraints16);
            jPanel1.add(jLabel4, gridBagConstraints17);
        }
        return jPanel1;
    }

    /**
     * This method initializes btnOk    
     *  
     * @return javax.swing.BButton  
     */
    private BButton getBtnOk() {
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

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="15,16"
