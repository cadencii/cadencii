package com.boare;

import java.util.*;
import java.lang.reflect.*;

public class BEvent{
    private Vector<BEventHandler> m_delegates;

    public BEvent(){
        m_delegates = new Vector<BEventHandler>();
    }

    public void add( BEventHandler delegate ){
        m_delegates.add( delegate );
    }

    public void remove( BEventHandler delegate ){
    }

    public void raise( Object... args ) throws IllegalAccessException, InvocationTargetException{
        int count = m_delegates.size();
        for( int i = 0; i < count; i++ ){
            m_delegates.get( i ).invoke( args );
        }
    }
}
