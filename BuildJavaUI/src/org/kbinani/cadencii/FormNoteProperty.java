package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Component;
import java.awt.Dimension;
import java.awt.GridBagLayout;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BMenu;
import org.kbinani.windows.forms.BMenuBar;
import org.kbinani.windows.forms.BMenuItem;
import javax.swing.JButton;
import java.awt.GridBagConstraints;

//SECTION-END-IMPORT
public class FormNoteProperty extends BDialog {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private JPanel panelMain = null;
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
    public void addComponent( Component c ){
        GridBagConstraints gridBagConstraints = new GridBagConstraints();
        gridBagConstraints.gridx = 0;
        gridBagConstraints.fill = GridBagConstraints.BOTH;
        gridBagConstraints.weightx = 1.0D;
        gridBagConstraints.weighty = 1.0D;
        gridBagConstraints.anchor = GridBagConstraints.NORTH;
        gridBagConstraints.gridy = 0;
        getPanelMain().add( c, gridBagConstraints);
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
    		
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getPanelMain() {
        if (panelMain == null) {
            panelMain = new JPanel();
            panelMain.setLayout(new GridBagLayout());
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
            menuStrip.setVisible(false);
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
