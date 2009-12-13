package org.kbinani.windows.forms;

import org.kbinani.BEventHandler;

public class BPaintEventHandler extends BEventHandler{
    public BPaintEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BPaintEventArgs.class );
    }
    
    public BPaintEventHandler( Class<?> sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BPaintEventArgs.class );
    }
}
