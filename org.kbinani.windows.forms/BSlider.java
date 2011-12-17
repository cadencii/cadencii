package com.github.cadencii.windows.forms;

import java.awt.event.FocusEvent;
import java.awt.event.FocusListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import javax.swing.JSlider;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BSlider extends JSlider 
                     implements ChangeListener,
                                MouseListener,
                                FocusListener
{
    private static final long serialVersionUID = -2771998534716750091L;
    public final BEvent<BEventHandler> valueChangedEvent = new BEvent<BEventHandler>();
    
    public BSlider(){
        addChangeListener( this );
        addMouseListener( this );
        addFocusListener( this );
    }
    
    public void stateChanged( ChangeEvent e ){
        try{
            valueChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BSlider#stateChanged; ex=" + ex );
        }
    }

    // root impl of FocusListener is in BButton
    public final BEvent<BEventHandler> enterEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> leaveEvent = new BEvent<BEventHandler>();
    public void focusGained(FocusEvent e) {
        try{
            enterEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BButton#focusGained; ex=" + ex );
        }
    }
    public void focusLost(FocusEvent e) {
        try{
            leaveEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BButton#focusLost; ex=" + ex );
        }
    }

    // root impl of MouseListener is in BButton
    public final BEvent<BMouseEventHandler> mouseClickEvent = new BEvent<BMouseEventHandler>();
    public final BEvent<BMouseEventHandler> mouseDoubleClickEvent = new BEvent<BMouseEventHandler>();
    public final BEvent<BMouseEventHandler> mouseDownEvent = new BEvent<BMouseEventHandler>();
    public final BEvent<BMouseEventHandler> mouseUpEvent = new BEvent<BMouseEventHandler>();
    public final BEvent<BEventHandler> mouseEnterEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> mouseLeaveEvent = new BEvent<BEventHandler>();
    public void mouseClicked( MouseEvent e ){
        try{
            mouseClickEvent.raise( this, BMouseEventArgs.fromMouseEvent( e ) );
            if( e.getClickCount() >= 2 ){
                mouseDoubleClickEvent.raise( this, BMouseEventArgs.fromMouseEvent( e ) );
            }
        }catch( Exception ex ){
            System.err.println( "BButton#mouseClicked; ex=" + ex );
        }
    }    
    public void mouseEntered( MouseEvent e ){
        try{
            mouseEnterEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BButton#mouseEntered; ex=" + ex );
        }
    }    
    public void mouseExited( MouseEvent e ){
        try{
            mouseLeaveEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BButton#mouseExited; ex=" + ex );
        }
    }    
    public void mousePressed( MouseEvent e ){
        try{
            mouseDownEvent.raise( this, BMouseEventArgs.fromMouseEvent( e ) );
        }catch( Exception ex ){
            System.err.println( "BButton#mousePressed; ex=" + ex );
        }
    }    
    public void mouseReleased( MouseEvent e ){
        try{
            mouseUpEvent.raise( this, BMouseEventArgs.fromMouseEvent( e ) );
        }catch( Exception ex ){
            System.err.println( "BButton#mouseReleased; ex=" + ex );
        }
    }

}
