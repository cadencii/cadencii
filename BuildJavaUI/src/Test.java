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
import org.kbinani.windows.forms.BListView_DRAFT;
import javax.swing.JButton;
import java.awt.GridBagConstraints;
import javax.swing.JPanel;
import java.awt.GridBagLayout;

public class Test extends JFrame {

    private JMenuBar jJMenuBar = null;
    private JMenu jMenu = null;
    private JMenuItem jMenuItem = null;
    private BListView_DRAFT jTable = null;
    private JButton jButton = null;
    private JButton jButton1 = null;
    private JPanel jPanel = null;

    /**
     * This method initializes 
     * 
     */
    public Test() {
    	super();
    	initialize();
    	jTable.setMultiSelect( false );
        //jTable.setCheckBoxes( true );
    	jTable.addItem( "", new BListViewItem( new String[] { "foo1", "bar1" } ) );
    	jTable.addItem( "mikan", new BListViewItem( new String[] { "foo2", "bar2", "baz2" } ) );
        jTable.setColumnHeaders( new String[]{ " ", " " } );
    }

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(379, 321));
        this.setContentPane(getJPanel());
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
    private BListView_DRAFT getJTable() {
        if (jTable == null) {
            jTable = new BListView_DRAFT();
        }
        return jTable;
    }

    /**
     * This method initializes jButton	
     * 	
     * @return javax.swing.JButton	
     */
    private JButton getJButton() {
        if (jButton == null) {
            jButton = new JButton();
            jButton.setText("PRINT");
            jButton.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    int rowCount = getJTable().getItemCount();
                    for( int i = 0; i < rowCount; i++ ){
                        //System.out.println( i + "\t" + getJTable().getItemAt( i ).isSelected() );
                    }
                }
            });
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
            jButton1.setText("ADD");
            jButton1.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    int row = getJTable().getItemCount();
                    BListViewItem copy = (BListViewItem)getJTable().getItemAt( row - 1 ).clone();
                    getJTable().addItem( "", copy );
                }
            });
        }
        return jButton1;
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.gridx = 1;
            gridBagConstraints1.weightx = 1.0D;
            gridBagConstraints1.gridy = 1;
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.gridx = 0;
            gridBagConstraints.weightx = 1.0D;
            gridBagConstraints.gridy = 1;
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 0;
            gridBagConstraints2.fill = GridBagConstraints.BOTH;
            gridBagConstraints2.gridwidth = 2;
            gridBagConstraints2.weightx = 1.0D;
            gridBagConstraints2.weighty = 1.0D;
            gridBagConstraints2.gridy = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.add(getJTable(), gridBagConstraints2);
            jPanel.add(getJButton(), gridBagConstraints);
            jPanel.add(getJButton1(), gridBagConstraints1);
        }
        return jPanel;
    }

}  //  @jve:decl-index=0:visual-constraint="10,10"
