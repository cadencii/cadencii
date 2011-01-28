package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BPanel;
import org.kbinani.windows.forms.BPictureBox;
import org.kbinani.windows.forms.BProgressBar;

//SECTION-END-IMPORT
public class FormGameControlerConfig extends BForm {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private BPanel BPanel = null;
    private BLabel lblMessage = null;
    private BPictureBox pictButton = null;
    private BProgressBar progressCount = null;
    private BButton btnSkip = null;
    private BButton btnReset = null;
    private BPanel jPanel11 = null;
    private BButton btnOK = null;
    private BButton btnCancel = null;
    private BLabel jLabel4 = null;

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public FormGameControlerConfig() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(356, 224));
        this.setTitle("Game Controler Configuration");
        this.setContentPane(getJPanel());
    		
    }

    /**
     * This method initializes BPanel	
     * 	
     * @return javax.swing.BPanel	
     */
    private BPanel getJPanel() {
        if (BPanel == null) {
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 0;
            gridBagConstraints5.gridwidth = 2;
            gridBagConstraints5.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints5.weightx = 1.0D;
            gridBagConstraints5.weighty = 1.0D;
            gridBagConstraints5.anchor = GridBagConstraints.NORTH;
            gridBagConstraints5.insets = new Insets(12, 0, 12, 0);
            gridBagConstraints5.gridy = 4;
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 1;
            gridBagConstraints4.weightx = 0.5D;
            gridBagConstraints4.anchor = GridBagConstraints.EAST;
            gridBagConstraints4.insets = new Insets(0, 12, 0, 12);
            gridBagConstraints4.gridy = 3;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.weightx = 0.5D;
            gridBagConstraints3.anchor = GridBagConstraints.WEST;
            gridBagConstraints3.insets = new Insets(0, 24, 0, 12);
            gridBagConstraints3.gridy = 3;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.gridwidth = 2;
            gridBagConstraints2.weightx = 1.0D;
            gridBagConstraints2.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints2.insets = new Insets(12, 12, 12, 12);
            gridBagConstraints2.gridy = 2;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1.ipadx = 1;
            gridBagConstraints1.gridwidth = 2;
            gridBagConstraints1.gridy = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints.gridwidth = 2;
            gridBagConstraints.insets = new Insets(16, 12, 0, 12);
            gridBagConstraints.gridy = 0;
            lblMessage = new BLabel();
            lblMessage.setText(" ");
            BPanel = new BPanel();
            BPanel.setLayout(new GridBagLayout());
            BPanel.add(lblMessage, gridBagConstraints);
            BPanel.add(getPictButton(), gridBagConstraints1);
            BPanel.add(getProgressCount(), gridBagConstraints2);
            BPanel.add(getBtnSkip(), gridBagConstraints3);
            BPanel.add(getBtnReset(), gridBagConstraints4);
            BPanel.add(getJPanel11(), gridBagConstraints5);
        }
        return BPanel;
    }

    /**
     * This method initializes pictButton	
     * 	
     * @return javax.swing.BPanel	
     */
    private BPictureBox getPictButton() {
        if (pictButton == null) {
            pictButton = new BPictureBox();
            pictButton.setLayout(new GridBagLayout());
        }
        return pictButton;
    }

    /**
     * This method initializes progressCount	
     * 	
     * @return javax.swing.BProgressBar	
     */
    private BProgressBar getProgressCount() {
        if (progressCount == null) {
            progressCount = new BProgressBar();
        }
        return progressCount;
    }

    /**
     * This method initializes btnSkip	
     * 	
     * @return javax.swing.BButton	
     */
    private BButton getBtnSkip() {
        if (btnSkip == null) {
            btnSkip = new BButton();
            btnSkip.setText("Skip");
        }
        return btnSkip;
    }

    /**
     * This method initializes btnReset	
     * 	
     * @return javax.swing.BButton	
     */
    private BButton getBtnReset() {
        if (btnReset == null) {
            btnReset = new BButton();
            btnReset.setText("Reset and Exit");
        }
        return btnReset;
    }

    /**
     * This method initializes jPanel11	
     * 	
     * @return org.kbinani.windows.forms.BPanel	
     */
    private BPanel getJPanel11() {
        if (jPanel11 == null) {
            GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
            gridBagConstraints17.fill = GridBagConstraints.BOTH;
            gridBagConstraints17.gridy = 0;
            gridBagConstraints17.weightx = 1.0D;
            gridBagConstraints17.gridx = 0;
            jLabel4 = new BLabel();
            jLabel4.setText(" ");
            GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
            gridBagConstraints16.anchor = GridBagConstraints.EAST;
            gridBagConstraints16.gridy = 0;
            gridBagConstraints16.insets = new Insets(0, 8, 0, 12);
            gridBagConstraints16.gridx = 2;
            GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
            gridBagConstraints15.anchor = GridBagConstraints.EAST;
            gridBagConstraints15.gridy = 0;
            gridBagConstraints15.insets = new Insets(0, 0, 0, 8);
            gridBagConstraints15.gridx = 1;
            jPanel11 = new BPanel();
            jPanel11.setLayout(new GridBagLayout());
            jPanel11.add(getBtnOK(), gridBagConstraints15);
            jPanel11.add(getBtnCancel(), gridBagConstraints16);
            jPanel11.add(jLabel4, gridBagConstraints17);
        }
        return jPanel11;
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

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
