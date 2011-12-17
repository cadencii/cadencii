package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import org.kbinani.windows.forms.BDialog;
import java.awt.Dimension;
import javax.swing.JPanel;
import java.awt.GridBagLayout;
import org.kbinani.windows.forms.BButton;
import java.awt.GridBagConstraints;
import java.awt.Insets;
import javax.swing.JLabel;
import javax.swing.JTextField;
import javax.swing.JComboBox;
import javax.swing.JCheckBox;

//SECTION-END-IMPORT
public class FormGenerateKeySound extends BDialog
{
    //SECTION-BEGIN-FIELD
    
    private static final long serialVersionUID = 3420499863033740708L;
    private JPanel jPanel1 = null;
    private BButton btnCancel = null;
    private BButton btnExecute = null;
    private JPanel jPanel = null;
    private JPanel jPanel2 = null;
    private JLabel lblDir = null;
    private JTextField txtDir = null;
    private BButton btnBrowse = null;
    private JLabel lblSingingSynthSystem = null;
    private JComboBox comboSingingSynthSystem = null;
    private JLabel lblSinger = null;
    private JComboBox comboSinger = null;
    private JCheckBox chkIgnoreExistingWavs = null;

    //SECTION-END-FIELD
    public FormGenerateKeySound() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    private void initialize() {
        this.setSize(new Dimension(382, 208));
        this.setContentPane(getJPanel());
    	setCancelButton( btnCancel );
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
            gridBagConstraints5.insets = new Insets(0, 0, 0, 12);
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.anchor = GridBagConstraints.WEST;
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.gridy = 0;
            gridBagConstraints4.insets = new Insets(0, 0, 0, 0);
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getBtnCancel(), gridBagConstraints4);
            jPanel1.add(getBtnExecute(), gridBagConstraints5);
        }
        return jPanel1;
    }

    /**
     * This method initializes btnCancel	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnCancel() {
        if (btnCancel == null) {
            btnCancel = new BButton();
            btnCancel.setText("Close");
            btnCancel.setPreferredSize(new Dimension(100, 29));
        }
        return btnCancel;
    }

    /**
     * This method initializes btnExecute	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnExecute() {
        if (btnExecute == null) {
            btnExecute = new BButton();
            btnExecute.setText("Execute");
            btnExecute.setPreferredSize(new Dimension(100, 29));
        }
        return btnExecute;
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.gridx = 0;
            gridBagConstraints11.weighty = 1.0D;
            gridBagConstraints11.gridwidth = 2;
            gridBagConstraints11.anchor = GridBagConstraints.EAST;
            gridBagConstraints11.gridy = 4;
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 0;
            gridBagConstraints10.weightx = 1.0D;
            gridBagConstraints10.gridwidth = 2;
            gridBagConstraints10.anchor = GridBagConstraints.WEST;
            gridBagConstraints10.insets = new Insets(6, 12, 6, 12);
            gridBagConstraints10.gridy = 2;
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.gridx = 0;
            gridBagConstraints9.weightx = 1.0D;
            gridBagConstraints9.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints9.anchor = GridBagConstraints.WEST;
            gridBagConstraints9.gridwidth = 2;
            gridBagConstraints9.gridy = 3;
            GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
            gridBagConstraints8.fill = GridBagConstraints.NONE;
            gridBagConstraints8.gridy = 1;
            gridBagConstraints8.weightx = 1.0;
            gridBagConstraints8.anchor = GridBagConstraints.WEST;
            gridBagConstraints8.insets = new Insets(4, 12, 4, 12);
            gridBagConstraints8.gridx = 1;
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.gridx = 0;
            gridBagConstraints7.anchor = GridBagConstraints.WEST;
            gridBagConstraints7.insets = new Insets(6, 12, 6, 12);
            gridBagConstraints7.gridy = 1;
            lblSinger = new JLabel();
            lblSinger.setText("Singer");
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.fill = GridBagConstraints.NONE;
            gridBagConstraints6.gridy = 0;
            gridBagConstraints6.weightx = 1.0;
            gridBagConstraints6.anchor = GridBagConstraints.WEST;
            gridBagConstraints6.insets = new Insets(12, 12, 4, 12);
            gridBagConstraints6.gridx = 1;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.insets = new Insets(12, 12, 6, 12);
            gridBagConstraints3.anchor = GridBagConstraints.WEST;
            gridBagConstraints3.gridy = 0;
            lblSingingSynthSystem = new JLabel();
            lblSingingSynthSystem.setText("Singing Synth. System");
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(lblSingingSynthSystem, gridBagConstraints3);
            jPanel.add(getComboSingingSynthSystem(), gridBagConstraints6);
            jPanel.add(lblSinger, gridBagConstraints7);
            jPanel.add(getComboSinger(), gridBagConstraints8);
            jPanel.add(getJPanel2(), gridBagConstraints9);
            jPanel.add(getChkIgnoreExistingWavs(), gridBagConstraints10);
            jPanel.add(getJPanel1(), gridBagConstraints11);
        }
        return jPanel;
    }

    /**
     * This method initializes jPanel2	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel2() {
        if (jPanel2 == null) {
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 2;
            gridBagConstraints2.insets = new Insets(0, 0, 0, 12);
            gridBagConstraints2.gridy = 0;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weightx = 1.0;
            gridBagConstraints1.insets = new Insets(0, 12, 0, 12);
            gridBagConstraints1.gridx = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints.gridy = 0;
            lblDir = new JLabel();
            lblDir.setText("Output Path");
            jPanel2 = new JPanel();
            jPanel2.setLayout(new GridBagLayout());
            jPanel2.add(lblDir, gridBagConstraints);
            jPanel2.add(getTxtDir(), gridBagConstraints1);
            jPanel2.add(getBtnBrowse(), gridBagConstraints2);
        }
        return jPanel2;
    }

    /**
     * This method initializes txtDir	
     * 	
     * @return javax.swing.JTextField	
     */
    private JTextField getTxtDir() {
        if (txtDir == null) {
            txtDir = new JTextField();
        }
        return txtDir;
    }

    /**
     * This method initializes btnBrowse	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnBrowse() {
        if (btnBrowse == null) {
            btnBrowse = new BButton();
            btnBrowse.setPreferredSize(new Dimension(41, 23));
            btnBrowse.setText("...");
        }
        return btnBrowse;
    }

    /**
     * This method initializes comboSingingSynthSystem	
     * 	
     * @return javax.swing.JComboBox	
     */
    private JComboBox getComboSingingSynthSystem() {
        if (comboSingingSynthSystem == null) {
            comboSingingSynthSystem = new JComboBox();
            comboSingingSynthSystem.setPreferredSize(new Dimension(121, 27));
        }
        return comboSingingSynthSystem;
    }

    /**
     * This method initializes comboSinger	
     * 	
     * @return javax.swing.JComboBox	
     */
    private JComboBox getComboSinger() {
        if (comboSinger == null) {
            comboSinger = new JComboBox();
            comboSinger.setPreferredSize(new Dimension(121, 27));
        }
        return comboSinger;
    }

    /**
     * This method initializes chkIgnoreExistingWavs	
     * 	
     * @return javax.swing.JCheckBox	
     */
    private JCheckBox getChkIgnoreExistingWavs() {
        if (chkIgnoreExistingWavs == null) {
            chkIgnoreExistingWavs = new JCheckBox();
            chkIgnoreExistingWavs.setText("Ignore Existing WAVs");
        }
        return chkIgnoreExistingWavs;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
