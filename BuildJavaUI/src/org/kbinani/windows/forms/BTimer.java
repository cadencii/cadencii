package org.kbinani.windows.forms;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import javax.swing.Timer;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BTimer extends Timer implements ActionListener {
    private static final long serialVersionUID = 9174919033610117641L;
    public BEvent<BEventHandler> tickEvent = new BEvent<BEventHandler>();
    
    public BTimer(){
        super( 100, null );
        addActionListener( this );
    }
    
    public void actionPerformed( ActionEvent e ){
        try{
            tickEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BTimer#actionPerformed; ex=" + ex );
        }
    }
}
