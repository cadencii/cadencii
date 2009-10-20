package com.boare.cadencii;

public class StartRenderArg implements Runnable{
    public String renderer;
    public NrpnData[] nrpn;
    public TempoTableEntry[] tempo;
    public double amplifyLeft;
    public double amplifyRight;
    public int trimMillisec;
    public long totalSamples;
    public String[] files;
    public double waveReadOffsetSeconds;
    public boolean modeInfinite;
    public VstiRenderer renderingContext;

    public void run(){
        s_direct_play = direct_play;
        if ( renderingContext == null ) {
            return;
        }
        if ( !renderingContext.loaded ) {
            return;
        }
        int tempo_count = tempo.length;
        float first_tempo = 125.0f;
        if ( tempo.length > 0 ) {
            first_tempo = (float)(60e6 / (double)tempo[0].tempo);
        }
        int[] masterEventsSrc = new int[tempo_count * 3];
        int[] masterClocksSrc = new int[tempo_count];
        int count = -3;
        for ( int i = 0; i < tempo.length; i++ ) {
            count += 3;
            masterClocksSrc[i] = tempo[i].clock;
            int b0 = tempo[i].tempo >>> 16;
            int u0 = tempo[i].tempo - (b0 << 16));
            int b1 = u0 >>> 8;
            int b2 = u0 - (u0 << 8);
            masterEventsSrc[count] = b0;
            masterEventsSrc[count + 1] = b1;
            masterEventsSrc[count + 2] = b2;
        }
        renderingContext.dllInstance.sendEvent( masterEventsSrc, masterClocksSrc, 0 );

        int numEvents = nrpn.length;
        int[] bodyEventsSrc = new int[numEvents * 3];
        int[] bodyClocksSrc = new int[numEvents];
        count = -3;
        int last_clock = 0;
        for ( int i = 0; i < numEvents; i++ ) {
            count += 3;
            bodyEventsSrc[count] = 0xb0;
            bodyEventsSrc[count + 1] = nrpn[i].getParameter();
            bodyEventsSrc[count + 2] = nrpn[i].value;
            bodyClocksSrc[i] = nrpn[i].getClock();
            last_clock = nrpn[i].getClock();
        }

        int index = tempo_count - 1;
        for ( int i = tempo_count - 1; i >= 0; i-- ) {
            if ( tempo[i].clock < last_clock ) {
                index = i;
                break;
            }
        }
        int last_tempo = tempo[index].tempo;
        int trim_remain = GetErrorSamples( first_tempo ) + (int)(presend_msec / 1000.0 * _SAMPLE_RATE);
        s_trim_remain = trim_remain;

        renderingContext.dllInstance.sendEvent( bodyEventsSrc, bodyClocksSrc, 1 );

        s_rendering = true;
        if ( s_wave != null ) {
            if ( s_trim_remain < 0 ) {
                double[] d = new double[-s_trim_remain];
                for ( int i = 0; i < -s_trim_remain; i++ ) {
                    d[i] = 0.0;
                }
                s_wave.Append( d, d );
                s_trim_remain = 0;
            }
        }

        s_amplify_left = amplify_left;
        s_amplify_right = amplify_right;
        renderingContext.dllInstance.WaveIncoming += vstidrv_WaveIncoming;
        renderingContext.dllInstance.RenderingFinished += vstidrv_RenderingFinished;
        renderingContext.dllInstance.startRendering( total_samples, mode_infinite );
        while ( s_rendering ) {
            Application.DoEvents();
        }
        s_rendering = false;
        renderingContext.dllInstance.WaveIncoming -= vstidrv_WaveIncoming;
        renderingContext.dllInstance.RenderingFinished -= vstidrv_RenderingFinished;
        if ( s_direct_play ) {
            PlaySound.waitForExit();
        }
        s_wave = null;
    }
}
