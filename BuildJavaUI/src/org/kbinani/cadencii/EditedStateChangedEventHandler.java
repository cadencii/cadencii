package org.kbinani.cadencii;

import org.kbinani.BEventHandler;

public class EditedStateChangedEventHandler extends BEventHandler{
    public EditedStateChangedEventHandler( Object invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }
    
    public EditedStateChangedEventHandler( Class<?> invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }
}
