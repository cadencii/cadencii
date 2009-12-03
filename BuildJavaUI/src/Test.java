import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BSplitPane;
import java.awt.Dimension;
import javax.swing.JPanel;
import java.awt.GridBagLayout;
import javax.swing.JButton;
import java.awt.GridBagConstraints;
import javax.swing.JCheckBox;
import javax.swing.JSplitPane;


public class Test extends BForm {

    private BSplitPane jSplitPane = null;
    private JButton jButton = null;
    private JButton jButton1 = null;

    /**
     * This method initializes 
     * 
     */
    public Test() {
    	super();
    	initialize();
    	jSplitPane.setPanel1MinSize( 100 );
    }
     
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(315, 240));
        this.setContentPane(getJSplitPane());
    }

    /**
     * This method initializes jSplitPane	
     * 	
     * @return javax.swing.JSplitPane	
     */
    private BSplitPane getJSplitPane() {
        if (jSplitPane == null) {
            jSplitPane = new BSplitPane();
            jSplitPane.setLeftComponent(getJButton());
            jSplitPane.setRightComponent(getJButton1());
        }
        return jSplitPane;
    }

    /**
     * This method initializes jButton	
     * 	
     * @return javax.swing.JButton	
     */
    private JButton getJButton() {
        if (jButton == null) {
            jButton = new JButton();
        }
        return jButton;
    }

    /**
     * This method initializes jButton1	
     * 	
     * @return javax.swing.JButton	
     */
    private JButton getJButton1() {
        if (jButton1 == null) {
            jButton1 = new JButton();
        }
        return jButton1;
    }

}  //  @jve:decl-index=0:visual-constraint="10,10"
