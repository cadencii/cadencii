package org.kbinani.windows.forms;

import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.Frame;
import java.awt.GridBagLayout;
import java.awt.Point;
import java.awt.Rectangle;
import javax.swing.JFrame;
import javax.swing.JMenuBar;
import javax.swing.JMenu;
import javax.swing.JMenuItem;

public class BPropertyGridTest extends JFrame {

    private BPropertyGrid jPanel = null;
    private JMenuBar jJMenuBar = null;
    private JMenu jMenu = null;
    private JMenu jMenuItem = null;
    private JMenuItem jMenuItem1 = null;
    private JMenuItem jMenuItem2 = null;
    private JMenuItem jMenuItem3 = null;
    private JMenu jMenu1 = null;
    private JMenuItem jMenuItem4 = null;
    private JMenuItem jMenuItem5 = null;
    private JMenuItem jMenuItem6 = null;
   
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
            jMenu.setText("Edit");
            jMenu.add(getJMenuItem());
            jMenu.add(getJMenu1());
        }
        return jMenu;
    }

    /**
     * This method initializes jMenuItem	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenu getJMenuItem() {
        if (jMenuItem == null) {
            jMenuItem = new JMenu();
            jMenuItem.setText("Change row height");
            jMenuItem.add(getJMenuItem1());
            jMenuItem.add(getJMenuItem2());
            jMenuItem.add(getJMenuItem3());
        }
        return jMenuItem;
    }

    /**
     * This method initializes jMenuItem1	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenuItem getJMenuItem1() {
        if (jMenuItem1 == null) {
            jMenuItem1 = new JMenuItem();
            jMenuItem1.setText("10");
            jMenuItem1.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                     jPanel.setRowHeight( 10 );
                }
            });
        }
        return jMenuItem1;
    }

    /**
     * This method initializes jMenuItem2	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenuItem getJMenuItem2() {
        if (jMenuItem2 == null) {
            jMenuItem2 = new JMenuItem();
            jMenuItem2.setText("15");
            jMenuItem2.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    jPanel.setRowHeight( 15 );
                }
            });
        }
        return jMenuItem2;
    }

    /**
     * This method initializes jMenuItem3	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenuItem getJMenuItem3() {
        if (jMenuItem3 == null) {
            jMenuItem3 = new JMenuItem();
            jMenuItem3.setText("20");
            jMenuItem3.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    jPanel.setRowHeight( 20 );
                }
            });
        }
        return jMenuItem3;
    }

    /**
     * This method initializes jMenu1	
     * 	
     * @return javax.swing.JMenu	
     */
    private JMenu getJMenu1() {
        if (jMenu1 == null) {
            jMenu1 = new JMenu();
            jMenu1.setText("Change column width");
            jMenu1.add(getJMenuItem4());
            jMenu1.add(getJMenuItem5());
            jMenu1.add(getJMenuItem6());
        }
        return jMenu1;
    }

    /**
     * This method initializes jMenuItem4	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenuItem getJMenuItem4() {
        if (jMenuItem4 == null) {
            jMenuItem4 = new JMenuItem();
            jMenuItem4.setText("50");
            jMenuItem4.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    jPanel.setColumnWidth( 50 );
                }
            });
        }
        return jMenuItem4;
    }

    /**
     * This method initializes jMenuItem5	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenuItem getJMenuItem5() {
        if (jMenuItem5 == null) {
            jMenuItem5 = new JMenuItem();
            jMenuItem5.setText("100");
            jMenuItem5.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    jPanel.setColumnWidth( 100 );
                }
            });
        }
        return jMenuItem5;
    }

    /**
     * This method initializes jMenuItem6	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JMenuItem getJMenuItem6() {
        if (jMenuItem6 == null) {
            jMenuItem6 = new JMenuItem();
            jMenuItem6.setText("150");
            jMenuItem6.addActionListener(new java.awt.event.ActionListener() {
                public void actionPerformed(java.awt.event.ActionEvent e) {
                    jPanel.setColumnWidth( 150 );
                }
            });
        }
        return jMenuItem6;
    }

    public static void main( String[] args ){
        BPropertyGridTest dialog = new BPropertyGridTest();
        dialog.setDefaultCloseOperation( EXIT_ON_CLOSE );
        dialog.setVisible( true );
    }
    
    /**
     * This method initializes 
     * 
     */
    public BPropertyGridTest() {
    	super();
    	initialize();
    	jPanel.setSelectedObjects( new BPropertyGridTestItem[]{ new BPropertyGridTestItem() } );
    }

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(252, 216));
        this.setJMenuBar(getJJMenuBar());
        this.add(getJPanel(), BorderLayout.CENTER);
    		
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private BPropertyGrid getJPanel() {
        if (jPanel == null) {
            jPanel = new BPropertyGrid();
            jPanel.setLayout(new GridBagLayout());
        }
        return jPanel;
    }

}  //  @jve:decl-index=0:visual-constraint="10,10"

enum FooEnum{
    VALUE1,
    VALUE2,
    NONE,
}
