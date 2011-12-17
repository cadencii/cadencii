package com.github.cadencii.windows.forms;

import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import javax.swing.JRadioButton;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BRadioButton extends JRadioButton
                          implements ItemListener
{

    private static final long serialVersionUID = 6869663294795814279L;

    public BRadioButton(){
        super();
        addItemListener( this );
    }
    
    // root impl of ItemListener is in BCheckBox
    public final BEvent<BEventHandler> checkedChangedEvent = new BEvent<BEventHandler>();
    public void itemStateChanged(ItemEvent e) {
        try{
            checkedChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BCheckBox#itemStateChanged; ex=" + ex );
        }
    }
}
