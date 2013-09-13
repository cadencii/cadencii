package com.github.cadencii;

import com.github.cadencii.BEventHandler;

public class WaveViewRealoadRequiredEventHandler extends BEventHandler{
//    public delegate void WaveViewRealoadRequiredEventHandler( object sender, int track, string file, double sec_start, double sec_end );

    public WaveViewRealoadRequiredEventHandler( Object invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, WaveViewRealoadRequiredEventArgs.class );
    }

    public WaveViewRealoadRequiredEventHandler( Class<?> invoker, String method_name ){
        super( invoker, method_name, Void.TYPE, Object.class, WaveViewRealoadRequiredEventArgs.class );
    }
}
