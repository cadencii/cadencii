package com.github.cadencii.ui;

//SECTION-BEGIN-IMPORT
import java.awt.Component;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import javax.swing.BorderFactory;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import java.awt.event.WindowListener;
import java.awt.event.WindowEvent;
import com.github.cadencii.windows.forms.BDialog;
import com.github.cadencii.windows.forms.BMenu;
import com.github.cadencii.windows.forms.BMenuBar;
import com.github.cadencii.windows.forms.BMenuItem;

//SECTION-END-IMPORT
public class FormNoteProperty extends BDialog {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private JPanel panelMain = null;
    private BMenuBar menuStrip = null;
    private BMenu menuWindow = null;
    private BMenuItem menuClose = null;
    private JScrollPane jScrollPane = null;
    private boolean isMinimized = false;
    //SECTION-END-FIELD

    /**
     * This method initializes
     *
     */
    public FormNoteProperty() {
    	super();
    	initialize();
    }

    //SECTION-BEGIN-METHOD
    public void addComponent( Component c ){
        getJScrollPane().setViewportView( c );
    }

    /**
     * ウィンドウが最小化された状態かどうかを取得する
     * @return boolean
     */
    public boolean isMinimized()
    {
        return this.isMinimized;
    }

    /**
     * This method initializes this
     *
     */
    private void initialize() {
        this.setSize(new Dimension(217, 330));
        this.setJMenuBar(getMenuStrip());
        this.setContentPane(getPanelMain());
        this.setTitle("Note Property");
        this.addWindowListener( new WindowListener(){
            public void windowIconified( WindowEvent e )
            {
                {//TODO:
                    System.out.println( "iconified" );
                }
                isMinimized = true;
            }

            public void windowDeiconified( WindowEvent e )
            {
                {//TODO:
                    System.out.println( "deiconified" );
                }
                isMinimized = false;
            }

            public void windowDeactivated( WindowEvent e ){};
            public void windowActivated( WindowEvent e ){};
            public void windowClosed( WindowEvent e ){};
            public void windowClosing( WindowEvent e ){};
            public void windowOpened( WindowEvent e ){};
        } );
    }

    /**
     * This method initializes jPanel
     *
     * @return javax.swing.JPanel
     */
    private JPanel getPanelMain() {
        if (panelMain == null) {
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.BOTH;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weightx = 1.0;
            gridBagConstraints1.weighty = 1.0;
            gridBagConstraints1.gridx = 0;
            panelMain = new JPanel();
            panelMain.setLayout(new GridBagLayout());
            panelMain.add(getJScrollPane(), gridBagConstraints1);
        }
        return panelMain;
    }

    /**
     * This method initializes menuStrip
     *
     * @return javax.swing.JMenuBar
     */
    private BMenuBar getMenuStrip() {
        if (menuStrip == null) {
            menuStrip = new BMenuBar();
            menuStrip.setVisible(true);
            menuStrip.add(getMenuWindow());
        }
        return menuStrip;
    }

    /**
     * This method initializes menuWindow
     *
     * @return javax.swing.JMenu
     */
    private BMenu getMenuWindow() {
        if (menuWindow == null) {
            menuWindow = new BMenu();
            menuWindow.setText("Window");
            menuWindow.add(getMenuClose());
        }
        return menuWindow;
    }

    /**
     * This method initializes menuClose
     *
     * @return javax.swing.JMenuItem
     */
    private BMenuItem getMenuClose() {
        if (menuClose == null) {
            menuClose = new BMenuItem();
            menuClose.setText("Close");
        }
        return menuClose;
    }

    /**
     * This method initializes jScrollPane
     *
     * @return javax.swing.JScrollPane
     */
    private JScrollPane getJScrollPane() {
        if (jScrollPane == null) {
            jScrollPane = new JScrollPane();
            jScrollPane.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            jScrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
            jScrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
        }
        return jScrollPane;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
