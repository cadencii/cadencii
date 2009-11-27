package org.kbinani.componentModel;

import org.kbinani.*;

public class BProgressChangedEventHandler implements IEventHandler
{
    private BDelegate m_delegate = null;
    private Object m_sender = null;

    public BProgressChangedEventHandler( Object sender, String method_name )
    {
        m_sender = sender;
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, BProgressChangedEventArgs.class );
        }
        catch( Exception ex )
        {
            System.out.println( "BProgressChangedEventHandler#.ctor; ex=" + ex );
        }
    }

    public BProgressChangedEventHandler( Class sender, String method_name )
    {
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, BProgressChangedEventArgs.class );
        }
        catch( Exception ex )
        {
            System.out.println( "BProgressChangedEventHandler#.ctor; ex=" + ex );
        }
    }

    public void invoke( Object... arguments )
    {
        try
        {
            m_delegate.invoke( m_sender, arguments );
        }
        catch( Exception ex )
        {
            System.out.println( "BProgressChangedEventHandler#invoke; ex=" + ex );
        }
    }
}
