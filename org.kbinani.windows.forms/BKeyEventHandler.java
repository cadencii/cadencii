package com.github.cadencii.windows.forms;

import org.kbinani.BEventHandler;

public class BKeyEventHandler extends BEventHandler{
    public BKeyEventHandler( Object invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, BKeyEventArgs.class );
    }
    
    public BKeyEventHandler( Class<?> invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, BKeyEventArgs.class );
    }
}
