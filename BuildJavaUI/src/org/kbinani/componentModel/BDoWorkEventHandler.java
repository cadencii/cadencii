package org.kbinani.componentModel;

import java.lang.reflect.*;
import org.kbinani.*;

public class BDoWorkEventHandler implements IEventHandler{
    private BDelegate m_delegate = null;
    private Object m_sender = null;

    public BDoWorkEventHandler( Object sender, String method_name ){
        m_sender = sender;
        try{
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, BDoWorkEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BDoWorkEventHandler#.ctor; ex=" + ex );
        }
    }

    public BDoWorkEventHandler( Class sender, String method_name ){
        try{
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, BDoWorkEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BDoWorkEventHandler#.ctor; ex=" + ex );
        }
    }

    public void invoke( Object... arguments ){
        try{
            m_delegate.invoke( m_sender, arguments );
        }catch( Exception ex ){
            System.out.println( "BDoWorkEventHandler#invoke; ex=" + ex );
        }
    }
}
