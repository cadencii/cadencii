#if JAVA
package org.kbinani.Cadencii;

import java.lang.reflect.*;
import org.kbinani.*;

public class MuteChangedEventHandler implements IEventHandler{
    private BDelegate m_delegate = null;
    private Object m_invoker = null;

    public MuteChangedEventHandler( Object invoker, String method_name ){
        m_invoker = invoker;
        try{
            m_delegate = new BDelegate( m_invoker, method_name, Void.TYPE, Integer.TYPE, Boolean.TYPE );
        }catch( Exception ex ){
            System.out.println( "MuteChangedEventHandler#.ctor; ex=" + ex );
        }
    }

    public MuteChangedEventHandler( Class invoker, String method_name ){
        try{
            m_delegate = new BDelegate( invoker, method_name, Void.TYPE, Integer.TYPE, Boolean.TYPE );
        }catch( Exception ex ){
            System.out.println( "MuteChangedEventHandler#.ctor; ex=" + ex );
        }
    }

    public void invoke( Object... arguments ){
        try{
            m_delegate.invoke( m_invoker, arguments );
        }catch( Exception ex ){
            System.out.println( "MuteChangedEventHandler#invoke; ex=" + ex );
        }
    }
}
#else
namespace Boare.Cadencii {

    public delegate void MuteChangedEventHandler( int track, bool mute );

}
#endif
