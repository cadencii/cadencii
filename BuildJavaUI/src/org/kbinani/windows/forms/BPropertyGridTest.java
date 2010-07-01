package org.kbinani.windows.forms;

import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.Frame;
import java.awt.GridBagLayout;
import java.awt.Point;
import java.awt.Rectangle;

public class BPropertyGridTest extends Frame {

    private BPropertyGrid jPanel = null;
   
    /**
     * This method initializes 
     * 
     */
    public BPropertyGridTest() {
    	super();
    	initialize();
    	jPanel.setSelectedObjects( new Point[]{ new Point( 10, 20 ), new Point( 10, 30 ) } );
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

class Foo{
    public boolean A = true;
    public String B = "text";
    public int C = 10;
    public Point D = new Point( 1, 2 );
    public Rectangle E = new Rectangle( 1, 2, 3, 4 );
    public FooEnum F = FooEnum.VALUE1;
}
