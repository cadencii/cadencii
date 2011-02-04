package org.kbinani.windows.forms;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JLabel;
import javax.swing.JPanel;
//SECTION-END-IMPORT

public class InputBox extends BDialog
{
    //SECTION-BEGIN-FIELD

    private JPanel jPanel = null;
    private BLabel lblMessage = null;
    private BTextBox txtInput = null;
    private BButton btnOk = null;
    private JPanel jPanel1 = null;
    private BButton btnCancel = null;

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public InputBox() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(320, 132));
        this.setContentPane(getJPanel());
    	setCancelButton( btnCancel );
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
            gridBagConstraints4.gridx = 0;
            gridBagConstraints4.anchor = GridBagConstraints.EAST;
            gridBagConstraints4.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints4.weighty = 1.0D;
            gridBagConstraints4.gridy = 2;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1.gridy = 1;
            gridBagConstraints1.weightx = 1.0D;
            gridBagConstraints1.insets = new Insets(3, 12, 6, 12);
            gridBagConstraints1.gridx = 0;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.gridwidth = 1;
            gridBagConstraints.weightx = 1.0D;
            gridBagConstraints.anchor = GridBagConstraints.WEST;
            gridBagConstraints.insets = new Insets(12, 12, 3, 12);
            gridBagConstraints.gridy = 0;
            lblMessage = new BLabel();
            lblMessage.setText("JLabel");
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(lblMessage, gridBagConstraints);
            jPanel.add(getTxtInput(), gridBagConstraints1);
            jPanel.add(getJPanel1(), gridBagConstraints4);
        }
        return jPanel;
    }

    /**
     * This method initializes txtInput	
     * 	
     * @return javax.swing.JTextField	
     */
    private BTextBox getTxtInput() {
        if (txtInput == null) {
            txtInput = new BTextBox();
        }
        return txtInput;
    }

    /**
     * This method initializes btnOk	
     * 	
     * @return javax.swing.JButton	
     */
    private BButton getBtnOk() {
        if (btnOk == null) {
            btnOk = new BButton();
            btnOk.setText("OK");
            btnOk.setPreferredSize(new Dimension(100, 29));
        }
        return btnOk;
    }

    /**
     * This method initializes jPanel1	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.gridy = 0;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 1;
            gridBagConstraints2.insets = new Insets(0, 0, 0, 12);
            gridBagConstraints2.gridy = 0;
            jPanel1 = new JPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.add(getBtnOk(), gridBagConstraints2);
            jPanel1.add(getBtnCancel(), gridBagConstraints3);
        }
        return jPanel1;
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
            btnCancel.setPreferredSize(new Dimension(100, 29));
        }
        return btnCancel;
    }
    
    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,2"

class _InputBox extends BDialog {
    //private BLabel lblMessage;
    //private BButton btnCancel;
    //private BTextBox txtInput;
    //private BButton btnOk;

    private JLabel jLabel = null;
    /**
     * This method initializes 
     * 
     */
    public _InputBox() {
    	super();
    	initialize();
    }
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
       jLabel = new JLabel();
        jLabel.setText("JLabel");
        this.setSize(new Dimension(280, 124));
        this.setContentPane(jLabel);
    		
    }

/*    private void initializeComponent(){
        txtInput = new BTextBox();
        btnOk = new BButton();
        lblMessage = new BLabel();
        btnCancel = new BButton();
        // 
        // txtInput
        // 
        // 
        // btnOk
        // 
        this.btnOk.setText( "OK" );
        this.btnOk.clickEvent.add( new BEventHandler( this, "btnOk_Click" ) );
        // 
        // lblMessage
        // 
        // 
        // btnCancel
        // 
        this.btnCancel.setText( "Cancel" );
        //this.btnCancel.setVisible( false );
        // 
        // InputBox
        // 
        GridBagLayout gridbag = new GridBagLayout();
        GridBagConstraints c = new GridBagConstraints();
        setLayout( gridbag );
        // 1段目
        JPanel jp1_1 = new JPanel();
        gridbag.setConstraints( jp1_1, c );
        add( jp1_1 );

        c.gridwidth = 2;
        c.fill = GridBagConstraints.HORIZONTAL;
        gridbag.setConstraints( lblMessage, c );
        add( lblMessage );
        
        JPanel jp1_2 = new JPanel();
        c.gridwidth = GridBagConstraints.REMAINDER;
        c.fill = GridBagConstraints.NONE;
        gridbag.setConstraints( jp1_2, c );
        add( jp1_2 );

        // 2段目
        JPanel jp2_1 = new JPanel();
        c.gridwidth = 1;
        gridbag.setConstraints( jp2_1, c );
        add( jp2_1 );
        
        c.gridwidth = 2;
        c.fill = GridBagConstraints.HORIZONTAL;
        c.weightx = 1.0;
        gridbag.setConstraints( txtInput, c );
        add( txtInput );
        
        JPanel jp2_2 = new JPanel();
        c.gridwidth = GridBagConstraints.REMAINDER;
        c.fill = GridBagConstraints.NONE;
        c.weightx = 0.0;
        gridbag.setConstraints( jp2_2, c );
        add( jp2_2 );

        // 3段目
        JPanel jp3 = new JPanel();
        c.gridwidth = GridBagConstraints.REMAINDER;
        gridbag.setConstraints( jp3, c );
        add( jp3 );

        // 4段目
        JPanel jp4_1 = new JPanel();
        c.gridwidth = 2;
        gridbag.setConstraints( jp4_1, c );
        add( jp4_1 );
        
        c.gridwidth = 1;
        c.anchor = GridBagConstraints.EAST;
        gridbag.setConstraints( btnOk, c );
        add( btnOk );
        
        JPanel jp4_2 = new JPanel();
        c.gridwidth = GridBagConstraints.REMAINDER;
        c.anchor = GridBagConstraints.CENTER;
        gridbag.setConstraints( jp4_2, c );
        add( jp4_2 );

        // 5段目
        JPanel jp5 = new JPanel();
        c.gridwidth = GridBagConstraints.REMAINDER;
        c.gridheight = GridBagConstraints.REMAINDER;
        c.fill = GridBagConstraints.BOTH;
        gridbag.setConstraints( jp5, c );
        add( jp5 );

        this.setTitle( "InputBox" );
        this.setSize( 339, 110 );
    }*/
}  //  @jve:decl-index=0:visual-constraint="10,10"

