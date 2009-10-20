#if JAVA
package org.kbinani.Cadencii;

import org.kbinani.*;

public class SelectedCurveChangedEventHandler implements IEventHandler
{
    private BDelegate m_delegate = null;
    private Object m_sender = null;

    public SelectedCurveChangedEventHandler( Object sender, String method_name )
    {
        m_sender = sender;
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, CurveType.class );
        }
        catch( Exception ex )
        {
            System.out.println( "SelectedCurveChangedEventHandler#.ctor; ex=" + ex );
        }
    }

    public SelectedCurveChangedEventHandler( Class sender, String method_name )
    {
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, CurveType.class );
        }
        catch( Exception ex )
        {
            System.out.println( "SelectedCurveChangedEventHandler#.ctor; ex=" + ex );
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
            System.out.println( "SelectedCurveChangedEventHandler#invoke; ex=" + ex );
        }
    }
}
#else
using System;

namespace Boare.Cadencii
{

    public delegate void SelectedCurveChangedEventHandler( Object sender, CurveType curve );

}
#endif