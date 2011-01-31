package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JLabel;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BTextBox;
import org.kbinani.windows.forms.BLabel;

//SECTION-END-IMPORT
public class FormCurvePointEdit extends BForm {
    //SECTION-BEGIN-FIELD
    
    private static final long serialVersionUID = 1L;
    private JPanel jPanel = null;
    private BButton btnBackward3 = null;
    private BButton btnBackward2 = null;
    private BButton btnBackward = null;
    private BButton btnForward = null;
    private BButton btnForward2 = null;
    private BButton btnForward3 = null;
    private JPanel jPanel1 = null;
    private JLabel lblDataPointValue = null;
    private BTextBox txtDataPointValue = null;
    private BButton btnUndo = null;
    private JLabel lblDataPointClock = null;
    private BTextBox txtDataPointClock = null;
    private BButton btnRedo = null;
    private JPanel jPanel3 = null;
    private BButton btnExit = null;
    private BButton btnApply = null;
    private BLabel lblRightValue = null;

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public FormCurvePointEdit() {
    	super();
    	initialize();
    }

    //SECTION-BEGIN-METHOD
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(328, 195));
        this.setTitle("FormCurvePointEdit");
        this.setContentPane(getJPanel1());
    		
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 5;
            gridBagConstraints5.insets = new Insets(0, 1, 0, 1);
            gridBagConstraints5.gridy = 0;
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 4;
            gridBagConstraints4.insets = new Insets(0, 1, 0, 1);
            gridBagConstraints4.gridy = 0;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 3;
            gridBagConstraints3.insets = new Insets(0, 1, 0, 1);
            gridBagConstraints3.gridy = 0;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 2;
            gridBagConstraints2.insets = new Insets(0, 1, 0, 1);
            gridBagConstraints2.gridy = 0;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 1;
            gridBagConstraints1.insets = new Insets(0, 1, 0, 1);
            gridBagConstraints1.gridy = 0;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.insets = new Insets(0, 1, 0, 1);
            gridBagConstraints.gridy = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getBtnBackward3(), gridBagConstraints);
            jPanel.add(getBtnBackward2(), gridBagConstraints1);
            jPanel.add(getBtnBackward(), gridBagConstraints2);
            jPanel.add(getBtnForward(), gridBagConstraints3);
            jPanel.add(getBtnForward2(), gridBagConstraints4);
            jPanel.add(getBtnForward3(), gridBagConstraints5);
        }
        return jPanel;
    }

    /**
     * This method initializes btnBackward3	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnBackward3() {
        if (btnBackward3 == null) {
            btnBackward3 = new BButton();
            btnBackward3.setText("<10");
            btnBackward3.setPreferredSize(new Dimension(55, 29));
        }
        return btnBackward3;
    }

    /**
     * This method initializes btnBackward2	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnBackward2() {
        if (btnBackward2 == null) {
            btnBackward2 = new BButton();
            btnBackward2.setText("<5");
            btnBackward2.setPreferredSize(new Dimension(48, 29));
        }
        return btnBackward2;
    }

    /**
     * This method initializes btnBackward	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnBackward() {
        if (btnBackward == null) {
            btnBackward = new BButton();
            btnBackward.setText("<");
            btnBackward.setPreferredSize(new Dimension(41, 29));
        }
        return btnBackward;
    }

    /**
     * This method initializes btnForward	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnForward() {
        if (btnForward == null) {
            btnForward = new BButton();
            btnForward.setText(">");
            btnForward.setPreferredSize(new Dimension(41, 29));
        }
        return btnForward;
    }

    /**
     * This method initializes btnForward2	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnForward2() {
        if (btnForward2 == null) {
            btnForward2 = new BButton();
            btnForward2.setText("5>");
            btnForward2.setPreferredSize(new Dimension(48, 29));
        }
        return btnForward2;
    }

    /**
     * This method initializes btnForward3	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnForward3() {
        if (btnForward3 == null) {
            btnForward3 = new BButton();
            btnForward3.setText("10>");
            btnForward3.setPreferredSize(new Dimension(55, 29));
        }
        return btnForward3;
    }

    /**
     * This method initializes jPanel1	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.gridx = 0;
            gridBagConstraints13.gridwidth = 3;
            gridBagConstraints13.anchor = GridBagConstraints.EAST;
            gridBagConstraints13.insets = new Insets(12, 0, 12, 0);
            gridBagConstraints13.weighty = 1.0D;
            gridBagConstraints13.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints13.gridy = 3;
            GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
            gridBagConstraints12.gridx = 2;
            gridBagConstraints12.insets = new Insets(4, 0, 0, 12);
            gridBagConstraints12.gridy = 2;
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints11.gridy = 2;
            gridBagConstraints11.weightx = 1.0;
            gridBagConstraints11.insets = new Insets(4, 12, 0, 0);
            gridBagConstraints11.gridx = 1;
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 0;
            gridBagConstraints10.anchor = GridBagConstraints.NORTHEAST;
            gridBagConstraints10.insets = new Insets(4, 0, 0, 0);
            gridBagConstraints10.gridy = 2;
            lblDataPointClock = new JLabel();
            lblDataPointClock.setText("Clock");
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.gridx = 2;
            gridBagConstraints9.insets = new Insets(4, 0, 0, 12);
            gridBagConstraints9.gridy = 1;
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.gridwidth = 3;
            gridBagConstraints8.gridy = 0;
            gridBagConstraints8.insets = new Insets(12, 0, 12, 0);
            gridBagConstraints8.gridx = 0;
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints7.gridy = 1;
            gridBagConstraints7.weightx = 1.0;
            gridBagConstraints7.insets = new Insets(4, 12, 0, 0);
            gridBagConstraints7.gridx = 1;
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 0;
            gridBagConstraints6.anchor = GridBagConstraints.EAST;
            gridBagConstraints6.weightx = 1.0D;
            gridBagConstraints6.insets = new Insets(4, 0, 0, 0);
            gridBagConstraints6.gridy = 1;
            lblDataPointValue = new JLabel();
            lblDataPointValue.setText("Value");
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getJPanel(), gridBagConstraints8);
            jPanel1.add(lblDataPointValue, gridBagConstraints6);
            jPanel1.add(getTxtDataPointValue(), gridBagConstraints7);
            jPanel1.add(getBtnUndo(), gridBagConstraints9);
            jPanel1.add(lblDataPointClock, gridBagConstraints10);
            jPanel1.add(getTxtDataPointClock(), gridBagConstraints11);
            jPanel1.add(getBtnRedo(), gridBagConstraints12);
            jPanel1.add(getJPanel3(), gridBagConstraints13);
        }
        return jPanel1;
    }

    /**
     * This method initializes txtDataPointValue	
     * 	
     * @return javax.swing.JTextField	
     */
    private BTextBox getTxtDataPointValue() {
        if (txtDataPointValue == null) {
            txtDataPointValue = new BTextBox();
            txtDataPointValue.setPreferredSize(new Dimension(71, 20));
        }
        return txtDataPointValue;
    }

    /**
     * This method initializes btnUndo	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnUndo() {
        if (btnUndo == null) {
            btnUndo = new BButton();
            btnUndo.setText("undo");
            btnUndo.setPreferredSize(new Dimension(63, 29));
        }
        return btnUndo;
    }

    /**
     * This method initializes txtDataPointClock	
     * 	
     * @return javax.swing.JTextField	
     */
    private BTextBox getTxtDataPointClock() {
        if (txtDataPointClock == null) {
            txtDataPointClock = new BTextBox();
            txtDataPointClock.setPreferredSize(new Dimension(71, 20));
        }
        return txtDataPointClock;
    }

    /**
     * This method initializes btnRedo	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnRedo() {
        if (btnRedo == null) {
            btnRedo = new BButton();
            btnRedo.setText("redo");
            btnRedo.setPreferredSize(new Dimension(63, 29));
        }
        return btnRedo;
    }

    /**
     * This method initializes jPanel3	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel3() {
        if (jPanel3 == null) {
            GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
            gridBagConstraints14.gridx = 0;
            gridBagConstraints14.weightx = 1.0D;
            gridBagConstraints14.gridy = 0;
            lblRightValue = new BLabel();
            lblRightValue.setText("");
            lblRightValue.setPreferredSize(new Dimension(4, 4));
            GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
            gridBagConstraints111.insets = new Insets(0, 0, 0, 12);
            gridBagConstraints111.gridy = 0;
            gridBagConstraints111.gridx = 2;
            GridBagConstraints gridBagConstraints121 = new GridBagConstraints();
            gridBagConstraints121.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints121.gridy = 0;
            gridBagConstraints121.gridx = 1;
            jPanel3 = new JPanel();
            jPanel3.setLayout(new GridBagLayout());
            jPanel3.add(getBtnExit(), gridBagConstraints121);
            jPanel3.add(getBtnApply(), gridBagConstraints111);
            jPanel3.add(lblRightValue, gridBagConstraints14);
        }
        return jPanel3;
    }

    /**
     * This method initializes btnExit	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnExit() {
        if (btnExit == null) {
            btnExit = new BButton();
            btnExit.setText("Exit");
            btnExit.setPreferredSize(new Dimension(100, 29));
        }
        return btnExit;
    }

    /**
     * This method initializes btnApply	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnApply() {
        if (btnApply == null) {
            btnApply = new BButton();
            btnApply.setText("Apply");
            btnApply.setPreferredSize(new Dimension(100, 29));
        }
        return btnApply;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
