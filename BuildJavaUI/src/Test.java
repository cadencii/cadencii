import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BForm;
import java.awt.Dimension;
import javax.swing.JPanel;
import java.awt.GridBagLayout;
import javax.swing.JButton;
import java.awt.GridBagConstraints;
import javax.swing.JCheckBox;


public class Test extends BForm {

    private JPanel jPanel = null;
    private BCheckBox chkFoo = null;
    /**
     * This method initializes 
     * 
     */
    public Test() {
    	super();
    	initialize();
    	registerEventHandlers();
    }

    public void registerEventHandlers(){
        chkFoo.checkedChangedEvent.add( new BEventHandler( this, "chkFoo_CheckedChanged" ) );
    }
     
    public void chkFoo_CheckedChanged( Object sender, BEventArgs e ){
        System.out.println( "Test#chkFoo_CheckedChanged" );
        if( sender instanceof BCheckBox ){
            BCheckBox b = (BCheckBox)sender;
            System.out.println( "Test#chkFoo_CheckedChanged; isSelected=" + b.isSelected() );
        }
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
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.gridy = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getChkFoo(), gridBagConstraints1);
        }
        return jPanel;
    }

    /**
     * This method initializes chkFoo	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox
     */
    private BCheckBox getChkFoo() {
        if (chkFoo == null) {
            chkFoo = new BCheckBox();
        }
        return chkFoo;
    }

}  //  @jve:decl-index=0:visual-constraint="10,10"
