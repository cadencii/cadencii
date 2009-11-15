package org.kbinani.windows.forms;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import javax.swing.JToggleButton;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BToolStripButton extends JToggleButton implements ActionListener{
    private static final long serialVersionUID = -9098464491775550703L;
    public BEvent<BEventHandler> clickEvent = new BEvent<BEventHandler>();
    private Object tag;
    private boolean checkOnClick = true;
    
    public BToolStripButton(){
        super();
        addActionListener( this );
    }
    
    public boolean isCheckOnClick(){
        return checkOnClick;
    }
    
    public void setCheckOnClick( boolean value ){
        checkOnClick = value;
    }
    
    public void actionPerformed( ActionEvent e ){
        if( checkOnClick ){
            this.setSelected( !this.isSelected() );
        }
        try{
            clickEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BToolStripButton#actionPerformed; ex=" + ex );
        }
    }
    
    public Object getTag(){
        return tag;
    }
    
    public void setTag( Object value ){
        tag = value;
    }
}
