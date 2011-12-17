package com.github.cadencii.windows.forms;

import org.kbinani.BEventHandler;

public class BPreviewKeyDownEventHandler extends BEventHandler{
    public BPreviewKeyDownEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BPreviewKeyDownEventArgs.class );
    }
    
    public BPreviewKeyDownEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BPreviewKeyDownEventArgs.class );
    }
}
