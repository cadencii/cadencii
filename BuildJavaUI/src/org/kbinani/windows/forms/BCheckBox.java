package org.kbinani.windows.forms;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import javax.swing.JCheckBox;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BCheckBox extends JCheckBox implements ActionListener{
    private static final long serialVersionUID = 1L;
    public BEvent<BEventHandler> checkedChangedEvent = new BEvent<BEventHandler>();

    public BCheckBox(){
        super();
        addActionListener( this );
    }

    public void actionPerformed( ActionEvent e ){
        try{
            checkedChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BCheckBox#actionPerformed; ex=" + ex );
        }
    }
}
