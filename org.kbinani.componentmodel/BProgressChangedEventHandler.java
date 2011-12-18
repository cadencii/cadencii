package com.github.cadencii.componentmodel;

import com.github.cadencii.*;

public class BProgressChangedEventHandler extends BEventHandler{
    public BProgressChangedEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BProgressChangedEventArgs.class );
    }

    public BProgressChangedEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BProgressChangedEventArgs.class );
    }
}
