package com.github.cadencii.ui;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.Font;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JLabel;
import javax.swing.JPanel;
import com.github.cadencii.windows.forms.BButton;
import com.github.cadencii.windows.forms.BForm;
import com.github.cadencii.windows.forms.BLabel;
import com.github.cadencii.windows.forms.BNumericUpDown;

//SECTION-END-IMPORT
public class FormRealtimeConfig extends BForm {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private JLabel lblRealTimeInput = null;
    private JPanel jPanel = null;
    private BButton btnStart = null;
    private BButton btnCancel = null;
    private JPanel jPanel1 = null;
    private BLabel lblSpeed = null;
    private BNumericUpDown numSpeed = null;
    private JPanel jPanel2 = null;

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public FormRealtimeConfig() {
    	super();
    	initialize();
    }

    //SECTION-BEGIN-METHOD
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        lblRealTimeInput = new JLabel();
        lblRealTimeInput.setText("Realtime Input");
        lblRealTimeInput.setFont(new Font("Dialog", Font.PLAIN, 18));
        this.setSize(new Dimension(320, 182));
        this.setContentPane(getJPanel());
    		
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 1;
            gridBagConstraints6.weightx = 1.0D;
            gridBagConstraints6.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints6.gridy = 1;
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 0;
            gridBagConstraints5.gridwidth = 2;
            gridBagConstraints5.weightx = 1.0D;
            gridBagConstraints5.fill = GridBagConstraints.NONE;
            gridBagConstraints5.weighty = 1.0D;
            gridBagConstraints5.gridy = 2;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.gridwidth = 2;
            gridBagConstraints.insets = new Insets(12, 0, 12, 0);
            gridBagConstraints.gridy = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(lblRealTimeInput, gridBagConstraints);
            jPanel.add(getJPanel1(), gridBagConstraints5);
            jPanel.add(getJPanel2(), gridBagConstraints6);
        }
        return jPanel;
    }

    /**
     * This method initializes btnStart	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnStart() {
        if (btnStart == null) {
            btnStart = new BButton();
            btnStart.setText("Start");
            btnStart.setPreferredSize(new Dimension(120, 33));
            btnStart.setFont(new Font("Dialog", Font.PLAIN, 12));
        }
        return btnStart;
    }

    /**
     * This method initializes btnCancel	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnCancel() {
        if (btnCancel == null) {
            btnCancel = new BButton();
            btnCancel.setText("Cancel");
            btnCancel.setPreferredSize(new Dimension(120, 33));
            btnCancel.setFont(new Font("Dialog", Font.PLAIN, 12));
        }
        return btnCancel;
    }

    /**
     * This method initializes jPanel1	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.fill = GridBagConstraints.NONE;
            gridBagConstraints4.gridy = 0;
            gridBagConstraints4.weightx = 1.0;
            gridBagConstraints4.insets = new Insets(0, 6, 0, 0);
            gridBagConstraints4.gridx = 1;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.insets = new Insets(0, 0, 0, 6);
            gridBagConstraints3.gridy = 0;
            lblSpeed = new BLabel();
            lblSpeed.setText("Speed");
            lblSpeed.setFont(new Font("Dialog", Font.PLAIN, 12));
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(lblSpeed, gridBagConstraints3);
            jPanel1.add(getNumSpeed(), gridBagConstraints4);
        }
        return jPanel1;
    }

    /**
     * This method initializes numSpeed	
     * 	
     * @return javax.swing.JComboBox	
     */
    private BNumericUpDown getNumSpeed() {
        if (numSpeed == null) {
            numSpeed = new BNumericUpDown();
            numSpeed.setPreferredSize(new Dimension(120, 19));
        }
        return numSpeed;
    }

    /**
     * This method initializes jPanel2	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel2() {
        if (jPanel2 == null) {
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 1;
            gridBagConstraints2.weightx = 1.0D;
            gridBagConstraints2.insets = new Insets(0, 12, 12, 0);
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.gridy = 0;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.weightx = 1.0D;
            gridBagConstraints1.insets = new Insets(0, 0, 12, 12);
            gridBagConstraints1.anchor = GridBagConstraints.EAST;
            gridBagConstraints1.gridy = 0;
            jPanel2 = new JPanel();
            jPanel2.setLayout(new GridBagLayout());
            jPanel2.add(getBtnStart(), gridBagConstraints1);
            jPanel2.add(getBtnCancel(), gridBagConstraints2);
        }
        return jPanel2;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
