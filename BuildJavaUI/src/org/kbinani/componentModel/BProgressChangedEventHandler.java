package org.kbinani.componentModel;

import org.kbinani.*;

public class BProgressChangedEventHandler extends BEventHandler{
    public BProgressChangedEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BProgressChangedEventArgs.class );
    }

    public BProgressChangedEventHandler( Class sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BProgressChangedEventArgs.class );
    }
}
