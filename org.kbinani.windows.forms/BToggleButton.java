package com.github.cadencii.windows.forms;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.FocusEvent;
import java.awt.event.FocusListener;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import javax.swing.JToggleButton;
import com.github.cadencii.BEvent;
import com.github.cadencii.BEventArgs;
import com.github.cadencii.BEventHandler;

public class BToggleButton extends JToggleButton
                           implements ActionListener,
                                      ItemListener,
                                      FocusListener
{
    private static final long serialVersionUID = 6912088646788737458L;

    public BToggleButton(){
        super();
        addActionListener( this );
        addItemListener( this );
        addFocusListener( this );
    }
    
    // root impl of FocusListener is in BButton
    public final BEvent<BEventHandler> enterEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> leaveEvent = new BEvent<BEventHandler>();
    public void focusGained(FocusEvent e) {
        try{
            enterEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BToggleButton#focusGained; ex=" + ex );
        }
    }
    public void focusLost(FocusEvent e) {
        try{
            leaveEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BToggleButton#focusLost; ex=" + ex );
        }
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
    
    // root impl of Click event is in BButton
    public final BEvent<BEventHandler> clickEvent = new BEvent<BEventHandler>();
    public void actionPerformed( ActionEvent e ){
        try{
            clickEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BButton#actionPerformed; ex=" + ex );
        }
    }
    
}
