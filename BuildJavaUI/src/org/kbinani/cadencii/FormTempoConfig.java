package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BGroupBox;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BNumericUpDown;
import org.kbinani.windows.forms.BPanel;

//SECTION-END-IMPORT
public class FormTempoConfig extends BForm {
    //SECTION-BEGIN-FIELD
    
    private static final long serialVersionUID = 3025908775165151783L;
    private JPanel jPanel = null;
    private BGroupBox groupPosition = null;
    private BLabel lblBar = null;
    private BNumericUpDown numBar = null;
    private BLabel lblBeat = null;
    private BNumericUpDown numBeat = null;
    private BLabel lblClock = null;
    private BNumericUpDown numClock = null;
    private BGroupBox groupTempo = null;
    private BLabel lblTempoRange = null;
    private BNumericUpDown numTempo = null;
    private BPanel jPanel2 = null;
    private BButton btnOK = null;
    private BButton btnCancel = null;
    private BLabel jLabel4 = null;
    
    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public FormTempoConfig() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD
    
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(352, 213));
        this.setTitle("Global Tempo");
        this.setContentPane(getJPanel());
    		
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.gridx = 0;
            gridBagConstraints9.gridwidth = 2;
            gridBagConstraints9.anchor = GridBagConstraints.NORTHEAST;
            gridBagConstraints9.weightx = 1.0D;
            gridBagConstraints9.weighty = 1.0D;
            gridBagConstraints9.gridy = 1;
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.gridx = 1;
            gridBagConstraints8.fill = GridBagConstraints.BOTH;
            gridBagConstraints8.weightx = 1.0D;
            gridBagConstraints8.insets = new Insets(12, 6, 12, 12);
            gridBagConstraints8.gridy = 0;
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 0;
            gridBagConstraints6.fill = GridBagConstraints.BOTH;
            gridBagConstraints6.weightx = 1.0D;
            gridBagConstraints6.insets = new Insets(12, 12, 12, 6);
            gridBagConstraints6.gridy = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getGroupPosition(), gridBagConstraints6);
            jPanel.add(getGroupTempo(), gridBagConstraints8);
            jPanel.add(getJPanel2(), gridBagConstraints9);
        }
        return jPanel;
    }

    /**
     * This method initializes groupPosition	
     * 	
     * @return javax.swing.JPanel	
     */
    private BGroupBox getGroupPosition() {
        if (groupPosition == null) {
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.fill = GridBagConstraints.NONE;
            gridBagConstraints5.gridy = 2;
            gridBagConstraints5.weightx = 1.0;
            gridBagConstraints5.anchor = GridBagConstraints.WEST;
            gridBagConstraints5.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints5.gridx = 1;
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.anchor = GridBagConstraints.WEST;
            gridBagConstraints4.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints4.gridy = 2;
            lblClock = new BLabel();
            lblClock.setText("Clock");
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.fill = GridBagConstraints.NONE;
            gridBagConstraints3.gridy = 1;
            gridBagConstraints3.weightx = 1.0;
            gridBagConstraints3.anchor = GridBagConstraints.WEST;
            gridBagConstraints3.insets = new Insets(4, 12, 4, 0);
            gridBagConstraints3.gridx = 1;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints2.gridy = 1;
            lblBeat = new BLabel();
            lblBeat.setText("Beat");
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.NONE;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weightx = 1.0;
            gridBagConstraints1.anchor = GridBagConstraints.WEST;
            gridBagConstraints1.insets = new Insets(8, 12, 4, 0);
            gridBagConstraints1.gridx = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.anchor = GridBagConstraints.WEST;
            gridBagConstraints.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints.gridy = 0;
            lblBar = new BLabel();
            lblBar.setText("Measure");
            groupPosition = new BGroupBox();
            groupPosition.setLayout(new GridBagLayout());
            groupPosition.setTitle("Position");
            groupPosition.setPreferredSize(new Dimension(143, 113));
            groupPosition.add(lblBar, gridBagConstraints);
            groupPosition.add(getNumBar(), gridBagConstraints1);
            groupPosition.add(lblBeat, gridBagConstraints2);
            groupPosition.add(getNumBeat(), gridBagConstraints3);
            groupPosition.add(lblClock, gridBagConstraints4);
            groupPosition.add(getNumClock(), gridBagConstraints5);
        }
        return groupPosition;
    }

    /**
     * This method initializes numBar	
     * 	
     * @return javax.swing.JTextField	
     */
    private BNumericUpDown getNumBar() {
        if (numBar == null) {
            numBar = new BNumericUpDown();
            numBar.setPreferredSize(new Dimension(45, 20));
        }
        return numBar;
    }

    /**
     * This method initializes numBeat	
     * 	
     * @return javax.swing.JComboBox	
     */
    private BNumericUpDown getNumBeat() {
        if (numBeat == null) {
            numBeat = new BNumericUpDown();
            numBeat.setPreferredSize(new Dimension(45, 20));
        }
        return numBeat;
    }

    /**
     * This method initializes numClock	
     * 	
     * @return javax.swing.JComboBox	
     */
    private BNumericUpDown getNumClock() {
        if (numClock == null) {
            numClock = new BNumericUpDown();
            numClock.setPreferredSize(new Dimension(45, 20));
        }
        return numClock;
    }

    /**
     * This method initializes groupTempo	
     * 	
     * @return org.kbinani.windows.forms.BGroupBox	
     */
    private BGroupBox getGroupTempo() {
        if (groupTempo == null) {
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 1;
            gridBagConstraints10.weighty = 1.0D;
            gridBagConstraints10.gridy = 1;
            jLabel4 = new BLabel();
            jLabel4.setText(" ");
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.anchor = GridBagConstraints.WEST;
            gridBagConstraints11.insets = new Insets(8, 12, 4, 0);
            gridBagConstraints11.gridx = 1;
            gridBagConstraints11.gridy = 0;
            gridBagConstraints11.weightx = 0.0D;
            gridBagConstraints11.weighty = 0.0D;
            gridBagConstraints11.fill = GridBagConstraints.NONE;
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.anchor = GridBagConstraints.WEST;
            gridBagConstraints7.gridx = 2;
            gridBagConstraints7.gridy = 0;
            gridBagConstraints7.weightx = 1.0D;
            gridBagConstraints7.weighty = 0.0D;
            gridBagConstraints7.insets = new Insets(0, 12, 0, 0);
            lblTempoRange = new BLabel();
            lblTempoRange.setText("(20 to 300)");
            groupTempo = new BGroupBox();
            groupTempo.setLayout(new GridBagLayout());
            groupTempo.setTitle("Tempo");
            groupTempo.setPreferredSize(new Dimension(143, 113));
            groupTempo.add(lblTempoRange, gridBagConstraints7);
            groupTempo.add(getNumTempo(), gridBagConstraints11);
            groupTempo.add(jLabel4, gridBagConstraints10);
        }
        return groupTempo;
    }

    /**
     * This method initializes numTempo	
     * 	
     * @return org.kbinani.windows.forms.BNumericUpDown	
     */
    private BNumericUpDown getNumTempo() {
        if (numTempo == null) {
            numTempo = new BNumericUpDown();
            numTempo.setPreferredSize(new Dimension(59, 20));
            numTempo.setMaximum(300);
            numTempo.setValue(120);
            numTempo.setMinimum(20);
        }
        return numTempo;
    }

    /**
     * This method initializes jPanel2	
     * 	
     * @return org.kbinani.windows.forms.BPanel	
     */
    private BPanel getJPanel2() {
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
     * @return org.kbinani.windows.forms.BButton	
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
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnCancel() {
        if (btnCancel == null) {
            btnCancel = new BButton();
            btnCancel.setText("Cancel");
        }
        return btnCancel;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="-213,27"
