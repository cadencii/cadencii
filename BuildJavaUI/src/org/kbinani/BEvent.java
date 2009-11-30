package org.kbinani;

import java.lang.reflect.InvocationTargetException;
import java.util.Vector;

public class BEvent<T extends BEventHandler>{
    private Vector<T> m_delegates;

    public BEvent(){
        m_delegates = new Vector<T>();
    }

    public void add( T delegate ){
        m_delegates.add( delegate );
    }

    public void remove( T delegate ){
        int count = m_delegates.size();
        for( int i = 0; i < count; i++ ){
            T item = m_delegates.get( i );
            if( delegate.equals( item ) ){
                m_delegates.remove( i );
                break;
            }
        }
    }
    
    public void raise( Object... args ) 
        throws IllegalAccessException, InvocationTargetException
    {
        int count = m_delegates.size();
        for( int i = 0; i < count; i++ ){
            m_delegates.get( i ).invoke( args );
        }
    }
}
