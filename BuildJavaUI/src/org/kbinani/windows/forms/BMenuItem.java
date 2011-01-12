package org.kbinani.windows.forms;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import javax.swing.JCheckBoxMenuItem;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BMenuItem extends JCheckBoxMenuItem implements ActionListener, MouseListener{
    private static final long serialVersionUID = -1354135252399786976L;
    private Object tag;
    private boolean checkOnClick = true;
    public BEvent<BEventHandler> clickEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> checkedChangedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> mouseEnterEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> mouseLeaveEvent = new BEvent<BEventHandler>();

    public BMenuItem(){
        addActionListener( this );
    }
    
    public void actionPerformed( ActionEvent e ){
        if( checkOnClick ){
            this.setSelected( !this.isSelected() );
            try{
                checkedChangedEvent.raise( this, new BEventArgs() );
            }catch( Exception ex ){
                System.err.println( "BMenuItem#actionPerformed; ex=" + ex );
            }
        }
        try{
            clickEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BMenuItem#actionPerformed; ex=" + ex );
        }
    }

    public Object getTag(){
        return tag;
    }
    
    public void setTag( Object value ){
        tag = value;
    }

    public boolean isCheckOnClick(){
        return checkOnClick;
    }
    
    public void setCheckOnClick( boolean value ){
        checkOnClick = value;
    }

    public void mouseClicked(MouseEvent e) {
        // TODO �����������ꂽ���\�b�h�E�X�^�u
        
    }

    public void mouseEntered(MouseEvent e) {
        try{
            mouseEnterEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BMenuItem#mouseEntered; ex=" + ex );
        }
    }

    public void mouseExited(MouseEvent e) {
        try{
            mouseLeaveEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BMenuItem#mouseExited; ex=" + ex );
        }
    }

    public void mousePressed(MouseEvent e) {
        // TODO �����������ꂽ���\�b�h�E�X�^�u
        
    }

    public void mouseReleased(MouseEvent e) {
        // TODO �����������ꂽ���\�b�h�E�X�^�u
        
    }
}
