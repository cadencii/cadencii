package com.github.cadencii.windows.forms;

import java.awt.AWTEvent;
import java.awt.Component;
import java.awt.Dimension;
import java.awt.Toolkit;
import java.awt.event.AWTEventListener;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JDialog;
import javax.swing.UIManager;
import com.github.cadencii.BEvent;
import com.github.cadencii.BEventArgs;
import com.github.cadencii.BEventHandler;

public class BDialog extends JDialog 
                   implements WindowListener, 
                              KeyListener, 
                              ComponentListener,
                              AWTEventListener
{
    private static final long serialVersionUID = 6813116345545558212L;

    public final BEvent<BFormClosingEventHandler> formClosingEvent = new BEvent<BFormClosingEventHandler>();
    public final BEvent<BFormClosedEventHandler> formClosedEvent = new BEvent<BFormClosedEventHandler>();
    public final BEvent<BEventHandler> activatedEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> deactivateEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> loadEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> windowStateChangedEvent = new BEvent<BEventHandler>();
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

    /**
     * ESCを押したときにクリックするボタン
     */
    private JButton mCancelButton = null;
    public void setCancelButton( JButton button )
    {
        mCancelButton = button;
    }
    public void eventDispatched( AWTEvent arg0 ) {
        if ( mCancelButton == null ){
            return;
        }
        if( !(arg0 instanceof KeyEvent ) ){
            return;
        }
        KeyEvent e = (KeyEvent)arg0;
        int state = e.getID();
        if ( state != KeyEvent.KEY_PRESSED ){
            return;
        }
        Object obj = e.getComponent();
        if ( obj == null ){
            return;
        }
        int code = e.getKeyCode();
        if ( code == KeyEvent.VK_ESCAPE ){
            if ( obj instanceof JComboBox ){
                JComboBox cb = (JComboBox)obj;
                if( cb.isPopupVisible() ){
                    // ポップアップが表示中の場合は何もしない
                    return;
                }
            }
            mCancelButton.doClick();
        }
    }
    
    public BDialogResult showDialog( Component parent ){
        setModalityType( ModalityType.APPLICATION_MODAL );
		//setModal( true );
        setVisible( true );
        return this.m_result;
    }

    // root impl of KeyListener is in BButton
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
            Toolkit.getDefaultToolkit().addAWTEventListener( this, AWTEvent.KEY_EVENT_MASK );
        }catch( Exception ex ){
            System.err.println( "BForm#windowActivated; ex=" + ex );
        }
    }
    
    public void windowClosed( WindowEvent e ){
        Toolkit.getDefaultToolkit().removeAWTEventListener( this );
        try{
            formClosedEvent.raise( this, new BFormClosedEventArgs() );
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
            Toolkit.getDefaultToolkit().removeAWTEventListener( this );
        }catch( Exception ex ){
            System.err.println( "BForm#windowDeactivated; ex=" + ex );
        }
    }

    @Override
    public void windowDeiconified( WindowEvent e ){
        try{
            windowStateChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BDialog#windowDeiconified; ex=" + ex );
        }
    }
    
    @Override
    public void windowIconified( WindowEvent e ){
        try{
            windowStateChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BDialog#windowIconified; ex=" + ex );
        }
    }
    
    public void windowOpened( WindowEvent e ){
        try{
            loadEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BForm#windowOpened; ex=" + ex );
        }
    }
    
    public final BEvent<BEventHandler> sizeChangedEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> locationChangedEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> resizeEvent = new BEvent<BEventHandler>();
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
