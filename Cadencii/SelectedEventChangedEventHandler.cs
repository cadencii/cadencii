#if JAVA
package org.kbinani.Cadencii;

import org.kbinani.*;

public class SelectedEventChangedEventHandler implements IEventHandler
{
    private BDelegate m_delegate = null;
    private Object m_sender = null;

    public SelectedEventChangedEventHandler( Object sender, String method_name )
    {
        m_sender = sender;
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, Boolean.TYPE );
        }
        catch( Exception ex )
        {
            System.out.println( "SelectedEventChangedEventHandler#.ctor; ex=" + ex );
        }
    }

    public SelectedEventChangedEventHandler( Class sender, String method_name )
    {
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, Boolean.TYPE );
        }
        catch( Exception ex )
        {
            System.out.println( "SelectedEventChangedEventHandler#.ctor; ex=" + ex );
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
            System.out.println( "SelectedEventChangedEventHandler#invoke; ex=" + ex );
        }
    }
}
#else
namespace Boare.Cadencii
{

    public delegate void SelectedEventChangedEventHandler( object sender, bool selected_is_null );

}
#endif
