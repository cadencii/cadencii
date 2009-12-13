package org.kbinani.Cadencii;

//SECTION-BEGIN-IMPORT
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BListView;
import java.awt.Dimension;
import javax.swing.JPanel;
import java.awt.GridBagLayout;
import java.awt.GridBagConstraints;
import org.kbinani.windows.forms.BButton;
import java.awt.Insets;

//SECTION-END-IMPORT
public class FormShortcutKeys extends BForm {
    //SECTION-BEGIN-FIELD
 
    private static final long serialVersionUID = 2743132471603994391L;
    private JPanel jPanel = null;
    private BListView list = null;
    private JPanel jPanel3 = null;
    private BButton btnLoadDefault = null;
    private BButton btnRevert = null;
    private JPanel jPanel31 = null;
    private BButton btnCancel = null;
    private BButton btnOK = null;

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public FormShortcutKeys() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(396, 468));
        this.setTitle("Shortcut Config");
        this.setContentPane(getJPanel());
    		
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
            gridBagConstraints2.anchor = GridBagConstraints.EAST;
            gridBagConstraints2.insets = new Insets(0, 0, 16, 12);
            gridBagConstraints2.gridy = 2;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.anchor = GridBagConstraints.WEST;
            gridBagConstraints1.insets = new Insets(0, 12, 12, 0);
            gridBagConstraints1.gridy = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.fill = GridBagConstraints.BOTH;
            gridBagConstraints.weightx = 1.0D;
            gridBagConstraints.weighty = 1.0D;
            gridBagConstraints.insets = new Insets(12, 12, 6, 12);
            gridBagConstraints.gridy = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getList(), gridBagConstraints);
            jPanel.add(getJPanel3(), gridBagConstraints1);
            jPanel.add(getJPanel31(), gridBagConstraints2);
        }
        return jPanel;
    }

    /**
     * This method initializes list	
     * 	
     * @return javax.swing.JPanel	
     */
    private BListView getList() {
        if (list == null) {
            list = new BListView();
        }
        return list;
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
            jPanel3.add(getBtnLoadDefault(), gridBagConstraints121);
            jPanel3.add(getBtnRevert(), gridBagConstraints111);
        }
        return jPanel3;
    }

    /**
     * This method initializes btnLoadDefault	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnLoadDefault() {
        if (btnLoadDefault == null) {
            btnLoadDefault = new BButton();
            btnLoadDefault.setText("Load Default");
        }
        return btnLoadDefault;
    }

    /**
     * This method initializes btnRevert	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnRevert() {
        if (btnRevert == null) {
            btnRevert = new BButton();
            btnRevert.setText("Revert");
        }
        return btnRevert;
    }

    /**
     * This method initializes jPanel31	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel31() {
        if (jPanel31 == null) {
            GridBagConstraints gridBagConstraints1111 = new GridBagConstraints();
            gridBagConstraints1111.insets = new Insets(0, 0, 0, 16);
            gridBagConstraints1111.gridy = 0;
            gridBagConstraints1111.gridx = 0;
            GridBagConstraints gridBagConstraints1211 = new GridBagConstraints();
            gridBagConstraints1211.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints1211.gridy = 0;
            gridBagConstraints1211.gridx = 2;
            jPanel31 = new JPanel();
            jPanel31.setLayout(new GridBagLayout());
            jPanel31.add(getBtnCancel(), gridBagConstraints1211);
            jPanel31.add(getBtnOK(), gridBagConstraints1111);
        }
        return jPanel31;
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

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
