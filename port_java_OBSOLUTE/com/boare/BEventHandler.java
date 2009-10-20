package com.boare;

import java.lang.reflect.*;

public class BEventHandler{
    private BDelegate m_delegate = null;

    public BEventHandler( Object sender, String method_name ){
        try{
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, BEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BEventHandler#.ctor; ex=" + ex );
        }
    }

    public BEventHandler( Class sender, String method_name ){
        try{
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, BEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BEventHandler#.ctor; ex=" + ex );
        }
    }

    public void invoke( Object... arguments ){
        try{
            m_delegate.invoke( arguments );
        }catch( Exception ex ){
            System.out.println( "BEventHandler#invoke; ex=" + ex );
        }
    }
}
