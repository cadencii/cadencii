package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BListView;
import org.kbinani.windows.forms.BPanel;
import javax.swing.JScrollPane;

//SECTION-END-IMPORT
public class FormWordDictionary extends BForm {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private JPanel jPanel = null;
    private BLabel lblAvailableDictionaries = null;
    private BPanel jPanel2 = null;
    private BButton btnOK = null;
    private BButton btnCancel = null;
    private BPanel jPanel21 = null;
    private BButton btnUp = null;
    private BButton btnDown = null;
    private BListView listDictionaries = null;
    private JScrollPane jScrollPane = null;
    
    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public FormWordDictionary() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(327, 404));
        this.setTitle("User Dictionary Configuration");
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
            gridBagConstraints2.fill = GridBagConstraints.BOTH;
            gridBagConstraints2.gridy = 1;
            gridBagConstraints2.weightx = 1.0;
            gridBagConstraints2.weighty = 1.0;
            gridBagConstraints2.insets = new Insets(6, 12, 6, 12);
            gridBagConstraints2.gridx = 0;
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.anchor = GridBagConstraints.EAST;
            gridBagConstraints4.insets = new Insets(6, 0, 12, 16);
            gridBagConstraints4.gridy = 3;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.anchor = GridBagConstraints.EAST;
            gridBagConstraints3.insets = new Insets(6, 0, 6, 12);
            gridBagConstraints3.gridy = 2;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.anchor = GridBagConstraints.WEST;
            gridBagConstraints.insets = new Insets(12, 12, 6, 0);
            gridBagConstraints.gridy = 0;
            lblAvailableDictionaries = new BLabel();
            lblAvailableDictionaries.setText("Available Dictionaries");
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(lblAvailableDictionaries, gridBagConstraints);
            jPanel.add(getJPanel21(), gridBagConstraints3);
            jPanel.add(getJPanel2(), gridBagConstraints4);
            jPanel.add(getJScrollPane(), gridBagConstraints2);
        }
        return jPanel;
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
            gridBagConstraints52.insets = new Insets(0, 0, 0, 0);
            GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
            gridBagConstraints42.anchor = GridBagConstraints.WEST;
            gridBagConstraints42.gridx = 0;
            gridBagConstraints42.gridy = 0;
            gridBagConstraints42.insets = new Insets(0, 0, 0, 16);
            jPanel2 = new BPanel();
            jPanel2.setLayout(new GridBagLayout());
            jPanel2.setPreferredSize(new Dimension(132, 29));
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
            btnOK.setPreferredSize(new Dimension(49, 29));
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
            btnCancel.setPreferredSize(new Dimension(67, 29));
        }
        return btnCancel;
    }

    /**
     * This method initializes jPanel21	
     * 	
     * @return org.kbinani.windows.forms.BPanel	
     */
    private BPanel getJPanel21() {
        if (jPanel21 == null) {
            GridBagConstraints gridBagConstraints521 = new GridBagConstraints();
            gridBagConstraints521.anchor = GridBagConstraints.SOUTHWEST;
            gridBagConstraints521.gridx = 1;
            gridBagConstraints521.gridy = 0;
            gridBagConstraints521.insets = new Insets(0, 0, 0, 0);
            GridBagConstraints gridBagConstraints421 = new GridBagConstraints();
            gridBagConstraints421.anchor = GridBagConstraints.WEST;
            gridBagConstraints421.gridx = 0;
            gridBagConstraints421.gridy = 0;
            gridBagConstraints421.insets = new Insets(0, 0, 0, 16);
            jPanel21 = new BPanel();
            jPanel21.setLayout(new GridBagLayout());
            jPanel21.add(getBtnUp(), gridBagConstraints421);
            jPanel21.add(getBtnDown(), gridBagConstraints521);
        }
        return jPanel21;
    }

    /**
     * This method initializes btnUp	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnUp() {
        if (btnUp == null) {
            btnUp = new BButton();
            btnUp.setText("Up");
            btnUp.setPreferredSize(new Dimension(49, 29));
        }
        return btnUp;
    }

    /**
     * This method initializes btnDown	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnDown() {
        if (btnDown == null) {
            btnDown = new BButton();
            btnDown.setText("Down");
            btnDown.setPreferredSize(new Dimension(66, 29));
        }
        return btnDown;
    }

    /**
     * This method initializes listDictionaries	
     * 	
     * @return javax.swing.JPanel	
     */
    private BListView getListDictionaries() {
        if (listDictionaries == null) {
            listDictionaries = new BListView();
        }
        return listDictionaries;
    }

    /**
     * This method initializes jScrollPane	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private JScrollPane getJScrollPane() {
        if (jScrollPane == null) {
            jScrollPane = new JScrollPane();
            jScrollPane.setPreferredSize(new Dimension(100, 100));
            jScrollPane.setViewportView(getListDictionaries());
        }
        return jScrollPane;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
