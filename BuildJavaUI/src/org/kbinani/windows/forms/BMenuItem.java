package org.kbinani.windows.forms;

import java.awt.Graphics;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import javax.swing.JCheckBoxMenuItem;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BMenuItem extends JCheckBoxMenuItem 
                       implements ActionListener, MouseListener
{
    private static final long serialVersionUID = -1354135252399786976L;
    private Object tag;
    private boolean checkOnClick = false;
    private String mShortcutDisplayString = "";
    public BEvent<BEventHandler> clickEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> checkedChangedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> mouseEnterEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> mouseLeaveEvent = new BEvent<BEventHandler>();

    public BMenuItem(){
        addActionListener( this );
        addMouseListener( this );
    }

    public void setShortcutKeyDisplayString( String value )
    {
        mShortcutDisplayString = value;
    }
    
    public String getShortcutKeyDisplayString()
    {
        return mShortcutDisplayString;
    }
    
    public void setSelected( boolean value )
    {
        if( super.isSelected() != value ){
            super.setSelected( value );
            invokeCheckedChangedEvent();
        }
    }
    
    /**
     * CheckedChangedイベントに登録されたメソッドを呼び出します
     */
    private void invokeCheckedChangedEvent()
    {
        try{
            checkedChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BMenuItem#invokeCheckedChangedEvent; ex=" + ex );
        }
    }
    
    public void actionPerformed( ActionEvent e )
    {
        if( checkOnClick ){
            invokeCheckedChangedEvent();
        }else{
            super.setSelected( false );
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
        // System.Windows.Forms.ToolStripMenuItemでは、
        // CheckedChangedイベントの後にClickイベントが発生するのでこれに準じる
        if( checkOnClick ){
            invokeCheckedChangedEvent();
        }else{
            super.setSelected( false );
        }
        try{
            clickEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BMenuItem#mouseClicked; ex=" + ex );
        }
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
    }

    public void mouseReleased(MouseEvent e) {
    }
}
