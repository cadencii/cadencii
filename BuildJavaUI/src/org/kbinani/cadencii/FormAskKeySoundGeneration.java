package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BLabel;

//SECTION-END-IMPORT
public class FormAskKeySoundGeneration extends BDialog {
//SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private JPanel jPanel = null;
    private BLabel lblMessage = null;
    private JPanel jPanel1 = null;
    private BButton btnYes = null;
    private BButton btnNo = null;
    private BCheckBox chkAlwaysPerformThisCheck = null;

    //SECTION-END-FIELD
    public FormAskKeySoundGeneration() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(376, 190));
        this.setContentPane(getJPanel());
        setCancelButton( btnNo );
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.weightx = 1.0D;
            gridBagConstraints4.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints4.ipadx = 0;
            gridBagConstraints4.ipady = 16;
            gridBagConstraints4.insets = new Insets(0, 0, 16, 0);
            gridBagConstraints4.gridy = 2;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.ipady = 16;
            gridBagConstraints1.anchor = GridBagConstraints.WEST;
            gridBagConstraints1.insets = new Insets(0, 16, 0, 0);
            gridBagConstraints1.gridy = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.weightx = 1.0D;
            gridBagConstraints.weighty = 1.0D;
            gridBagConstraints.fill = GridBagConstraints.BOTH;
            gridBagConstraints.ipadx = 0;
            gridBagConstraints.insets = new Insets(16, 16, 8, 16);
            gridBagConstraints.gridy = 0;
            lblMessage = new BLabel();
            lblMessage.setText("It seems some key-board sounds are missing.\nDo you want to re-generate them now?");
            lblMessage.setAutoEllipsis(true);
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(lblMessage, gridBagConstraints);
            jPanel.add(getJCheckBox(), gridBagConstraints1);
            jPanel.add(getJPanel1(), gridBagConstraints4);
        }
        return jPanel;
    }

    /**
     * This method initializes jPanel1	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.weightx = 1.0D;
            gridBagConstraints3.gridy = 0;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 1;
            gridBagConstraints2.weightx = 1.0D;
            gridBagConstraints2.gridy = 0;
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getJButton(), gridBagConstraints2);
            jPanel1.add(getJButton1(), gridBagConstraints3);
        }
        return jPanel1;
    }

    /**
     * This method initializes jButton	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getJButton() {
        if (btnYes == null) {
            btnYes = new BButton();
            btnYes.setText("Yes");
            btnYes.setPreferredSize(new Dimension(100, 29));
        }
        return btnYes;
    }

    /**
     * This method initializes jButton1	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getJButton1() {
        if (btnNo == null) {
            btnNo = new BButton();
            btnNo.setText("No");
            btnNo.setPreferredSize(new Dimension(100, 29));
        }
        return btnNo;
    }

    /**
     * This method initializes jCheckBox	
     * 	
     * @return javax.swing.JCheckBox	
     */
    private BCheckBox getJCheckBox() {
        if (chkAlwaysPerformThisCheck == null) {
            chkAlwaysPerformThisCheck = new BCheckBox();
            chkAlwaysPerformThisCheck.setText("Always perform this check when starting Cadencii.");
        }
        return chkAlwaysPerformThisCheck;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
