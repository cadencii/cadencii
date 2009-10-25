/*
 * SelectedEventChangedEventHandler.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
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
namespace Boare.Cadencii {

    public delegate void SelectedEventChangedEventHandler( object sender, bool selected_is_null );

}
#endif
