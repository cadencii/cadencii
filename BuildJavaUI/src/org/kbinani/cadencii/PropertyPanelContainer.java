package org.kbinani.cadencii;
//SECTION-BEGIN-IMPORT

import java.awt.Component;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.BorderFactory;
import javax.swing.ImageIcon;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BPanel;

//SECTION-END-IMPORT
public class PropertyPanelContainer extends BPanel {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 2607395876517475240L;
    private BPanel panelTitle = null;
    private BButton btnClose = null;
    private BButton btnWindow = null;
    private JPanel panelMain = null;

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public PropertyPanelContainer() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    public void addComponent( Component comp ){
        GridBagConstraints gridBagConstraints = new GridBagConstraints();
        gridBagConstraints.gridx = 0;
        gridBagConstraints.fill = GridBagConstraints.BOTH;
        gridBagConstraints.weightx = 1.0D;
        gridBagConstraints.weighty = 1.0D;
        gridBagConstraints.anchor = GridBagConstraints.NORTH;
        gridBagConstraints.gridy = 0;
        getPanelMain().add( comp, gridBagConstraints);
    }
    
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
        gridBagConstraints1.gridx = 0;
        gridBagConstraints1.weighty = 1.0D;
        gridBagConstraints1.weightx = 1.0D;
        gridBagConstraints1.fill = GridBagConstraints.BOTH;
        gridBagConstraints1.gridy = 1;
        GridBagConstraints gridBagConstraints = new GridBagConstraints();
        gridBagConstraints.gridx = 0;
        gridBagConstraints.weightx = 1.0D;
        gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
        gridBagConstraints.gridy = 0;
        this.setLayout(new GridBagLayout());
        this.setSize(new Dimension(205, 223));
        this.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
        this.add(getPanelTitle(), gridBagConstraints);
        this.add(getPanelMain(), gridBagConstraints1);
    		
    }

    /**
     * This method initializes panelTitle	
     * 	
     * @return javax.swing.JPanel	
     */
    private BPanel getPanelTitle() {
        if (panelTitle == null) {
            GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
            gridBagConstraints111.insets = new Insets(3, 0, 3, 3);
            gridBagConstraints111.gridy = 0;
            gridBagConstraints111.anchor = GridBagConstraints.EAST;
            gridBagConstraints111.weightx = 1.0D;
            gridBagConstraints111.gridx = 1;
            GridBagConstraints gridBagConstraints1211 = new GridBagConstraints();
            gridBagConstraints1211.insets = new Insets(3, 0, 3, 3);
            gridBagConstraints1211.gridy = 0;
            gridBagConstraints1211.anchor = GridBagConstraints.EAST;
            gridBagConstraints1211.gridx = 3;
            panelTitle = new BPanel();
            panelTitle.setLayout(new GridBagLayout());
            panelTitle.setPreferredSize(new Dimension(156, 29));
            panelTitle.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            panelTitle.add(getBtnClose(), gridBagConstraints1211);
            panelTitle.add(getBtnWindow(), gridBagConstraints111);
        }
        return panelTitle;
    }

    /**
     * This method initializes btnClose	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnClose() {
        if (btnClose == null) {
            btnClose = new BButton();
            btnClose.setText("");
            btnClose.setPreferredSize(new Dimension(23, 23));
        }
        return btnClose;
    }

    /**
     * This method initializes btnWindow	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnWindow() {
        if (btnWindow == null) {
            btnWindow = new BButton();
            btnWindow.setText("");
            btnWindow.setPreferredSize(new Dimension(23, 23));
        }
        return btnWindow;
    }

    /**
     * This method initializes panelMain	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getPanelMain() {
        if (panelMain == null) {
            panelMain = new JPanel();
            panelMain.setLayout(new GridBagLayout());
            panelMain.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
        }
        return panelMain;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="51,-10"
