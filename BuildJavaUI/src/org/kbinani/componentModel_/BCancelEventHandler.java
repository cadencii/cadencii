package org.kbinani.componentModel;

import java.lang.reflect.*;
import org.kbinani.*;

public class BCancelEventHandler extends BEventHandler{
    public BCancelEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BCancelEventArgs.class );
    }

    public BCancelEventHandler( Class sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BCancelEventArgs.class );
    }
}
