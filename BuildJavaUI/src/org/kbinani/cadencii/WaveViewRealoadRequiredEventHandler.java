package org.kbinani.cadencii;

import org.kbinani.BEventHandler;

public class WaveViewRealoadRequiredEventHandler extends BEventHandler{
//    public delegate void WaveViewRealoadRequiredEventHandler( object sender, int track, string file, double sec_start, double sec_end );

    public WaveViewRealoadRequiredEventHandler( Object invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, WaveViewRealoadRequiredEventArgs.class );
    }
    
    public WaveViewRealoadRequiredEventHandler( Class<?> invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, WaveViewRealoadRequiredEventArgs.class );
    }
}
