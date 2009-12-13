package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BMenu;
import org.kbinani.windows.forms.BMenuBar;
import org.kbinani.windows.forms.BMenuItem;
import java.awt.Dimension;
import javax.swing.JPanel;
import java.awt.GridBagLayout;
import javax.swing.JMenuBar;
import javax.swing.JMenu;
import javax.swing.JMenuItem;

//SECTION-END-IMPORT
public class FormNoteProperty extends BForm {
    //SECTION-BEGIN-FIELD

    private JPanel jPanel = null;
    private BMenuBar menuStrip = null;
    private BMenu menuWindow = null;
    private BMenuItem menuClose = null;

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
    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setSize(new Dimension(217, 330));
        this.setJMenuBar(getMenuStrip());
        this.setContentPane(getJPanel());
        this.setTitle("Note Property");
    		
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

    /**
     * This method initializes menuStrip	
     * 	
     * @return javax.swing.JMenuBar	
     */
    private BMenuBar getMenuStrip() {
        if (menuStrip == null) {
            menuStrip = new BMenuBar();
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

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
