package com.boare.cadencii;

import java.util.*;
import java.lang.reflect.*;
import com.boare.corlib.*;

public class Event{
    private Vector<EventHandler> m_handler;

    public Event(){
        m_handler = new Vector<EventHandler>();
    }

    public void invoke( Object... argument ){
        try{
            for( EventHandler handler : m_handler ){
                handler.invoke( argument );
            }
        }catch( Exception ex ){
            System.out.println( "Event.invoke; ex=" + ex );
        }
    }

    public void add( EventHandler handler ){
        m_handler.add( handler );
    }

    public void remove( EventHandler handler ){
        int c = m_handler.size();
        for( int i = 0; i < c; i++ ){
            if( m_handler.get( i ) == handler ){
                m_handler.removeElementAt( i );
                break;
            }
        }
    }
}
