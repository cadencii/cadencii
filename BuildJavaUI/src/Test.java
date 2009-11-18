import javax.swing.JFrame;
import java.awt.Dimension;
import java.awt.event.InputEvent;
import java.awt.event.KeyEvent;
import javax.swing.JDialog;
import javax.swing.JMenuBar;
import javax.swing.JMenu;
import javax.swing.JMenuItem;
import javax.swing.KeyStroke;
import javax.swing.JTable;
import org.kbinani.windows.forms.BListView;
import org.kbinani.windows.forms.BListViewItem;

public class Test extends JFrame {

    private JMenuBar jJMenuBar = null;
    private JMenu jMenu = null;
    private JMenuItem jMenuItem = null;
    private BListView jTable = null;

    /**
     * This method initializes 
     * 
     */
    public Test() {
    	super();
    	initialize();
    	jTable.setMultiSelect( false );
    	jTable.addItem( new BListViewItem( new String[] { "foo", "bar" } ) );
    	jTable.addItem( new BListViewItem( new String[] { "foo1", "bar1", "baz" } ) );
        jTable.setColumnHeaders( new String[]{ "HEADER1", "HEADER2" } );
    }

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(356, 271));
        this.setContentPane(getJTable());
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
        }
        return jJMenuBar;
    }

    /**
     * This method initializes jMenu	
     * 	
     * @return javax.swing.JMenu	
     */
    private JMenu getJMenu() {
        if (jMenu == null) {
            jMenu = new JMenu();
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
            jMenuItem.setAccelerator( KeyStroke.getKeyStroke( KeyEvent.VK_A, InputEvent.CTRL_MASK ) );
        }
        return jMenuItem;
    }

    /**
     * This method initializes jTable	
     * 	
     * @return org.kbinani.windows.forms.BListView
     */
    private BListView getJTable() {
        if (jTable == null) {
            jTable = new BListView();
        }
        return jTable;
    }

}  //  @jve:decl-index=0:visual-constraint="10,10"
