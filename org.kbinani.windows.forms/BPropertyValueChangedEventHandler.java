package com.github.cadencii.windows.forms;

import org.kbinani.BEventHandler;

public class BPropertyValueChangedEventHandler extends BEventHandler{
    public BPropertyValueChangedEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BPropertyValueChangedEventArgs.class );
    }
    
    public BPropertyValueChangedEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BPropertyValueChangedEventArgs.class );
    }
}
