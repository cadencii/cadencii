package org.kbinani.componentModel;

import java.lang.reflect.*;
import org.kbinani.*;

public class BDoWorkEventHandler extends BEventHandler{
    public BDoWorkEventHandler( Object sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BDoWorkEventArgs.class );
    }

    public BDoWorkEventHandler( Class sender, String method_name ){
        super( sender, method_name, Void.TYPE, Object.class, BDoWorkEventArgs.class );
    }
}
