import java.awt.Dimension;
import java.awt.GridBagLayout;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BForm;
import javax.swing.JComboBox;
import java.awt.GridBagConstraints;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import javax.swing.JTextField;


public class Test extends BForm {
    /**
     * 
     */
    private static final long serialVersionUID = 1L;
    private JPanel jPanel = null;
    private JTextField jTextField = null;
    /**
     * This method initializes jTextField	
     * 	
     * @return javax.swing.JTextField	
     */
    private JTextField getJTextField() {
        if (jTextField == null) {
            jTextField = new JTextField();
            jTextField.addActionListener( new ActionListener(){

                @Override
                public void actionPerformed(ActionEvent arg0) {
                    System.out.println( "actionPerformed" );
                    // TODO Auto-generated method stub
                    
                }
                
            });
        }
        return jTextField;
    }

    public static void main( String[] args )
    {
        Test t = new Test();
        t.setVisible( true );
    }
    
    /**
     * This method initializes 
     * 
     */
    public Test() {
    	super();
    	initialize();
    }
     
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(315, 240));
        this.setContentPane(getJPanel());
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints.gridy = 0;
            gridBagConstraints.weightx = 1.0;
            gridBagConstraints.gridx = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getJTextField(), gridBagConstraints);
        }
        return jPanel;
    }

}  //  @jve:decl-index=0:visual-constraint="63,28"
