package org.kbinani.windows.forms;

import java.awt.event.AdjustmentEvent;
import java.awt.event.AdjustmentListener;
import javax.swing.JScrollBar;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BHScrollBar extends JScrollBar implements AdjustmentListener {
    private static final long serialVersionUID = 1L;
    public BEvent<BEventHandler> valueChangedEvent = new BEvent<BEventHandler>();

    public BHScrollBar(){
        super();
        setOrientation( JScrollBar.HORIZONTAL );
    }

    @Override
    public void adjustmentValueChanged(AdjustmentEvent e) {
        try{
            valueChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BHScrollBar#adjustmentValueChanged; ex=" + ex );
        }
    }
}
