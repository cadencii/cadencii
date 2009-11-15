package org.kbinani.windows.forms;

import org.kbinani.BDelegate;
import org.kbinani.IEventHandler;

public class BKeyEventHandler implements IEventHandler{
    private BDelegate m_delegate = null;
    private Object m_invoker = null;
    
    public BKeyEventHandler( Object invoker, String method_name ){
        m_invoker = invoker;
        try{
            m_delegate = new BDelegate( m_invoker, method_name, Void.TYPE, Object.class, BKeyEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BKeyEventHandler#.ctor; ex=" + ex );
        }
    }
    
    public BKeyEventHandler( Class<?> invoker, String method_name ){
        try{
            m_delegate = new BDelegate( invoker, method_name, Void.TYPE, Object.class, BKeyEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BKeyEventHandler#.ctor; ex=" + ex );
        }
    }
    
    public void invoke( Object... arguments ){
        try{
            m_delegate.invoke( m_invoker, arguments );
        }catch( Exception ex ){
            System.out.println( "BKeyEventHandler#invoke; ex=" + ex );
        }
    }
}
