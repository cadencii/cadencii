/*
 * BDelegate.cs
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

public class BDelegate
{
    Method m_method = null;
    Object m_invoker = null;

    public BDelegate( Class invoker, String method_name, Class return_type, Class... argument_type )
        throws Exception
    {
        m_invoker = null;
        for( Method method : invoker.getDeclaredMethods() )
        {
            if( !method.getName().equals( method_name ) )
            {
                continue;
            }
            if( !compareType( method.getReturnType(), return_type ) )
            {
                continue;
            }
            Class[] param = method.getParameterTypes();
            if( param.length != argument_type.length )
            {
                continue;
            }
            boolean same = true;
            for( int i = 0; i < param.length; i++ )
            {
                if( !compareType( param[i], argument_type[i] ) )
                {
                    same = false;
                    break;
                }
            }
            if( same )
            {
                m_method = method;
                break;
            }
        }
        if( m_method == null )
        {
            throw new Exception( "don't know such method" );
        }
    }

    public BDelegate( Object invoker, String method_name, Class return_type, Class... argument_type )
        throws Exception
    {
        this( invoker.getClass(), method_name, return_type, argument_type );
        m_invoker = invoker;
    }

    public Object invoke( Object... args )
        throws IllegalAccessException, InvocationTargetException
    {
        Object ret = null;
        try
        {
            ret = m_method.invoke( m_invoker, args );
        }
        catch( Exception ex )
        {
            System.out.println( "BDelegate#invoke; ex=" + ex );
        }
        return ret;
    }

    private boolean compareType( Class cls1, Class cls2 )
    {
        Class cls1t = transform( cls1 );
        Class cls2t = transform( cls2 );
        return cls1t.equals( cls2t );
    }

    private Class transform( Class cls )
    {
        if( cls.equals( Integer.class ) )
        {
            return Integer.TYPE;
        }
        else if( cls.equals( Byte.class ) )
        {
            return Byte.TYPE;
        }
        else if( cls.equals( Void.class ) )
        {
            return Void.TYPE;
        }
        else if( cls.equals( Double.class ) )
        {
            return Double.TYPE;
        }
        else if( cls.equals( Float.class ) )
        {
            return Float.TYPE;
        }
        else if( cls.equals( Boolean.class ) )
        {
            return Boolean.TYPE;
        }
        else if( cls.equals( Character.class ) )
        {
            return Character.TYPE;
        }
        else if( cls.equals( Long.class ) )
        {
            return Long.TYPE;
        }
        else if( cls.equals( Short.class ) )
        {
            return Short.TYPE;
        }
        else
        {
            return cls;
        }
    }
}
#endif
