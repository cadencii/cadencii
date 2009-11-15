package org.kbinani.windows.forms;

import org.kbinani.BDelegate;
import org.kbinani.IEventHandler;

public class BMouseEventHandler implements IEventHandler{
    private BDelegate m_delegate = null;
    private Object m_sender = null;
    
    public BMouseEventHandler( Object sender, String method_name ){
        m_sender = sender;
        try{
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, BMouseEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BMouseEventHandler#.ctor; ex=" + ex );
        }
    }
    
    public BMouseEventHandler( Class sender, String method_name ){
        try{
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, BMouseEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BMouseEventHandler#.ctor; ex=" + ex );
        }
    }
    
    public void invoke( Object... arguments ){
        try{
            m_delegate.invoke( m_sender, arguments );
        }catch( Exception ex ){
            System.out.println( "BMouseEventHandler#invoke; ex=" + ex );
        }
    }
}
