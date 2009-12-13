import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BMenu;
import org.kbinani.windows.forms.BSplitPane;
import java.awt.Dimension;
import javax.swing.JPanel;
import java.awt.GridBagLayout;
import javax.swing.JButton;
import java.awt.GridBagConstraints;
import javax.swing.JCheckBox;
import javax.swing.JSplitPane;
import javax.swing.JToolBar;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;


public class Test extends BForm {
    private JMenuBar jJMenuBar = null;
    private BMenu jMenu = null;
    private JMenuItem jMenuItem = null;
    private JMenu jMenu1 = null;

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
        this.setJMenuBar(getJJMenuBar());
    }

    /**
     * This method initializes jJMenuBar	
     * 	
     * @return javax.swing.JMenuBar	
     */
    private JMenuBar getJJMenuBar() {
        if (jJMenuBar == null) {
            jJMenuBar = new JMenuBar();
            jJMenuBar.add(getJMenu());
            jJMenuBar.add(getJMenu1());
        }
        return jJMenuBar;
    }

    /**
     * This method initializes jMenu	
     * 	
     * @return javax.swing.JMenu	
     */
    private BMenu getJMenu() {
        if (jMenu == null) {
            jMenu = new BMenu();
            jMenu.setText("File");
            jMenu.add(getJMenuItem());
        }
        return jMenu;
    }

    /**
     * This method initializes jMenuItem	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenuItem getJMenuItem() {
        if (jMenuItem == null) {
            jMenuItem = new JMenuItem();
            jMenuItem.setText("Open");
        }
        return jMenuItem;
    }

    /**
     * This method initializes jMenu1	
     * 	
     * @return javax.swing.JMenu	
     */
    private JMenu getJMenu1() {
        if (jMenu1 == null) {
            jMenu1 = new JMenu();
            jMenu1.setText("Edit");
        }
        return jMenu1;
    }

}  //  @jve:decl-index=0:visual-constraint="10,10"
