package org.kbinani.windows.forms;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import javax.swing.JMenuItem;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BMenuItem extends JMenuItem implements ActionListener{
    private static final long serialVersionUID = -1354135252399786976L;
    private Object tag;
    private boolean checkOnClick = true;
    public BEvent<BEventHandler> clickEvent = new BEvent<BEventHandler>();

    public BMenuItem(){
        addActionListener( this );
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

    public boolean isCheckOnClick(){
        return checkOnClick;
    }
    
    public void setCheckOnClick( boolean value ){
        checkOnClick = value;
    }
}
