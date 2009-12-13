package org.kbinani.windows.forms;

import java.awt.event.AdjustmentEvent;
import java.awt.event.AdjustmentListener;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.FocusListener;
import java.awt.event.FocusEvent;
import javax.swing.JScrollBar;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BScrollBar extends JScrollBar 
                        implements AdjustmentListener,
                                   ComponentListener,
                                   FocusListener
{
    private static final long serialVersionUID = 1L;
    public BEvent<BEventHandler> valueChangedEvent = new BEvent<BEventHandler>();

    public BScrollBar( int orientation ){
        super();
        addAdjustmentListener( this );
        addComponentListener( this );
        addFocusListener( this );
        setOrientation( orientation );
    }

    public void adjustmentValueChanged(AdjustmentEvent e) {
        try{
            valueChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BHScrollBar#adjustmentValueChanged; ex=" + ex );
        }
    }
    
    // root impl of FocusListener is in BButton
    public BEvent<BEventHandler> enterEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> leaveEvent = new BEvent<BEventHandler>();
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
    
    // root impl of ComponentListener is in BButton
    public BEvent<BEventHandler> visibleChangedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> resizeEvent = new BEvent<BEventHandler>();
    public void componentHidden(ComponentEvent e) {
        try{
            visibleChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BButton#componentHidden; ex=" + ex );
        }
    }
    public void componentMoved(ComponentEvent e) {
    }
    public void componentResized(ComponentEvent e) {
        try{
            resizeEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BButton#componentResized; ex=" + ex );
        }
    }
    public void componentShown(ComponentEvent e) {
        try{
            visibleChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BButton#componentShown; ex=" + ex );
        }
    }

}
