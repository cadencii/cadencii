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
boolean removed = false;
        for( int i = 0; i < count; i++ ){
            T item = m_delegates.get( i );
            if( delegate.equals( item ) ){
                m_delegates.remove( i );
                removed = true;
                break;
            }
        }
if( removed ){
    System.err.println( "BEvent#remove; delegate was successfully removed" );
}else{
    System.err.println("BEvent#remove; delegate was NOT successfully removed" );
}
System.err.println( "BEvent#remove; delegate.m_delegate.m_name=" + delegate.m_delegate.m_name + "; this.m_delegates.size()=" + this.m_delegates.size() );
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
