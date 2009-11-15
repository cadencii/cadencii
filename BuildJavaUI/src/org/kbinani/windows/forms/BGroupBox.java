package org.kbinani.windows.forms;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import javax.swing.BorderFactory;
import javax.swing.JPanel;
import javax.swing.border.TitledBorder;

public class BGroupBox extends JPanel {

	private static final long serialVersionUID = 1L;
	private TitledBorder titledBorder = null;  //  @jve:decl-index=0:

    public BGroupBox(){
        super();
        initialize();
    }

    /**
	 * This method initializes this
	 * 
	 */
	private void initialize() {
        this.setSize(new Dimension(352, 268));
        this.setTitle("");
        setBorder( getTitledBorder() );
	}

	public TitledBorder getTitledBorder(){
    	if( titledBorder == null ){
            titledBorder = BorderFactory.createTitledBorder( null, 
                    "",
                     TitledBorder.DEFAULT_JUSTIFICATION,
                     TitledBorder.DEFAULT_POSITION,
                     new Font( "Dialog", Font.BOLD, 12 ),
                     new Color( 51, 51, 51 ) );
    	}
    	return titledBorder;
    }
    
    public String getTitle(){
        return getTitledBorder().getTitle();
    }
    
    public void setTitle( String value ){
        getTitledBorder().setTitle( value );
    }
}
