package com.github.cadencii.ui;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagLayout;
import javax.swing.JPanel;
import com.github.cadencii.windows.forms.BDialog;
import com.github.cadencii.windows.forms.BMenu;
import com.github.cadencii.windows.forms.BMenuBar;
import com.github.cadencii.windows.forms.BMenuItem;

//SECTION-END-IMPORT
public class FormIconPalette extends BDialog
{
//SECTION-BEGIN-FIELD
    private static final long serialVersionUID = 1L;
    private BMenuBar myMenuBar = null;
    private BMenu menuWindow = null;
    private BMenuItem menuWindowHide = null;
    private JPanel jPanel = null;

    //SECTION-END-FIELD
    public FormIconPalette() {
    	super();
    	initialize();
    }
    //SECTION-BEGIN-METHOD

    private void initialize() {
        this.setSize(new Dimension(275, 178));
        this.setContentPane(getJPanel());
        this.setJMenuBar(getMyMenuBar());
    		
    }

    /**
     * This method initializes menuBar	
     * 	
     * @return org.kbinani.windows.forms.BMenuBar	
     */
    private BMenuBar getMyMenuBar() {
        if (myMenuBar == null) {
            myMenuBar = new BMenuBar();
            myMenuBar.add(getMenuWindow());
        }
        return myMenuBar;
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
            menuWindow.add(getMenuWindowHide());
        }
        return menuWindow;
    }

    /**
     * This method initializes menuWindowHide	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuWindowHide() {
        if (menuWindowHide == null) {
            menuWindowHide = new BMenuItem();
            menuWindowHide.setText("Hide");
        }
        return menuWindowHide;
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel() {
        if (jPanel == null) {
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
        }
        return jPanel;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
