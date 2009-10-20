/*
 * BRunWorkerCompletedEventhandler.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.componentModel;

import org.kbinani.*;

public class BRunWorkerCompletedEventHandler implements IEventHandler
{
    private BDelegate m_delegate = null;
    private Object m_sender = null;

    public BRunWorkerCompletedEventHandler( Object sender, String method_name )
    {
        m_sender = sender;
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, BRunWorkerCompletedEventArgs.class );
        }
        catch( Exception ex )
        {
            System.out.println( "BRunWorkerCompletedEventHandler#.ctor; ex=" + ex );
        }
    }

    public BRunWorkerCompletedEventHandler( Class sender, String method_name )
    {
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, BRunWorkerCompletedEventArgs.class );
        }
        catch( Exception ex )
        {
            System.out.println( "BRunWorkerCompletedEventHandler#.ctor; ex=" + ex );
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
            System.out.println( "BRunWorkerCompletedEventHandler#invoke; ex=" + ex );
        }
    }
}
#endif
