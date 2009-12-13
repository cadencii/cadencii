package org.kbinani.windows.forms;

import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import javax.swing.JTextField;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BTextBox extends JTextField 
                      implements KeyListener, 
                                 DocumentListener
{
    private static final long serialVersionUID = 7503633539526888136L;

    public BTextBox(){
        super();
        addKeyListener( this );
        getDocument().addDocumentListener( this );
    }
    
    /* root impl of TextChanged event */
    // root impl of TextChanged event is in BTextBox
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
    
    // root imol of KeyListener is in BButton
    public BEvent<BPreviewKeyDownEventHandler> previewKeyDownEvent = new BEvent<BPreviewKeyDownEventHandler>();
    public BEvent<BKeyEventHandler> keyDownEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyEventHandler> keyUpEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyPressEventHandler> keyPressEvent = new BEvent<BKeyPressEventHandler>();
    public void keyPressed( KeyEvent e ) {
        try{
            previewKeyDownEvent.raise( this, new BPreviewKeyDownEventArgs( e ) );
            keyDownEvent.raise( this, new BKeyEventArgs( e ) );
        }catch( Exception ex ){
            System.err.println( "BButton#keyPressed; ex=" + ex );
        }
    }
    public void keyReleased(KeyEvent e) {
        try{
            keyUpEvent.raise( this, new BKeyEventArgs( e ) );
        }catch( Exception ex ){
            System.err.println( "BButton#keyReleased; ex=" + ex );
        }
    }
    public void keyTyped(KeyEvent e) {
        try{
            previewKeyDownEvent.raise( this, new BPreviewKeyDownEventArgs( e ) );
            keyPressEvent.raise( this, new BKeyPressEventArgs( e ) );
        }catch( Exception ex ){
            System.err.println( "BButton#keyType; ex=" + ex );
        }
    }

}
