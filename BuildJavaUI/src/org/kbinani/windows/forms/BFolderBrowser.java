package org.kbinani.windows.forms;

import java.awt.Dimension;
import javax.swing.JFileChooser;
import javax.swing.JTree;
import javax.swing.JScrollPane;
import javax.swing.BorderFactory;
import javax.swing.JPanel;
import java.awt.GridBagLayout;
import java.awt.GridBagConstraints;
import javax.swing.JLabel;
import javax.swing.JButton;
import java.awt.Insets;
import java.io.File;
import javax.swing.border.EtchedBorder;

public class BFolderBrowser extends BDialog {

    private static final long serialVersionUID = 1L;
    private JTree jTree = null;
    private JScrollPane jScrollPane = null;
    private JPanel jPanel = null;
    private JLabel lblMessage = null;
    private JButton btnNew = null;
    private JPanel jPanel1 = null;
    private BButton btnOK = null;
    private BButton btnCancel = null;
    private JFileChooser dialog = null;
    
    public boolean isNewFolderButtonVisible(){
        return btnNew.isVisible();
    }
    
    public void setNewFolderButtonVisible( boolean value ){
        btnNew.setVisible( value );
    }
    
    public String getDescription(){
        return dialog.getDialogTitle();
    }
    
    public void setDescription( String value ){
        dialog.setDialogTitle( value );
    }
    
    public void setSelectedPath( String path ){
        dialog.setSelectedFile( new File( path ) );
    }
    
    public String getSelectedPath(){
        return dialog.getSelectedFile().getAbsolutePath();
    }
    
    //TODO:
    /**
     * オーバーライド。jTreeの実装が完了したら消すこと
     */
    public void setVisible( boolean value ){
        dialog.setVisible( value );
    }
    
    //TODO:
    /**
     * オーバーライド。jTreeの実装が完了したら消すこと
     */
    public boolean isVisible(){
        return dialog.isVisible();
    }
    
    /**
     * This method initializes 
     * 
     */
    public BFolderBrowser() {
    	super();
    	initialize();
    	dialog = new JFileChooser();
    }
    
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(341, 416));
        this.setModal(true);
        this.setContentPane(getJPanel());
    }

    /**
     * This method initializes jTree	
     * 	
     * @return javax.swing.JTree	
     */
    private JTree getJTree() {
        if (jTree == null) {
            jTree = new JTree();
        }
        return jTree;
    }

    /**
     * This method initializes jScrollPane	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private JScrollPane getJScrollPane() {
        if (jScrollPane == null) {
            jScrollPane = new JScrollPane();
            jScrollPane.setViewportView(getJTree());
            jScrollPane.setBorder(BorderFactory.createEtchedBorder(EtchedBorder.RAISED));
        }
        return jScrollPane;
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 2;
            gridBagConstraints4.insets = new Insets(12, 0, 12, 12);
            gridBagConstraints4.anchor = GridBagConstraints.EAST;
            gridBagConstraints4.gridy = 3;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.insets = new Insets(12, 12, 12, 0);
            gridBagConstraints2.gridy = 3;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.gridwidth = 3;
            gridBagConstraints1.anchor = GridBagConstraints.WEST;
            gridBagConstraints1.insets = new Insets(16, 12, 0, 12);
            gridBagConstraints1.gridy = 0;
            lblMessage = new JLabel();
            lblMessage.setText(" ");
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.fill = GridBagConstraints.BOTH;
            gridBagConstraints.weighty = 1.0;
            gridBagConstraints.gridx = 0;
            gridBagConstraints.gridy = 1;
            gridBagConstraints.gridwidth = 3;
            gridBagConstraints.insets = new Insets(12, 12, 0, 12);
            gridBagConstraints.weightx = 1.0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(lblMessage, gridBagConstraints1);
            jPanel.add(getJScrollPane(), gridBagConstraints);
            jPanel.add(getBtnNew(), gridBagConstraints2);
            jPanel.add(getJPanel1(), gridBagConstraints4);
        }
        return jPanel;
    }

    /**
     * This method initializes btnNew	
     * 	
     * @return javax.swing.JButton	
     */
    private JButton getBtnNew() {
        if (btnNew == null) {
            btnNew = new JButton();
            btnNew.setText("Create new folder");
        }
        return btnNew;
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
            gridBagConstraints5.insets = new Insets(0, 0, 0, 0);
            GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
            gridBagConstraints41.anchor = GridBagConstraints.WEST;
            gridBagConstraints41.gridx = 0;
            gridBagConstraints41.gridy = 0;
            gridBagConstraints41.insets = new Insets(0, 0, 0, 16);
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getBtnOK(), gridBagConstraints41);
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

}  //  @jve:decl-index=0:visual-constraint="10,10"
