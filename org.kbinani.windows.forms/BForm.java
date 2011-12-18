package com.github.cadencii.windows.forms;

import java.awt.Dimension;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import javax.swing.JFrame;
import javax.swing.UIManager;
import com.github.cadencii.BEvent;
import com.github.cadencii.BEventArgs;
import com.github.cadencii.BEventHandler;

public class BForm extends JFrame 
                   implements WindowListener, 
                              KeyListener, 
                              ComponentListener
{
    private static final long serialVersionUID = -3700177079249925623L;
    public final BEvent<BFormClosingEventHandler> formClosingEvent = new BEvent<BFormClosingEventHandler>();
    public final BEvent<BFormClosedEventHandler> formClosedEvent = new BEvent<BFormClosedEventHandler>();
    public final BEvent<BEventHandler> activatedEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> deactivateEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> loadEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> windowStateChangedEvent = new BEvent<BEventHandler>();
    
    public BForm(){
        this( "" );
    }
    
    public BForm( String title ){
        super( title );
        addWindowListener( this );
        addKeyListener( this );
        addComponentListener( this );
        setDefaultCloseOperation( DO_NOTHING_ON_CLOSE );
        try{
            UIManager.getInstalledLookAndFeels();
            UIManager.setLookAndFeel( UIManager.getSystemLookAndFeelClassName() );
        }catch( Exception e ){
        }
    }
        
    // root imol of KeyListener is in BButton
    public final BEvent<BPreviewKeyDownEventHandler> previewKeyDownEvent = new BEvent<BPreviewKeyDownEventHandler>();
    public final BEvent<BKeyEventHandler> keyDownEvent = new BEvent<BKeyEventHandler>();
    public final BEvent<BKeyEventHandler> keyUpEvent = new BEvent<BKeyEventHandler>();
    public final BEvent<BKeyPressEventHandler> keyPressEvent = new BEvent<BKeyPressEventHandler>();
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

    public Dimension getClientSize(){
        return getContentPane().getSize();
    }
    
    public void close(){
        try{
            boolean previous = isVisible();
            BFormClosingEventArgs e = new BFormClosingEventArgs();
            formClosingEvent.raise( this, e );
            if( e.Cancel ){
                setVisible( previous );
                return;
            }
        }catch( Exception ex ){
            System.err.println( "BForm#close; ex=" + ex );
        }
        setVisible( false );
        dispose();
    }
    
    public void windowActivated( WindowEvent e ){
        try{
            activatedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#windowActivated; ex=" + ex );
        }
    }
    
    public void windowClosed( WindowEvent e ){
        try{
            formClosedEvent.raise( this, new BFormClosedEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#windowClosed; ex=" + ex );
        }
    }
    
    public void windowClosing( WindowEvent e ){
        try{
            boolean previous = isVisible();
            BFormClosingEventArgs ev = new BFormClosingEventArgs();
            formClosingEvent.raise( this, ev );
            if( ev.Cancel ){
                setVisible( previous );
                return;
            }
        }catch( Exception ex ){
            System.err.println( "BForm#windowClosing; ex=" + ex );
        }
        setVisible( false );
    }
    
    public void windowDeactivated( WindowEvent e ){
        try{
            deactivateEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#windowDeactivated; ex=" + ex );
        }
    }
    
    public void windowDeiconified( WindowEvent e ){
        try{
            windowStateChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#windowDeiconified; ex=" + ex );
        }
    }
    
    public void windowIconified( WindowEvent e ){
        try{
            windowStateChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#windowIconified; ex=" + ex );
        }
    }
    
    public void windowOpened( WindowEvent e ){
        try{
            loadEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#windowOpened; ex=" + ex );
        }
    }

    // root impl of ComponentListener is in BButton
    public final BEvent<BEventHandler> visibleChangedEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> resizeEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> sizeChangedEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> locationChangedEvent = new BEvent<BEventHandler>();
    public void componentHidden(ComponentEvent e) {
        try{
            visibleChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#componentHidden; ex=" + ex );
        }
    }
    public void componentMoved(ComponentEvent e) {
        try{
            locationChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#componentMoved; ex=" + ex );
        }
    }
    public void componentResized(ComponentEvent e) {
        try{
            resizeEvent.raise( this, new BEventArgs() );
            sizeChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#componentResized; ex=" + ex );
        }
    }
    public void componentShown(ComponentEvent e) {
        try{
            visibleChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#componentShown; ex=" + ex );
        }
    }
}
