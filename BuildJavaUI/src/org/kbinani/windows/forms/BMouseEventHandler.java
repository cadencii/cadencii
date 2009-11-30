package org.kbinani.windows.forms;

import org.kbinani.BEventHandler;

public class BMouseEventHandler extends BEventHandler{
    public BMouseEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BMouseEventArgs.class );
    }
    
    public BMouseEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BMouseEventArgs.class );
    }
}
