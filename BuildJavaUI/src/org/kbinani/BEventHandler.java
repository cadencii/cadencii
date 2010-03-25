package org.kbinani;

public class BEventHandler{
    protected BDelegate m_delegate = null;
    //protected Object m_invoker = null;

    protected BEventHandler( Class<?> invoker, String method_name, Class<?> return_type, Class<?> arg1, Class<?> arg2 ){
        try{
            m_delegate = new BDelegate( invoker, method_name, return_type, arg1, arg2 );
        }catch( Exception ex ){
            System.err.println( "BEventHandler#.ctor; ex=" + ex );
        }
    }
    
    protected BEventHandler( Object invoker, String method_name, Class<?> return_type, Class<?> arg1, Class<?> arg2 ){
        //m_invoker = invoker;
        try{
            m_delegate = new BDelegate( invoker, method_name, return_type, arg1, arg2 );
        }catch( Exception ex ){
            System.err.println( "BEventHandler#.ctor; ex=" + ex );
        }
    }

    public BEventHandler( Object invoker, String method_name ){
        this( invoker, method_name, Void.TYPE, Object.class, BEventArgs.class );
    }
    
    public BEventHandler( Class<?> invoker, String method_name ){
        this( invoker, method_name, Void.TYPE, Object.class, BEventArgs.class );
    }
    
    public boolean equals( Object item ){
        if( item == null ){
            return false;
        }
        if( !(item instanceof BEventHandler) ){
            return false;
        }
        BEventHandler casted = (BEventHandler)item;
        return m_delegate.equals( casted );
    }
    
    public void invoke( Object... arguments ){
        try{
            m_delegate.invoke( arguments );
        }catch( Exception ex ){
            System.err.println( "BEventHandler#invoke; ex=" + ex );
        }
    }
    
}
