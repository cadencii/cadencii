package org.kbinani.windows.forms;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import javax.swing.JButton;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BButton extends JButton implements ActionListener, MouseListener, MouseMotionListener{
    private static final long serialVersionUID = 1L;
    public BEvent<BEventHandler> clickEvent = new BEvent<BEventHandler>();
    public BEvent<BMouseEventHandler> mouseMoveEvent = new BEvent<BMouseEventHandler>();
    public BEvent<BMouseEventHandler> mouseDownEvent = new BEvent<BMouseEventHandler>();
    public BEvent<BMouseEventHandler> mouseUpEvent = new BEvent<BMouseEventHandler>();

    public BButton(){
        addActionListener( this );
    }
    
    public void actionPerformed( ActionEvent e ){
        try{
            clickEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BButton#actionPerformed; ex=" + ex );
        }
    }
    
    public void mouseClicked(MouseEvent e){
    }
    
    public void mouseEntered(MouseEvent e){
    }
    
    public void mouseExited(MouseEvent e){
    }
    
    public void mousePressed(MouseEvent e){
    	try{
    		mouseDownEvent.raise( this, BMouseEventArgs.fromMouseEvent( e ) );
    	}catch( Exception ex ){
    		System.err.println( "BButton#mousePressed; ex=" + ex );
    	}
    }
    
    public void mouseReleased(MouseEvent e){
    	try{
    		mouseUpEvent.raise( this, BMouseEventArgs.fromMouseEvent( e ) );
    	}catch( Exception ex ){
    		System.err.println( "BButton#mouseReleased; ex=" + ex );
    	}
    }
    
    public void mouseDragged(MouseEvent e){
        try{
            mouseMoveEvent.raise( this, BMouseEventArgs.fromMouseEvent( e ) );
        }catch( Exception ex ){
            System.err.println( "BButton#mouseDragged; ex=" + ex );
        }
    }
    
    public void mouseMoved(MouseEvent e){
    	try{
    		mouseMoveEvent.raise( this, BMouseEventArgs.fromMouseEvent( e ) );
    	}catch( Exception ex ){
    		System.err.println( "BButton#mouseMoved; ex=" + ex );
    	}
    }
}
