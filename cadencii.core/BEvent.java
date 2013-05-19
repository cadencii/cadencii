package com.github.cadencii;

import java.lang.reflect.InvocationTargetException;
import java.util.Vector;

public class BEvent<T extends BEventHandler>{
    private Vector<T> mDelegates;

    public BEvent(){
        mDelegates = new Vector<T>();
    }

    public int size()
    {
        return mDelegates.size();
    }

    public void add( T delegate ){
        synchronized( mDelegates ){
            if( delegate == null ){
                return;
            }
            mDelegates.add( delegate );
        }
    }

    public void remove( T delegate ){
        synchronized( mDelegates ){
            int count = mDelegates.size();
            for( int i = 0; i < count; i++ ){
                T item = mDelegates.get( i );
                if( delegate.equals( item ) ){
                    mDelegates.remove( i );
                    break;
                }
            }
        }
    }

    public void raise( Object... args )
        throws IllegalAccessException, InvocationTargetException
    {
        int count = mDelegates.size();
        for( int i = 0; i < count; i++ ){
            mDelegates.get( i ).invoke( args );
        }
    }
}
