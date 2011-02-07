package org.kbinani.windows.forms;

import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.Frame;
import java.awt.GridBagLayout;
import java.awt.Point;
import java.awt.Rectangle;
import javax.swing.JFrame;

public class BPropertyGridTest extends JFrame {

    private BPropertyGrid jPanel = null;
   
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
