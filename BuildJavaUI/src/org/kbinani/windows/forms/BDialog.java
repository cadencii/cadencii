package org.kbinani.windows.forms;

import java.awt.Component;
import java.awt.Dimension;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import javax.swing.JDialog;
import javax.swing.UIManager;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BDialog extends JDialog 
                   implements WindowListener, 
                              KeyListener, 
                              ComponentListener
{
    private static final long serialVersionUID = 6813116345545558212L;

    public BEvent<BFormClosingEventHandler> formClosingEvent = new BEvent<BFormClosingEventHandler>();
    public BEvent<BEventHandler> formClosedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> activatedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> deactivateEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> loadEvent = new BEvent<BEventHandler>();
    private BDialogResult m_result = BDialogResult.CANCEL;
    
    public BDialog(){
        super();
        addWindowListener( this );
        addKeyListener( this );
        addComponentListener( this );
        setDefaultCloseOperation( DO_NOTHING_ON_CLOSE );
        try{
            UIManager.getInstalledLookAndFeels();
            UIManager.setLookAndFeel( UIManager.getSystemLookAndFeelClassName() );
        }catch( Exception ex ){
            System.err.println( "BDialog#.ctor; ex=" + ex );
        }
    }
    
    public BDialogResult showDialog( Component parent ){
        setModalityType( ModalityType.APPLICATION_MODAL );
        setVisible( true );
        return this.m_result;
    }

    // root impl of KeyListener is in BButton
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

    public Dimension getClientSize(){
        return getContentPane().getSize();
    }
    
    public void close(){
        setVisible( false );
        try{
            BFormClosingEventArgs e = new BFormClosingEventArgs();
            formClosingEvent.raise( this, e );
            if( e.Cancel ){
                setVisible( true );
                return;
            }
        }catch( Exception ex ){
            System.err.println( "BForm#close; ex=" + ex );
        }
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
            formClosedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#windowClosed; ex=" + ex );
        }
    }
    
    public void windowClosing( WindowEvent e ){
        try{
            BFormClosingEventArgs ev = new BFormClosingEventArgs();
            formClosingEvent.raise( this, ev );
            if( !ev.Cancel ){
                dispose();
            }
        }catch( Exception ex ){
            System.err.println( "BDialog#windowClosing; ex=" + ex );
        }
    }
    
    public void windowDeactivated( WindowEvent e ){
        try{
            deactivateEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#windowDeactivated; ex=" + ex );
        }
    }
    
    public void windowDeiconified( WindowEvent e ){
    }
    
    public void windowIconified( WindowEvent e ){
    }
    
    public void windowOpened( WindowEvent e ){
        try{
            loadEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#windowOpened; ex=" + ex );
        }
    }
    
    public BEvent<BEventHandler> sizeChangedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> locationChangedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> resizeEvent = new BEvent<BEventHandler>();
    public BDialogResult getDialogResult(){
        return m_result;
    }
    
    public void setDialogResult( BDialogResult value ){
        m_result = value;
        setVisible( false );
    }
    
    public void componentHidden(ComponentEvent e) {
        // TODO 自動生成されたメソッド・スタブ
        
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
        // TODO 自動生成されたメソッド・スタブ
        
    }
}
