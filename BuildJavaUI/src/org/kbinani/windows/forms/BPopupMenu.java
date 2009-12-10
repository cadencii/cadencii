package org.kbinani.windows.forms;

import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import javax.swing.JPopupMenu;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BPopupMenu extends JPopupMenu implements ComponentListener {
    private static final long serialVersionUID = 363411779635481115L;
    private Object tag = null;
    public BEvent<BEventHandler> visibleChangedEvent = new BEvent<BEventHandler>();

    public Object getTag(){
        return tag;
    }
    
    public void setTag( Object value ){
        tag = value;
    }

    public void componentHidden(ComponentEvent e) {
        try{
            visibleChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BPopupMenu#componentHidden; ex=" + ex );
        }
    }

    public void componentMoved(ComponentEvent e) {
        // TODO 自動生成されたメソッド・スタブ
        
    }

    public void componentResized(ComponentEvent e) {
        // TODO 自動生成されたメソッド・スタブ
        
    }

    public void componentShown(ComponentEvent e) {
        try{
            visibleChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BPopupMenu#componentShown; ex=" + ex );
        }
    }
}
