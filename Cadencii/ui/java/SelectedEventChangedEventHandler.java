package org.kbinani.cadencii;

import org.kbinani.BEventHandler;

public class SelectedEventChangedEventHandler extends BEventHandler{
    public SelectedEventChangedEventHandler( Object invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }
    
    public SelectedEventChangedEventHandler( Class<?> invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }
}
