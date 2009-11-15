package org.kbinani.windows.forms;

import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import javax.swing.JTextField;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BTextBox extends JTextField implements KeyListener, DocumentListener{
    public BTextBox(){
        super();
        addKeyListener( this );
        getDocument().addDocumentListener( this );
    }
    
    public BEvent<BEventHandler> textChangedEvent = new BEvent<BEventHandler>();
    
    public void changedUpdate( DocumentEvent e ){
        updates( e );
    }
    
    public void insertUpdate( DocumentEvent e ){
        updates( e );
    }
    
    public void removeUpdate( DocumentEvent e ){
        updates( e );
    }
     
    public void updates( DocumentEvent e ){
        try{
            textChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BTextBox#updates; ex=" + ex );
        }
    }
    
    public boolean isImeModeOn(){
        return getInputContext().isCompositionEnabled();
    }
    
    public void setImeModeOn( boolean value ){
        getInputContext().setCompositionEnabled( value );
    }
    
    /* root implementation of bocoree.windows.forms.[component] */
    /* REGION bocoree.windows.forms.[component] */
    /* root implementation of bocoree.windows.forms.[component] instanceof in BTextBox.cs */
    private Object m_tag = null;
    
    public Object getTag(){
        return m_tag;
    }
    
    public void setTag( Object value ){
        m_tag = value;
    }
    /* END REGION */
    
    /* REGION java.awt.Component */
    /* root implementation of java.awt.Component instanceof in BForm.cs(java) */
    public BEvent<BKeyEventHandler> keyUpEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyEventHandler> keyDownEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyEventHandler> keyPressedEvent = new BEvent<BKeyEventHandler>();
    
    public void keyPressed( KeyEvent e0 ){
        try{
            BKeyEventArgs e = new BKeyEventArgs( e0 );
            keyDownEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyPressed; ex=" + ex );
        }
    }
    
    public void keyReleased( KeyEvent e0 ){
        try{
            BKeyEventArgs e = new BKeyEventArgs( e0 );
            keyUpEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyReleased; ex=" + ex );
        }
    }
    
    public void keyTyped( KeyEvent e0 ){
        try{
            BKeyEventArgs e = new BKeyEventArgs( e0 );
            keyPressedEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyTyped; ex=" + ex );
        }
    }
}
