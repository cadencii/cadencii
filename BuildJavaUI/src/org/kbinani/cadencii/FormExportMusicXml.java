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
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BLabel;

//SECTION-END-IMPORT
public class FormExportMusicXml extends BForm {
//SECTION-BEGIN-FIELD

    /**
     * 
     */
    private static final long serialVersionUID = 1L;
    private BCheckBox chkChangeTempo = null;
    private JPanel jPanel = null;
    private BLabel lblDescription = null;
    private JPanel jPanel1 = null;
    private BButton btnOK = null;
    private BButton btnCancel = null;

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public FormExportMusicXml() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(353, 175));
        this.setTitle("Export MusicXML");
        this.setContentPane(getJPanel());
    		
    }

    /**
     * This method initializes chkChangeTempo	
     * 	
     * @return javax.swing.JCheckBox	
     */
    private BCheckBox getChkChangeTempo() {
        if (chkChangeTempo == null) {
            chkChangeTempo = new BCheckBox();
            chkChangeTempo.setText("Convert Gate-time");
        }
        return chkChangeTempo;
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.insets = new Insets(8, 16, 16, 16);
            gridBagConstraints2.anchor = GridBagConstraints.EAST;
            gridBagConstraints2.gridy = 2;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.fill = GridBagConstraints.BOTH;
            gridBagConstraints1.anchor = GridBagConstraints.WEST;
            gridBagConstraints1.weightx = 1.0D;
            gridBagConstraints1.weighty = 1.0D;
            gridBagConstraints1.insets = new Insets(8, 16, 0, 16);
            gridBagConstraints1.gridy = 1;
            lblDescription = new BLabel();
            lblDescription.setText("If checked, convert gate-time of notes in order to match the base-tempo");
            lblDescription.setVerticalAlignment(SwingConstants.TOP);
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.anchor = GridBagConstraints.WEST;
            gridBagConstraints.insets = new Insets(16, 16, 0, 16);
            gridBagConstraints.gridy = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getChkChangeTempo(), gridBagConstraints);
            jPanel.add(lblDescription, gridBagConstraints1);
            jPanel.add(getJPanel1(), gridBagConstraints2);
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
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.anchor = GridBagConstraints.WEST;
            gridBagConstraints5.gridx = 1;
            gridBagConstraints5.gridy = 0;
            gridBagConstraints5.insets = new Insets(0, 0, 0, 16);
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.anchor = GridBagConstraints.WEST;
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.gridy = 0;
            gridBagConstraints4.insets = new Insets(0, 0, 0, 16);
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getBtnOK(), gridBagConstraints4);
            jPanel1.add(getBtnCancel(), gridBagConstraints5);
        }
        return jPanel1;
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
}  //  @jve:decl-index=0:visual-constraint="10,10"
