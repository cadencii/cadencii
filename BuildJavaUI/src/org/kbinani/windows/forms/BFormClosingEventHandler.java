package org.kbinani.windows.forms;

import org.kbinani.BDelegate;
import org.kbinani.IEventHandler;

public class BFormClosingEventHandler implements IEventHandler{
    private BDelegate m_delegate = null;
    private Object m_invoker = null;
    
    public BFormClosingEventHandler( Object invoker, String method_name ){
        m_invoker = invoker;
        try{
            m_delegate = new BDelegate( m_invoker, method_name, Void.TYPE, Object.class, BFormClosingEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BFormClosingEventHandler#.ctor; ex=" + ex );
        }
    }
    
    public BFormClosingEventHandler( Class<?> invoker, String method_name ){
        try{
            m_delegate = new BDelegate( invoker, method_name, Void.TYPE, Object.class, BFormClosingEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BFormClosingEventHandler#.ctor; ex=" + ex );
        }
    }
    
    public void invoke( Object... arguments ){
        try{
            m_delegate.invoke( m_invoker, arguments );
        }catch( Exception ex ){
            System.out.println( "BFormClosingEventHandler#invoke; ex=" + ex );
        }
    }
}
