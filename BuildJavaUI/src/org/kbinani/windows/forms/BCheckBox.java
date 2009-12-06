package org.kbinani.windows.forms;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import javax.swing.JCheckBox;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BCheckBox extends JCheckBox implements ActionListener, MouseListener{
    private static final long serialVersionUID = 1L;
    public BEvent<BEventHandler> checkedChangedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> clickEvent = new BEvent<BEventHandler>();

    public BCheckBox(){
        super();
        addActionListener( this );
    }

    public void actionPerformed( ActionEvent e ){
        try{
            checkedChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BCheckBox#actionPerformed; ex=" + ex );
        }
    }

    @Override
    public void mouseClicked( MouseEvent e ){
        try{
            clickEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BCheckBox#mouseClicked; ex=" + ex );
        }
    }

    @Override
    public void mouseEntered(MouseEvent e) {
        // TODO 自動生成されたメソッド・スタブ
        
    }

    @Override
    public void mouseExited(MouseEvent e) {
        // TODO 自動生成されたメソッド・スタブ
        
    }

    @Override
    public void mousePressed(MouseEvent e) {
        // TODO 自動生成されたメソッド・スタブ
        
    }

    @Override
    public void mouseReleased(MouseEvent e) {
        // TODO 自動生成されたメソッド・スタブ
        
    }
}
