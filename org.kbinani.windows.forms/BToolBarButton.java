package com.github.cadencii.windows.forms;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.FocusEvent;
import java.awt.event.FocusListener;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import javax.swing.JToggleButton;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BToolBarButton extends JToggleButton
                            implements ActionListener,
                                       ItemListener,
                                       FocusListener
{
    private static final long serialVersionUID = -4646914775808502496L;
    private boolean mCheckOnClick = false;

    public BToolBarButton(){
        super();
        addActionListener( this );
        addItemListener( this );
        addFocusListener( this );
    }

    public boolean isCheckOnClick(){
        return mCheckOnClick;
    }
    
    public void setCheckOnClick( boolean value ){
        mCheckOnClick = value;
    }
    
    // root impl of FocusListener is in BButton
    public final BEvent<BEventHandler> enterEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> leaveEvent = new BEvent<BEventHandler>();
    public void focusGained(FocusEvent e) {
        try{
            enterEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BToolBarButton#focusGained; ex=" + ex );
        }
    }
    public void focusLost(FocusEvent e) {
        try{
            leaveEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BToolBarButton#focusLost; ex=" + ex );
        }
    }

    // root impl of ItemListener is in BCheckBox
    public final BEvent<BEventHandler> checkedChangedEvent = new BEvent<BEventHandler>();
    public void itemStateChanged(ItemEvent e) {
        if( mCheckOnClick ){
            try{
                checkedChangedEvent.raise( this, new BEventArgs() );
            }catch( Exception ex ){
                System.err.println( "BCheckBox#itemStateChanged; ex=" + ex );
            }
        }
    }
    
    // root impl of Click event is in BButton
    public final BEvent<BEventHandler> clickEvent = new BEvent<BEventHandler>();
    public void actionPerformed( ActionEvent e ){
        try{
            clickEvent.raise( this, new BEventArgs() );
            if( !mCheckOnClick ){
                super.setSelected( false );
            }
        }catch( Exception ex ){
            System.err.println( "BButton#actionPerformed; ex=" + ex );
        }
    }
    
}
