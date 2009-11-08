#if JAVA
package org.kbinani.Cadencii;

import java.lang.reflect.*;

public class SoloChangedEventHandler implements IEventHandler{
    private BDelegate m_delegate = null;
    private Object m_invoker = null;

    public SoloChangedEventHandler( Object invoker, String method_name ){
        m_invoker = invoker;
        try{
            m_delegate = new BDelegate( m_invoker, method_name, Void.TYPE, Integer.TYPE, Boolean.TYPE );
        }catch( Exception ex ){
            System.out.println( "SoloChangedEventHandler#.ctor; ex=" + ex );
        }
    }

    public SoloChangedEventHandler( Class invoker, String method_name ){
        try{
            m_delegate = new BDelegate( invoker, method_name, Void.TYPE, Integer.TYPE, Boolean.TYPE );
        }catch( Exception ex ){
            System.out.println( "SoloChangedEventHandler#.ctor; ex=" + ex );
        }
    }

    public void invoke( Object... arguments ){
        try{
            m_delegate.invoke( m_invoker, arguments );
        }catch( Exception ex ){
            System.out.println( "SoloChangedEventHandler#invoke; ex=" + ex );
        }
    }
}
#else
namespace Boare.Cadencii {

    public delegate void SoloChangedEventHandler( int track, bool solo );

}
#endif
