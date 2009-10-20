/*
 * BEventHandler.cs
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
package org.kbinani;

import java.lang.reflect.*;

public class BEventHandler implements IEventHandler{
    private BDelegate m_delegate = null;
    private Object m_invoker = null;

    public BEventHandler( Object invoker, String method_name ){
        m_invoker = invoker;
        try{
            m_delegate = new BDelegate( m_invoker, method_name, Void.TYPE, Object.class, BEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BEventHandler#.ctor; ex=" + ex );
        }
    }

    public BEventHandler( Class invoker, String method_name ){
        try{
            m_delegate = new BDelegate( invoker, method_name, Void.TYPE, Object.class, BEventArgs.class );
        }catch( Exception ex ){
            System.out.println( "BEventHandler#.ctor; ex=" + ex );
        }
    }

    public void invoke( Object... arguments ){
        try{
            m_delegate.invoke( m_invoker, arguments );
        }catch( Exception ex ){
            System.out.println( "BEventHandler#invoke; ex=" + ex );
        }
    }
}
#endif
