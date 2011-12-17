package com.github.cadencii.windows.forms;

import org.kbinani.BEventHandler;

public class BKeyPressEventHandler extends BEventHandler{
    public BKeyPressEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BKeyPressEventArgs.class );
    }
    
    public BKeyPressEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BKeyPressEventArgs.class );
    }
}
