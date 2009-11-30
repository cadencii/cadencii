package org.kbinani.windows.forms;

import org.kbinani.BEventHandler;

public class BFormClosingEventHandler extends BEventHandler{
    public BFormClosingEventHandler( Object invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, BFormClosingEventArgs.class );
    }
    
    public BFormClosingEventHandler( Class<?> invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, BFormClosingEventArgs.class );
    }
}
