package org.kbinani.componentModel;

import org.kbinani.*;

public class BRunWorkerCompletedEventHandler extends BEventHandler{
    public BRunWorkerCompletedEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BRunWorkerCompletedEventArgs.class );
    }

    public BRunWorkerCompletedEventHandler( Class sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BRunWorkerCompletedEventArgs.class );
    }
}
