package com.github.cadencii.ui;

//SECTION-BEGIN-IMPORT
import java.awt.CardLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextArea;
import com.github.cadencii.windows.forms.BButton;
import com.github.cadencii.windows.forms.BCheckBox;
import com.github.cadencii.windows.forms.BDialog;
import com.github.cadencii.windows.forms.BPanel;
import com.github.cadencii.windows.forms.BPictureBox;

//SECTION-END-IMPORT
public class VersionInfo extends BDialog {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private JPanel jPanel = null;
    private BPanel panelCredit = null;
    private JPanel jPanel1 = null;
    private JPanel jPanel2 = null;
    private BButton btnFlip = null;
    private BButton btnSaveAuthorList = null;
    private BButton btnOK = null;
    private JLabel jLabel1 = null;
    private JTextArea lblVstLogo = null;
    private BPictureBox pictVstLogo = null;
    private BCheckBox chkTwitterID = null;
    
    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public VersionInfo() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(327, 455));
        this.setMinimumSize(new Dimension(327, 455));
        this.setPreferredSize(new Dimension(327, 455));
        this.setResizable(false);
        this.setContentPane(getJPanel());
        setCancelButton( btnOK );
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.gridx = 1;
            gridBagConstraints11.weightx = 0.0D;
            gridBagConstraints11.insets = new Insets(0, 0, 12, 3);
            gridBagConstraints11.anchor = GridBagConstraints.EAST;
            gridBagConstraints11.gridy = 1;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 2;
            gridBagConstraints3.weightx = 0.0D;
            gridBagConstraints3.insets = new Insets(0, 3, 12, 12);
            gridBagConstraints3.anchor = GridBagConstraints.EAST;
            gridBagConstraints3.gridy = 1;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 1;
            gridBagConstraints2.weightx = 1.0D;
            gridBagConstraints2.gridy = 1;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.weightx = 1.0D;
            gridBagConstraints1.insets = new Insets(0, 12, 12, 0);
            gridBagConstraints1.anchor = GridBagConstraints.WEST;
            gridBagConstraints1.gridy = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridwidth = 3;
            gridBagConstraints.gridy = 0;
            gridBagConstraints.fill = GridBagConstraints.BOTH;
            gridBagConstraints.weightx = 1.0D;
            gridBagConstraints.weighty = 1.0D;
            gridBagConstraints.insets = new Insets(0, 0, 12, 0);
            gridBagConstraints.gridx = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getJPanel2(), gridBagConstraints);
            jPanel.add(getBtnFlip(), gridBagConstraints1);
            jPanel.add(getBtnSaveAuthorList(), gridBagConstraints2);
            jPanel.add(getChkTwitterID(), gridBagConstraints11);
            jPanel.add(getBtnOK(), gridBagConstraints3);
        }
        return jPanel;
    }

    /**
     * This method initializes panelCredit	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private BPanel getPanelCredit() {
        if (panelCredit == null) {
            panelCredit = new BPanel();
            panelCredit.setName("jScrollPane");
            panelCredit.setBackground(Color.white);
            panelCredit.setPreferredSize(new Dimension(327, 111));
        }
        return panelCredit;
    }

    /**
     * This method initializes jPanel1	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.fill = GridBagConstraints.BOTH;
            gridBagConstraints6.gridy = 2;
            gridBagConstraints6.weightx = 1.0;
            gridBagConstraints6.weighty = 0.0D;
            gridBagConstraints6.insets = new Insets(0, 12, 6, 12);
            gridBagConstraints6.gridx = 0;
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 0;
            gridBagConstraints5.weighty = 1.0D;
            gridBagConstraints5.gridy = 1;
            jLabel1 = new JLabel();
            jLabel1.setText(" ");
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints4.gridwidth = 1;
            gridBagConstraints4.anchor = GridBagConstraints.NORTH;
            gridBagConstraints4.weightx = 1.0D;
            gridBagConstraints4.weighty = 1.0D;
            gridBagConstraints4.gridy = 2;
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.setName("jPanel1");
            jPanel1.setBackground(Color.white);
            jPanel1.setPreferredSize(new Dimension(327, 111));
            jPanel1.add(jLabel1, gridBagConstraints5);
            jPanel1.add(getLblVstLogo(), gridBagConstraints6);
            jPanel1.add(getPictVstLogo(), new GridBagConstraints());
        }
        return jPanel1;
    }

    /**
     * This method initializes jPanel2	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel2() {
        if (jPanel2 == null) {
            jPanel2 = new JPanel();
            jPanel2.setLayout(new CardLayout());
            jPanel2.setPreferredSize(new Dimension(327, 111));
            jPanel2.add(getPanelCredit(), getPanelCredit().getName());
            jPanel2.add(getJPanel1(), getJPanel1().getName());
        }
        return jPanel2;
    }

    /**
     * This method initializes btnFlip	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnFlip() {
        if (btnFlip == null) {
            btnFlip = new BButton();
            btnFlip.setText("Credit");
            btnFlip.setPreferredSize(new Dimension(150, 29));
        }
        return btnFlip;
    }

    /**
     * This method initializes btnSaveAuthorList	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnSaveAuthorList() {
        if (btnSaveAuthorList == null) {
            btnSaveAuthorList = new BButton();
            btnSaveAuthorList.setVisible(false);
        }
        return btnSaveAuthorList;
    }

    /**
     * This method initializes btnOK	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnOK() {
        if (btnOK == null) {
            btnOK = new BButton();
            btnOK.setText("OK");
        }
        return btnOK;
    }

    /**
     * This method initializes lblVstLogo	
     * 	
     * @return javax.swing.JTextArea	
     */
    private JTextArea getLblVstLogo() {
        if (lblVstLogo == null) {
            lblVstLogo = new JTextArea();
            lblVstLogo.setText("VST PlugIn Technology by Steinberg Media Technologies GmbH");
            lblVstLogo.setLineWrap(true);
        }
        return lblVstLogo;
    }

    /**
     * This method initializes pictVstLogo	
     * 	
     * @return javax.swing.JPanel	
     */
    private BPictureBox getPictVstLogo() {
        if (pictVstLogo == null) {
            pictVstLogo = new BPictureBox();
            pictVstLogo.setLayout(new GridBagLayout());
        }
        return pictVstLogo;
    }

    /**
     * This method initializes chkTwitterID	
     * 	
     * @return javax.swing.JButton	
     */
    private BCheckBox getChkTwitterID() {
        if (chkTwitterID == null) {
            chkTwitterID = new BCheckBox();
            chkTwitterID.setText("TwtrID");
            chkTwitterID.setName("btnOK1");
        }
        return chkTwitterID;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
