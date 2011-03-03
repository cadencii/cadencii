package org.kbinani.windows.forms;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import javax.swing.JCheckBoxMenuItem;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BMenuItem extends JCheckBoxMenuItem 
                       implements ActionListener
{
    private static final long serialVersionUID = -1354135252399786976L;
    private boolean checkOnClick = false;
    private String mShortcutDisplayString = "";
    public final BEvent<BEventHandler> clickEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> checkedChangedEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> mouseEnterEvent = new BEvent<BEventHandler>();
    public final BEvent<BEventHandler> mouseLeaveEvent = new BEvent<BEventHandler>();

    public BMenuItem(){
        addActionListener( this );
        final BMenuItem ref = this;
        this.addMouseListener( new MouseListener(){
            @Override
            public void mouseClicked(MouseEvent e) {
                // System.Windows.Forms.ToolStripMenuItemでは、
                // CheckedChangedイベントの後にClickイベントが発生するのでこれに準じる
                if( checkOnClick ){
                    invokeCheckedChangedEvent();
                }else{
                    setSelectedSuper( false );
                }
                try{
                    clickEvent.raise( ref, new BEventArgs() );
                }catch( Exception ex ){
                    System.err.println( "BMenuItem#mouseClicked; ex=" + ex );
                }
            }

            @Override
            public void mouseEntered(MouseEvent e) {
        System.out.println( "BMenuItem#mouseEntered" );
                try{
                    mouseEnterEvent.raise( ref, new BEventArgs() );
                }catch( Exception ex ){
                    System.err.println( "BMenuItem#mouseEntered; ex=" + ex );
                }
            }

            @Override
            public void mouseExited(MouseEvent e) {
                try{
                    mouseLeaveEvent.raise( ref, new BEventArgs() );
                }catch( Exception ex ){
                    System.err.println( "BMenuItem#mouseExited; ex=" + ex );
                }
            }

            @Override
            public void mousePressed(MouseEvent e) {
            }

            @Override
            public void mouseReleased(MouseEvent e) {
            }
        });
    }

    private void setSelectedSuper( boolean value ){
        super.setSelected( value );
    }
    
    public void setShortcutKeyDisplayString( String value )
    {
        mShortcutDisplayString = value;
    }
    
    public String getShortcutKeyDisplayString()
    {
        return mShortcutDisplayString;
    }

    @Override
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

    @Override
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

    public boolean isCheckOnClick(){
        return checkOnClick;
    }
    
    public void setCheckOnClick( boolean value ){
        checkOnClick = value;
    }

}
