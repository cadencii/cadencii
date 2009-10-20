#if JAVA
package org.kbinani.Cadencii;

import org.kbinani.*;

public class TopMostChangedEventHandler implements IEventHandler
{
    private BDelegate m_delegate = null;
    private Object m_sender = null;

    public TopMostChangedEventHandler( Object sender, String method_name )
    {
        m_sender = sender;
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, Boolean.TYPE );
        }
        catch( Exception ex )
        {
            System.out.println( "TopMostChangedEventHandler#.ctor; ex=" + ex );
        }
    }

    public TopMostChangedEventHandler( Class sender, String method_name )
    {
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, Boolean.TYPE );
        }
        catch( Exception ex )
        {
            System.out.println( "TopMostChangedEventHandler#.ctor; ex=" + ex );
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
            System.out.println( "TopMostChangedEventHandler#invoke; ex=" + ex );
        }
    }
}
#else
namespace Boare.Cadencii
{

    public delegate void TopMostChangedEventHandler( object sender, bool top_most );

}
#endif
