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
import javax.swing.JScrollPane;
import javax.swing.BorderFactory;

//SECTION-END-IMPORT
public class FormNoteProperty extends BDialog {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private JPanel panelMain = null;
    private BMenuBar menuStrip = null;
    private BMenu menuWindow = null;
    private BMenuItem menuClose = null;
    private JScrollPane jScrollPane = null;
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
            jScrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
        }
        return jScrollPane;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
