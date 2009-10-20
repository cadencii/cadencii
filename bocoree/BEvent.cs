/*
 * BEvent.cs
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

import java.util.*;
import java.lang.reflect.*;

public class BEvent<T extends IEventHandler>{
    private Vector<T> m_delegates;

    public BEvent(){
        m_delegates = new Vector<T>();
    }

    public void add( T delegate ){
        m_delegates.add( delegate );
    }

    public void remove( T delegate ){
    }

    public void raise( Object... args ) throws IllegalAccessException, InvocationTargetException{
        int count = m_delegates.size();
        for( int i = 0; i < count; i++ ){
            m_delegates.get( i ).invoke( args );
        }
    }
}
#endif
