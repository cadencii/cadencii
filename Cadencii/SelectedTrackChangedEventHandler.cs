#if JAVA
package org.kbinani.Cadencii;

import org.kbinani.*;

public class SelectedTrackChangedEventHandler implements IEventHandler
{
    private BDelegate m_delegate = null;
    private Object m_sender = null;

    public SelectedTrackChangedEventHandler( Object sender, String method_name )
    {
        m_sender = sender;
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, Integer.TYPE );
        }
        catch( Exception ex )
        {
            System.out.println( "SelectedTrackChangedEventHandler#.ctor; ex=" + ex );
        }
    }

    public SelectedTrackChangedEventHandler( Class sender, String method_name )
    {
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, Integer.TYPE );
        }
        catch( Exception ex )
        {
            System.out.println( "SelectedTrackChangedEventHandler#.ctor; ex=" + ex );
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
            System.out.println( "SelectedTrackChangedEventHandler#invoke; ex=" + ex );
        }
    }
}
#else
using System;

namespace Boare.Cadencii
{

    public delegate void SelectedTrackChangedEventHandler( Object sender, int selected_track );

}
#endif
