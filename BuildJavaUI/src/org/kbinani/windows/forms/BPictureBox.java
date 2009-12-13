package org.kbinani.windows.forms;

import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import javax.swing.JPanel;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BPictureBox extends JPanel
                         implements MouseListener,
                                    MouseMotionListener,
                                    KeyListener
{
    private static final long serialVersionUID = 5793624638905606676L;
    public BEvent<BKeyEventHandler> bKeyDownEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyEventHandler> bKeyUpEvent = new BEvent<BKeyEventHandler>();
    private Image m_image;

    public BPictureBox(){
        super();
        addMouseListener( this );
        addMouseMotionListener( this );
        addKeyListener( this );
    }

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

    // root impl of MouseMotionListener is in BButton
    public BEvent<BMouseEventHandler> mouseMoveEvent = new BEvent<BMouseEventHandler>();
    public void mouseDragged( MouseEvent e ){
        try{
            mouseMoveEvent.raise( this, BMouseEventArgs.fromMouseEvent( e ) );
        }catch( Exception ex ){
            System.err.println( "BButton#mouseDragged; ex=" + ex );
        }
    }    
    public void mouseMoved( MouseEvent e ){
        try{
            mouseMoveEvent.raise( this, BMouseEventArgs.fromMouseEvent( e ) );
        }catch( Exception ex ){
            System.err.println( "BButton#mouseMoved; ex=" + ex );
        }
    }

    // root impl of MouseListener is in BButton
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

    public Image getImage(){
        return m_image;
    }

    public void setImage( Image img ){
        m_image = img;
    }

    public BEvent<BPaintEventHandler> paintEvent = new BEvent<BPaintEventHandler>();
    public void paint( Graphics g1 ){
        if ( m_image != null ){
            Graphics2D g = (Graphics2D)g1;
            g.drawImage( m_image, 0, 0, m_image.getWidth( this ), m_image.getHeight( this ), this );
        }
        try{
            paintEvent.raise( this, new BPaintEventArgs( g1 ) );
        }catch( Exception ex ){
            System.err.println( "BPictureBox#paint; ex=" + ex );
        }
    }
}
