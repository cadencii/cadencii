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
