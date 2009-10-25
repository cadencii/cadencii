/*
 * RenderRequiredEventHandler.cs
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

public class RenderRequiredEventHandler implements IEventHandler
{
    private BDelegate m_delegate = null;
    private Object m_sender = null;

    public RenderRequiredEventHandler( Object sender, String method_name )
    {
        m_sender = sender;
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, int[].class );
        }
        catch( Exception ex )
        {
            System.out.println( "RenderRequiredEventHandler#.ctor; ex=" + ex );
        }
    }

    public RenderRequiredEventHandler( Class sender, String method_name )
    {
        try
        {
            m_delegate = new BDelegate( sender, method_name, Void.TYPE, Object.class, int[].class );
        }
        catch( Exception ex )
        {
            System.out.println( "RenderRequiredEventHandler#.ctor; ex=" + ex );
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
            System.out.println( "RenderRequiredEventHandler#invoke; ex=" + ex );
        }
    }
}
#else
using System;

namespace Boare.Cadencii {

    public delegate void RenderRequiredEventHandler( Object sender, int[] tracks );

}
#endif