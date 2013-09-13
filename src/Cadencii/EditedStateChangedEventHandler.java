package com.github.cadencii;

import com.github.cadencii.BEventHandler;

public class EditedStateChangedEventHandler extends BEventHandler{
    public EditedStateChangedEventHandler( Object invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }

    public EditedStateChangedEventHandler( Class<?> invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, Boolean.TYPE );
    }
}
