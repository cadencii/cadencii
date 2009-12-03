package org.kbinani.Cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JLabel;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BNumericUpDown;

//SECTION-END-IMPORT
public class FormInsertBar extends BForm {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private JPanel jContentPane = null;
    private BLabel lblPosition = null;
    private BNumericUpDown numPosition = null;
    private BLabel lblThBar = null;
    private BLabel lblLength = null;
    private BNumericUpDown numLength = null;
    private BLabel lblBar = null;
    private JPanel jPanel = null;
    private BButton btnOK = null;
    private BButton btnCancel = null;
    private JLabel lblPositionPrefix = null;
    
    //SECTION-END-FIELD
    /**
     * This is the default constructor
     */
    public FormInsertBar() {
        super();
        initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     * 
     * @return void
     */
    private void initialize() {
        this.setSize(249, 158);
        this.setContentPane(getJContentPane());
        this.setTitle("Insert bar");
    }

    /**
     * This method initializes jContentPane
     * 
     * @return javax.swing.JPanel
     */
    private JPanel getJContentPane() {
        if (jContentPane == null) {
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.gridx = 1;
            gridBagConstraints11.insets = new Insets(8, 0, 0, 0);
            gridBagConstraints11.gridy = 0;
            lblPositionPrefix = new JLabel();
            lblPositionPrefix.setText(" ");
            GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
            gridBagConstraints14.gridx = 0;
            gridBagConstraints14.anchor = GridBagConstraints.EAST;
            gridBagConstraints14.gridwidth = 4;
            gridBagConstraints14.weightx = 1.0D;
            gridBagConstraints14.insets = new Insets(16, 0, 8, 0);
            gridBagConstraints14.gridy = 2;
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.gridx = 3;
            gridBagConstraints13.insets = new Insets(4, 8, 0, 16);
            gridBagConstraints13.anchor = GridBagConstraints.WEST;
            gridBagConstraints13.gridy = 1;
            lblBar = new BLabel();
            lblBar.setText("bar");
            GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
            gridBagConstraints12.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints12.gridy = 1;
            gridBagConstraints12.weightx = 1.0;
            gridBagConstraints12.insets = new Insets(4, 0, 0, 0);
            gridBagConstraints12.gridx = 2;
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.anchor = GridBagConstraints.WEST;
            gridBagConstraints3.insets = new Insets(4, 16, 0, 8);
            gridBagConstraints3.gridwidth = 2;
            gridBagConstraints3.gridy = 1;
            lblLength = new BLabel();
            lblLength.setText("Length");
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 3;
            gridBagConstraints2.insets = new Insets(8, 8, 0, 16);
            gridBagConstraints2.anchor = GridBagConstraints.WEST;
            gridBagConstraints2.gridy = 0;
            lblThBar = new BLabel();
            lblThBar.setText("th bar");
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weightx = 1.0;
            gridBagConstraints1.insets = new Insets(8, 0, 0, 0);
            gridBagConstraints1.gridx = 2;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.anchor = GridBagConstraints.EAST;
            gridBagConstraints.insets = new Insets(8, 16, 0, 8);
            gridBagConstraints.gridy = 0;
            lblPosition = new BLabel();
            lblPosition.setText("Position");
            jContentPane = new JPanel();
            jContentPane.setLayout(new GridBagLayout());
            jContentPane.add(lblPosition, gridBagConstraints);
            jContentPane.add(getNumPosition(), gridBagConstraints1);
            jContentPane.add(lblThBar, gridBagConstraints2);
            jContentPane.add(lblLength, gridBagConstraints3);
            jContentPane.add(getNumLength(), gridBagConstraints12);
            jContentPane.add(lblBar, gridBagConstraints13);
            jContentPane.add(getJPanel(), gridBagConstraints14);
            jContentPane.add(lblPositionPrefix, gridBagConstraints11);
        }
        return jContentPane;
    }

    /**
     * This method initializes numPosition 
     *  
     * @return javax.swing.BTextBox 
     */
    private BNumericUpDown getNumPosition() {
        if (numPosition == null) {
            numPosition = new BNumericUpDown();
        }
        return numPosition;
    }

    /**
     * This method initializes numLength   
     *  
     * @return javax.swing.BTextBox 
     */
    private BNumericUpDown getNumLength() {
        if (numLength == null) {
            numLength = new BNumericUpDown();
        }
        return numLength;
    }

    /**
     * This method initializes jPanel   
     *  
     * @return javax.swing.JPanel   
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
            gridBagConstraints5.gridx = 1;
            gridBagConstraints5.anchor = GridBagConstraints.WEST;
            gridBagConstraints5.insets = new Insets(0, 0, 0, 16);
            gridBagConstraints5.gridy = 0;
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.anchor = GridBagConstraints.WEST;
            gridBagConstraints4.insets = new Insets(0, 0, 0, 16);
            gridBagConstraints4.gridy = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getBtnOK(), gridBagConstraints4);
            jPanel.add(getBtnCancel(), gridBagConstraints5);
        }
        return jPanel;
    }

    /**
     * This method initializes btnOK    
     *  
     * @return javax.swing.BButton  
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
     * @return javax.swing.BButton  
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
