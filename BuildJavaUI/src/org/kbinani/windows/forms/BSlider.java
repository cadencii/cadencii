package org.kbinani.windows.forms;

import javax.swing.JSlider;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BSlider extends JSlider implements ChangeListener{
    public BEvent<BEventHandler> valueChangedEvent = new BEvent<BEventHandler>();
    
    public void stateChanged( ChangeEvent e ){
        try{
            valueChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BSlider#stateChanged; ex=" + ex );
        }
    }
}
