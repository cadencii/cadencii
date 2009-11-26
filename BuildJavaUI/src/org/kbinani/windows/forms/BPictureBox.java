package org.kbinani.windows.forms;

import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Image;
import javax.swing.JPanel;
import org.kbinani.BEvent;

public class BPictureBox extends JPanel{
    private static final long serialVersionUID = 5793624638905606676L;
    public BEvent<BKeyEventHandler> keyDownEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyEventHandler> keyUpEvent = new BEvent<BKeyEventHandler>();
    private Image m_image;

    public BPictureBox(){
        super();
    }

    public Image getImage(){
        return m_image;
    }

    public void setImage( Image img ){
        m_image = img;
    }

    public void paint( Graphics g1 ){
        if ( m_image != null ){
            Graphics2D g = (Graphics2D)g1;
            g.drawImage( m_image, 0, 0, m_image.getWidth( this ), m_image.getHeight( this ), this );
        }
    }
}
