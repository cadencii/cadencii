package com.boare.corlib;

import java.lang.reflect.*;

public class EventHandler{
    private Delegate m_delegate = null;
    private Object m_sender = null;

    public EventHandler( Object sender, String method_name, Class... argument_types ) throws Exception{
        this( sender.getClass(), method_name, argument_types );
        m_sender = sender;
    }

    public EventHandler( Class sender, String method_name, Class... argument_types ) throws Exception{
        m_sender = sender;
        Class[] cls = new Class[argument_types.length + 1];
        cls[0] = Object.class;
        for( int i = 1; i < cls.length; i++ ){
            cls[i] = argument_types[i - 1];
        }
        m_delegate = new Delegate( sender, method_name, Void.TYPE, cls );
    }

    public void invoke( Object... arguments ) throws IllegalAccessException, InvocationTargetException{
        Object[] objs = new Object[arguments.length + 1];
        objs[0] = m_sender;
        for( int i = 1; i < objs.length; i++ ){
            objs[i] = arguments[i - 1];
        }
        m_delegate.invoke( objs );
    }
}
