/*
 * VSTiProxy.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.cadencii;

public class vstidrv {
    public static com.boare.cadencii.Event WaveIncoming = new Event();
    public static com.boare.cadencii.Event RenderingFinished = new Event();

    static{
        System.loadLibrary( "vstidrv" );
    }

    public vstidrv( String dll_path, int block_size, int sample_rate ){
        init( dll_path.toCharArray(), block_size, sample_rate );
        setListener( this );
    }

    private void invokeWaveIncoming( double[] left, double[] right ){
        WaveIncoming.invoke( left, right );
    }

    private void invokeRenderingFinished(){
        RenderingFinished.invoke();
    }

    private native void setListener( vstidrv instance );
    private native boolean init( char[] dll_path, int block_size, int sample_rate );
    public native int sendEvent( byte[] src, int[] deltaFrames, int targetTrack );
    public native int startRendering( long total_samples, boolean mode_infinite );
    public native void abortRendering();
    public native double getProgress();
    public native void terminate();
}
