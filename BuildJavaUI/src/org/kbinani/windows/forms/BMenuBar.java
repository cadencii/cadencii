package org.kbinani.windows.forms;

import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import javax.swing.JMenuBar;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BMenuBar extends JMenuBar
                      implements MouseListener
{
    private static final long serialVersionUID = 3226363259935125445L;

    public BMenuBar(){
        addMouseListener( this );
    }
    
    // root impl of Mouse* event is in BButton
    public BEvent<BMouseEventHandler> mouseClickEvent = new BEvent<BMouseEventHandler>();
    public BEvent<BMouseEventHandler> mouseDoubleClickEvent = new BEvent<BMouseEventHandler>();
    public BEvent<BMouseEventHandler> mouseDownEvent = new BEvent<BMouseEventHandler>();
    public BEvent<BMouseEventHandler> mouseUpEvent = new BEvent<BMouseEventHandler>();
    public BEvent<BEventHandler> mouseEnterEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> mouseLeaveEvent = new BEvent<BEventHandler>();
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
