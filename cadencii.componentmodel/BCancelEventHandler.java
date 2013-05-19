package com.github.cadencii.componentmodel;

import com.github.cadencii.BEventHandler;

public class BCancelEventHandler extends BEventHandler{
    public BCancelEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BCancelEventArgs.class );
    }

    public BCancelEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BCancelEventArgs.class );
    }
}
