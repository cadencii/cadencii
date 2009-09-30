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
//#define TEST
package com.boare.cadencii;

import com.boare.vsq.*;
/*public delegate void WaveIncomingEventHandler( double[] L, double[] R );
public delegate void RenderingFinishedEventHandler();

public static class VSTiProxyTrackType {
    public const int TempoTrack = 0;
    public const int MainTrack = 1;
}*/

public class VSTiProxy {
    public static final String RENDERER_DSB2 = "DSB2";
    public static final String RENDERER_DSB3 = "DSB3";
    public static final String RENDERER_UTU0 = "UTU0";
    private static final int _SAMPLE_RATE = 44100;
    private static final int _BLOCK_SIZE = 44100;

    public static String CurrentUser = "";
    private static WaveWriter s_wave;
    private static boolean s_rendering = false;
    private static int s_trim_remain = 0;
    private static Object s_locker;
    private static double s_amplify_left = 1.0;
    private static double s_amplify_right = 1.0;
    private static String s_working_renderer = "";
    private static Vector<VstiRenderer> m_vstidrv = new Vector<VstiRenderer>();
    private static boolean s_direct_play;
    private static Vector<WaveReader> s_reader = new Vector<WaveReader>();
    private static long s_total_append = 0;
    private static double s_wave_read_offset_seconds = 0.0;

    private class StartUtauRenderArg {
        public VsqFileEx vsq;
        public int track;
        public Vector<SingerConfig> utau_singers;
        public String path_resampler;
        public String path_wavtool;
        public String path_temp;
        public boolean invoke_with_wine;
        public int sample_rate;
        public int trim_msec;
        public boolean mode_infinite;
    }

    private class VstiRenderer {
        public vstidrv dllInstance = null;
        public boolean loaded = false;
        public String path = "";
        public String name = "";
    }

    public static void init() {
        /*Thread init = new Thread( new ThreadStart( InitCor ) );
        init.Priority = ThreadPriority.AboveNormal;
        init.Start();*/
        initCor();
    }

    public static void initCor() {
        //AppManager.LoadConfig();
        s_locker = new Object();
        PlaySound.init( _SAMPLE_RATE );
        try {
            String vocalo2_dll_path = VocaloSysUtil.getDllPathVsti2();
            String vocalo1_dll_path = VocaloSysUtil.getDllPathVsti1();
            if ( vocalo2_dll_path != "" && (new File( vocalo2_dll_path )).exists() ) {
                VstiRenderer vr = new VstiRenderer();
                vr.path = vocalo2_dll_path;
                vr.loaded = false;
                vr.dllInstance = new vstidrv();
                vr.name = RENDERER_DSB3;
                m_vstidrv.Add( vr );
            }
            if ( vocalo1_dll_path != "" && (new File( vocalo1_dll_path )).exists() ) {
                VstiRenderer vr = new VstiRenderer();
                vr.path = vocalo1_dll_path;
                vr.loaded = false;
                vr.dllInstance = new vstidrv();
                vr.name = RENDERER_DSB2;
                m_vstidrv.Add( vr );
            }
        } catch ( Exception ex ){
            debugWriteLine( "    ex=" + ex );
        }

        for ( int i = 0; i < m_vstidrv.size(); i++ ) {
            String dll_path = m_vstidrv.get( i ).path;
            boolean loaded = false;
            try {
                char[] str = dll_path.toCharArray();
                if ( dll_path != "" ) {
                    loaded = m_vstidrv.get( i ).dllInstance.init( str, _BLOCK_SIZE, _SAMPLE_RATE );
                } else {
                    loaded = false;
                }
                m_vstidrv[i].loaded = loaded;
            } catch ( Exception ex ) {

            }
        }
    }

    public static boolean isRendererAvailable( String renderer ) {
        for ( int i = 0; i < m_vstidrv.size(); i++ ) {
            if ( renderer.startsWith( m_vstidrv.get( i ).name ) && m_vstidrv.get( i ).loaded ) {
                return true;
            }
        }
        if ( renderer.startsWith( RENDERER_UTU0 ) ) {
            if ( AppManager.editorConfig.PathResampler != "" && (new File( AppManager.EditorConfig.PathResampler )).exists() ) &&
                 AppManager.editorConfig.PathWavtool != "" && (new File( AppManager.EditorConfig.PathWavtool )).exists() ) {
                return true;
            }
        }
        return false;
    }

    public static int getSampleRate(){
        return _SAMPLE_RATE;
    }

    public static void terminate() {
        for ( int i = 0; i < m_vstidrv.size(); i++ ) {
            if ( m_vstidrv.get( i ).dllInstance != null ) {
                m_vstidrv.get( i ).dllInstance.terminate();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vsq">レンダリング元</param>
    /// <param name="track">レンダリング対象のトラック番号</param>
    /// <param name="file">レンダリング結果を出力するファイル名。空文字ならファイルには出力されない</param>
    /// <param name="start_sec">レンダリングの開始位置</param>
    /// <param name="end_sec">レンダリングの終了位置</param>
    /// <param name="amplify_left">左チャンネルの増幅率</param>
    /// <param name="amplify_right">右チャンネルの増幅率</param>
    /// <param name="ms_presend">プリセンドするミリセカンド</param>
    /// <param name="direct_play">レンダリングと同時に再生するかどうか</param>
    /// <param name="files">レンダリング結果と同時に再生するWAVEファイルのリスト</param>
    /// <param name="wave_read_offset_seconds">filesに指定したファイルの先頭から読み飛ばす秒数</param>
    public static void Render(
        VsqFileEx vsq,
        int track,
        WaveWriter wave_writer,
        double start_sec,
        double end_sec,
        double amplify_left,
        double amplify_right,
        int ms_presend,
        boolean direct_play,
        String[] files,
        double wave_read_offset_seconds,
        boolean mode_infinite,
        String temp_dir
    ) {
        s_working_renderer = VSTiProxy.RENDERER_DSB3;
        s_direct_play = direct_play;
        if ( direct_play ) {
            PlaySound.reset();
        }
        s_wave = wave_writer;
        s_amplify_left = amplify_left;
        s_amplify_right = amplify_right;
        for ( int i = 0; i < s_reader.size(); i++ ) {
            s_reader.get( i ).close();
        }
        s_reader.clear();
        for ( int i = 0; i < files.length; i++ ) {
            s_reader.add( new WaveReader( files[i] ) );
        }
        s_total_append = 0;
        s_wave_read_offset_seconds = wave_read_offset_seconds;

        String version = vsq.tracks.get( track ).getCommon().version;
        if ( version.startsWith( VSTiProxy.RENDERER_DSB2 ) ) {
            s_working_renderer = VSTiProxy.RENDERER_DSB2;
        } else if ( version.startsWith( VSTiProxy.RENDERER_UTU0 ) ) {
            s_working_renderer = VSTiProxy.RENDERER_UTU0;
        }
        VsqFileEx split = (VsqFileEx)vsq.clone();
        split.updateTotalClocks();
        int clock_start = (int)vsq.getClockFromSec( start_sec );
        int clock_end = (int)vsq.getClockFromSec( end_sec );

        if ( clock_end < vsq.totalClocks ) {
            split.removePart( clock_end, split.totalClocks + 480 );
        }

        int extra_note_clock = (int)vsq.getClockFromSec( (float)end_sec + 10.0f );
        int extra_note_clock_end = (int)vsq.getClockFromSec( (float)end_sec + 10.0f + 3.1f ); //ブロックサイズが1秒分で、バッファの個数が3だから +3.1f。0.1fは安全のため。
        VsqEvent extra_note = new VsqEvent( extra_note_clock, new VsqID( 0 ) );
        extra_note.id.type = VsqIDType.Anote;
        extra_note.id.note = 60;
        extra_note.id.length = extra_note_clock_end - extra_note_clock;
        extra_note.id.vibratoHandle = null;
        extra_note.id.lyricHandle = new LyricHandle( "a", "a" );
        split.tracks.get( track ).addEvent( extra_note );

        double trim_sec = 0.0; // レンダリング結果から省かなければならない秒数。
        if ( clock_start < split.getPreMeasureClocks() ) {
            trim_sec = split.getSecFromClock( clock_start );
        } else {
            split.removePart( vsq.getPreMeasureClocks(), clock_start );
            trim_sec = split.getSecFromClock( split.getPreMeasureClocks() );
        }
        split.updateTotalClocks();
        split.reflectPitch();
        long total_samples = (long)((end_sec - start_sec) * _SAMPLE_RATE);
        int trim_msec = (int)(trim_sec * 1000.0);
        if ( direct_play ) {
            if ( s_working_renderer == VSTiProxy.RENDERER_UTU0 ) {
                StartUtauRenderArg sura = new StartUtauRenderArg();
                sura.vsq = split;
                sura.track = track;
                sura.utau_singers = AppManager.editorConfig.UtauSingers;
                sura.path_resampler = AppManager.editorConfig.PathResampler;
                sura.path_wavtool = AppManager.editorConfig.PathWavtool;
                sura.path_temp = temp_dir;
                sura.invoke_with_wine = AppManager.editorConfig.InvokeUtauCoreWithWine;
                sura.sample_rate = _SAMPLE_RATE;
                sura.trim_msec = trim_msec;
                sura.mode_infinite = mode_infinite;

                Thread thread = new Thread( new ParameterizedThreadStart( RenderUtauWithDirectPlay ) );
                thread.Priority = ThreadPriority.BelowNormal;
                thread.Start( sura );
            } else {
                VsqNrpn[] nrpn = VsqFile.generateNRPN( split, track, ms_presend );
                NrpnData[] nrpn_data = VsqNrpn.convert( nrpn );
                StartRenderArg sra = new StartRenderArg();
                sra.renderer = s_working_renderer;
                sra.nrpn = nrpn_data;
                sra.tempo = split.TempoTable.ToArray();
                sra.amplify_left = amplify_left;
                sra.amplify_right = amplify_right;
                sra.trim_msec = trim_msec;
                sra.total_samples = total_samples;
                sra.files = files;
                sra.wave_read_offset_seconds = wave_read_offset_seconds;
                sra.mode_infinite = mode_infinite;

                Thread thread = new Thread( new ParameterizedThreadStart( RenderWithDirectPlay ) );
                thread.Priority = ThreadPriority.BelowNormal;
                thread.Start( sra );
            }
        } else {
            if ( s_working_renderer == VSTiProxy.RENDERER_UTU0 ) {
                RenderUtau.WaveIncoming += vstidrv_WaveIncoming;
                RenderUtau.RenderingFinished += vstidrv_RenderingFinished;
                s_trim_remain = 0;
                RenderUtau.StartRendering( split,
                                           track,
                                           AppManager.EditorConfig.UtauSingers,
                                           AppManager.EditorConfig.PathResampler,
                                           AppManager.EditorConfig.PathWavtool,
                                           temp_dir,
                                           AppManager.EditorConfig.InvokeUtauCoreWithWine,
                                           _SAMPLE_RATE,
                                           trim_msec,
                                           mode_infinite );
                RenderUtau.WaveIncoming -= vstidrv_WaveIncoming;
                RenderUtau.RenderingFinished -= vstidrv_RenderingFinished;
            } else {
                VsqNrpn[] nrpn = VsqFile.generateNRPN( split, track, ms_presend );
                NrpnData[] nrpn_data = VsqNrpn.convert( nrpn );
                RenderCor( s_working_renderer,
                           nrpn_data,
                           split.TempoTable.ToArray(),
                           amplify_left,
                           amplify_right,
                           direct_play,
                           trim_msec,
                           total_samples,
                           files,
                           wave_read_offset_seconds,
                           mode_infinite );
            }
        }
    }

    private static void RenderWithDirectPlay( object argument ) {
        StartRenderArg sra = (StartRenderArg)argument;
        RenderCor( sra.renderer,
                   sra.nrpn,
                   sra.tempo,
                   sra.amplify_left,
                   sra.amplify_right,
                   true,
                   sra.trim_msec,
                   sra.total_samples,
                   sra.files,
                   sra.wave_read_offset_seconds,
                   sra.mode_infinite );
    }

    private static void RenderUtauWithDirectPlay( object argument ) {
        StartUtauRenderArg sura = (StartUtauRenderArg)argument;
        RenderUtau.WaveIncoming += vstidrv_WaveIncoming;
        RenderUtau.RenderingFinished += vstidrv_RenderingFinished;
        RenderUtau.StartRendering( sura.vsq,
                                   sura.track,
                                   sura.utau_singers,
                                   sura.path_resampler,
                                   sura.path_wavtool,
                                   sura.path_temp,
                                   sura.invoke_with_wine,
                                   sura.sample_rate,
                                   sura.trim_msec,
                                   sura.mode_infinite );
        RenderUtau.WaveIncoming -= vstidrv_WaveIncoming;
        RenderUtau.RenderingFinished -= vstidrv_RenderingFinished;
    }

    private static unsafe void RenderCor(
        String renderer,
        NrpnData[] nrpn,
        TempoTableEntry[] tempo,
        double amplify_left,
        double amplify_right,
        boolean direct_play,
        int presend_msec,
        long total_samples,
        String[] files,
        double wave_read_offset_seconds,
        boolean mode_infinite
    ) {
    }

    static void vstidrv_RenderingFinished() {
        s_rendering = false;
        /*if ( RenderingFinished != null ) {
            RenderingFinished();
        }*/
    }

    static unsafe void vstidrv_WaveIncoming( double[] L, double[] R ) {
#if TEST
        bocoree.debug.push_log( "VSTiProxy.vstidrv_WaveIncoming" );
        bocoree.debug.push_log( "    requiring lock of s_locker..." );
#endif
        if ( !s_rendering ) {
            return;
        }
        lock ( s_locker ) {
            if ( s_trim_remain > 0 ) {
                if ( s_trim_remain >= L.Length ) {
                    s_trim_remain -= L.Length;
                    return;
                }
                int actual_append = L.Length - s_trim_remain;
                double[] dL = new double[actual_append];
                double[] dR = new double[actual_append];
                for ( int i = 0; i < actual_append; i++ ) {
                    dL[i] = L[i + s_trim_remain] * s_amplify_left;
                    dR[i] = R[i + s_trim_remain] * s_amplify_right;
                }
                if ( s_wave != null ) {
                    s_wave.Append( dL, dR );
                }
                long start = s_total_append + (long)(s_wave_read_offset_seconds * _SAMPLE_RATE);
                for ( int i = 0; i < s_reader.Count; i++ ) {
                    double[] reader_r;
                    double[] reader_l;
                    s_reader[i].Read( start, actual_append, out reader_l, out reader_r );
                    for ( int j = 0; j < actual_append; j++ ) {
                        dL[j] += reader_l[j];
                        dR[j] += reader_r[j];
                    }
                    reader_l = null;
                    reader_r = null;
                }
                if ( s_direct_play ) {
                    PlaySound.Append( dL, dR, actual_append );
                }
                dL = null;
                dR = null;
                s_trim_remain = 0;
                s_total_append += actual_append;
            } else {
                int length = L.Length;
                for ( int i = 0; i < length; i++ ) {
                    L[i] = L[i] * s_amplify_left;
                    R[i] = R[i] * s_amplify_right;
                }
                if ( s_wave != null ) {
                    s_wave.Append( L, R );
                }
                long start = s_total_append + (long)(s_wave_read_offset_seconds * _SAMPLE_RATE);
                for ( int i = 0; i < s_reader.Count; i++ ) {
                    double[] reader_r;
                    double[] reader_l;
                    s_reader[i].Read( start, length, out reader_l, out reader_r );
                    for ( int j = 0; j < length; j++ ) {
                        L[j] += reader_l[j];
                        R[j] += reader_r[j];
                    }
                    reader_l = null;
                    reader_r = null;
                }
                if ( s_direct_play ) {
                    PlaySound.Append( L, R, L.Length );
                }
                s_total_append += length;
            }
        }
#if DEBUG
        bocoree.debug.push_log( "...done(vstidrv_WaveIncoming)" );
#endif
    }

    public static double GetProgress() {
        if ( s_working_renderer == VSTiProxy.RENDERER_UTU0 ) {
            return RenderUtau.GetProgress();
        } else {
            for ( int i = 0; i < m_vstidrv.Count; i++ ) {
                if ( m_vstidrv[i].Name == s_working_renderer ) {
                    return m_vstidrv[i].DllInstance.GetProgress();
                }
            }
            return 0.0;
        }
    }

    public static void AbortRendering() {
        if ( s_rendering ) {
            s_rendering = false;
        }
        if ( s_working_renderer == VSTiProxy.RENDERER_UTU0 ) {
            RenderUtau.AbortRendering();
        } else {
            for ( int i = 0; i < m_vstidrv.Count; i++ ) {
                if ( m_vstidrv[i].Loaded ) {
                    m_vstidrv[i].DllInstance.AbortRendering();
                }
            }
        }
        for ( int i = 0; i < s_reader.Count; i++ ) {
            s_reader[i].Close();
            s_reader[i] = null;
        }
        s_reader.Clear();
    }

    public static int GetErrorSamples( float tempo ) {
        const float a0 = -17317.563f;
        const float a1 = 86.7312112f;
        const float a2 = -0.237323499f;
        if ( tempo <= 240 ) {
            return 4666;
        } else {
            float x = tempo - 240;
            return (int)((a2 * x + a1) * x + a0);
        }
    }

    public static float GetPlayTime() {
        double pos = PlaySound.GetPosition();
#if DEBUG
        //AppManager.DebugWriteLine( "VSTiProxy.GetPlayTime; pos=" + pos );
#endif
        return (float)pos;
    }

#if OBSOLUTE
    public delegate void WaveIncomingEventHandler( double[] L, double[] R );
    public delegate void FirstBufferWrittenEventHandler();
    public delegate void RenderingFinishedEventHandler();

    public static unsafe class vstidrv {
        private delegate void __WaveIncomingCallback( double* L, double* R, int length );
        private delegate void __FirstBufferWrittenCallback();
        private delegate void __RenderingFinishedCallback();

        private volatile static __WaveIncomingCallback s_wave_incoming_callback;
        private volatile static __FirstBufferWrittenCallback s_first_buffer_written_callback;
        private volatile static __RenderingFinishedCallback s_rendering_finished_callback;

        private volatile static FirstBufferWrittenEventHandler s_first_buffer_written_callback_body;
        private volatile static WaveIncomingEventHandler s_wave_incoming_callback_body;

        //public static event WaveIncomingEventHandler WaveIncoming;
        public static event RenderingFinishedEventHandler RenderingFinished;

        static vstidrv() {
            s_wave_incoming_callback = new __WaveIncomingCallback( HandleWaveIncomingCallback );
            s_first_buffer_written_callback = new __FirstBufferWrittenCallback( HandleFirstBufferWrittenCallback );
            s_rendering_finished_callback = new __RenderingFinishedCallback( HandleRenderingFinishedCallback );
            try {
                vstidrv_setFirstBufferWrittenCallback( Marshal.GetFunctionPointerForDelegate( s_first_buffer_written_callback ) );
                vstidrv_setWaveIncomingCallback( Marshal.GetFunctionPointerForDelegate( s_wave_incoming_callback ) );
                vstidrv_setRenderingFinishedCallback( Marshal.GetFunctionPointerForDelegate( s_rendering_finished_callback ) );
            } catch ( Exception ex ) {
#if DEBUG
                AppManager.DebugWriteLine( "vstidrv.cctor()" );
                AppManager.DebugWriteLine( "    ex=" + ex );
#endif
            }
        }

        public static void SetWaveIncomingCallback( WaveIncomingEventHandler handler ) {
            s_wave_incoming_callback_body = handler;
        }

        public static void SetFirstBufferWrittenCallback( FirstBufferWrittenEventHandler handler ) {
            s_first_buffer_written_callback_body = handler;
        }

        public static void Terminate() {
            try {
                vstidrv_Terminate();
            } catch {
            }
        }

        private static void HandleWaveIncomingCallback( double* L, double* R, int length ) {
#if TEST
            bocoree.debug.push_log( "vstidrv.HandleWaveIncomingCallback" );
            bocoree.debug.push_log( "    length=" + length );
#endif
            if ( s_wave_incoming_callback_body != null ) {
                double[] ret_l = new double[length];
                double[] ret_r = new double[length];
                for ( int i = 0; i < length; i++ ) {
                    ret_l[i] = L[i];
                    ret_r[i] = R[i];
                }
                s_wave_incoming_callback_body( ret_l, ret_r );
            }
        }

        private static void HandleFirstBufferWrittenCallback() {
#if DEBUG
            AppManager.DebugWriteLine( "vstidrv+HandleFirstBufferWrittenCallback" );
#endif
            if ( s_first_buffer_written_callback_body != null ) {
                s_first_buffer_written_callback_body();
            }
        }

        private static void HandleRenderingFinishedCallback() {
            if ( RenderingFinished != null ) {
                RenderingFinished();
            }
        }

        public static boolean Init( char[] dll_path, int block_size, int sample_rate ) {
#if DEBUG
            AppManager.DebugWriteLine( "VSTIProxy.Init" );
#endif
            boolean ret = false;
            try {
                byte[] b_dll_path = new byte[dll_path.Length + 1];
                for ( int i = 0; i < dll_path.Length; i++ ) {
                    b_dll_path[i] = (byte)dll_path[i];
                }
                b_dll_path[dll_path.Length] = 0x0;
                fixed ( byte* ptr = &b_dll_path[0] ) {
                    ret = vstidrv_Init( ptr, block_size, sample_rate );
                }
            } catch( Exception ex) {
#if DEBUG
                AppManager.DebugWriteLine( "    ex=" + ex );
#endif
                ret = false;
            }
            return ret;
        }

        public static int SendEvent( byte* src, int* deltaFrames, int numEvents, int targetTrack ) {
            int ret = 0;
            try {
                ret = vstidrv_SendEvent( src, deltaFrames, numEvents, targetTrack );
            } catch {
                ret = -1;
            }
            return ret;
        }

        public static int StartRendering(
            long total_samples,
            double amplify_left,
            double amplify_right,
            int error_samples,
            boolean event_enabled,
            boolean direct_play_enabled,
            String[] files,
            double wave_read_offset_seconds,
            boolean mode_infinite
        ) {
            int ret = 0;
            try {
                IntPtr intptr_files = Marshal.AllocHGlobal( sizeof( char* ) * files.Length );
                char** ptr_files = (char**)intptr_files.ToPointer();
                IntPtr[] cont_intptr_files = new IntPtr[files.Length];
                for ( int i = 0; i < files.Length; i++ ) {
                    cont_intptr_files[i] = Marshal.AllocHGlobal( sizeof( char ) * (files.Length + 1) );
                    ptr_files[i] = (char*)cont_intptr_files[i].ToPointer();
                    for ( int j = 0; j < files[i].Length; j++ ) {
                        ptr_files[i][j] = files[i][j];
                    }
                    ptr_files[i][files[i].Length] = '\0';
                }
                ret = vstidrv_StartRendering( total_samples,
                                              amplify_left,
                                              amplify_right,
                                              error_samples,
                                              event_enabled,
                                              direct_play_enabled,
                                              ptr_files,
                                              files.Length,
                                              wave_read_offset_seconds,
                                              mode_infinite );
                for ( int i = 0; i < files.Length; i++ ) {
                    Marshal.FreeHGlobal( cont_intptr_files[i] );
                }
                Marshal.FreeHGlobal( intptr_files );
            } catch {
                ret = -1;
            }
            return ret;
        }

        public static void AbortRendering() {
            try {
                vstidrv_AbortRendering();
            } catch {
            }
        }

        public static double GetProgress() {
            double ret = 0.0;
            try {
                ret = vstidrv_GetProgress();
            } catch {
            }
            return ret;
        }

        public static float GetPlayTime() {
            float ret = -1.0f;
#if DEBUG
            //AppManager.DebugWriteLine( "vstidrv+GetPlayTime" );
#endif
            try {
                ret = vstidrv_GetPlayTime();
            } catch ( Exception ex ) {
#if DEBUG
                AppManager.DebugWriteLine( "    ex=" + ex );
#endif
                ret = -1.0f;
            }
#if DEBUG
            AppManager.DebugWriteLine( "    ret=" + ret );
#endif
            return ret;
        }

        public static void WaveOutReset() {
#if DEBUG
            AppManager.DebugWriteLine( "WaveOutReset" );
#endif
            try {
                vstidrv_WaveOutReset();
            } catch( Exception ex ) {
#if DEBUG
                AppManager.DebugWriteLine( "    ex=" + ex );
#endif
            }
        }

        public static int JoyInit() {
#if DEBUG
            AppManager.DebugWriteLine( "JoyInit" );
#endif
            int ret = 0;
            try {
                ret = vstidrv_JoyInit();
            } catch ( Exception ex ) {
#if DEBUG
                AppManager.DebugWriteLine( "    ex=" + ex );
#endif
                ret = 0;
            }
            return ret;
        }

        public static boolean JoyGetStatus( int index, out byte[] buttons, out int pov ) {
            boolean ret = false;
            pov = -1;
            try {
                int len = vstidrv_JoyGetNumButtons( index );
                buttons = new byte[len];
                IntPtr iptr = Marshal.AllocHGlobal( sizeof( byte ) * len );
                byte* ptr_buttons = (byte*)iptr.ToPointer();
                for ( int i = 0; i < len; i++ ) {
                    ptr_buttons[i] = 0x0;
                }
                int src_pov;
                ret = vstidrv_JoyGetStatus( index, ptr_buttons, len, &src_pov );
                for ( int i = 0; i < buttons.Length; i++ ) {
                    buttons[i] = ptr_buttons[i];
                }
                Marshal.FreeHGlobal( iptr );
                pov = src_pov;
            } catch ( Exception ex ){
                buttons = new byte[0];
                pov = -1;
                ret = false;
            }
            return ret;
        }

        public static int JoyGetNumJoyDev() {
            int ret = 0;
            try {
                ret = vstidrv_JoyGetNumJoyDev();
            } catch ( Exception ex ){
                ret = 0;
            }
            return ret;
        }

        //void vstidrv_setFirstBufferWrittenCallback( FirstBufferWrittenCallback proc );
        [DllImport( "vstidrv.dll" )]
        private static extern void vstidrv_setFirstBufferWrittenCallback( IntPtr proc );

        //void vstidrv_setWaveIncomingCallback( WaveIncomingCallback proc );
        [DllImport( "vstidrv.dll" )]
        private static extern void vstidrv_setWaveIncomingCallback( IntPtr proc );

        //void vstidrv_setRenderingFinishedCallback( RenderingFinishedCallback proc );
        [DllImport( "vstidrv.dll" )]
        private static extern void vstidrv_setRenderingFinishedCallback( IntPtr proc );

        //boolean vstidrv_Init( String dll_path, int block_size, int sample_rate );
        [DllImport( "vstidrv.dll" )]
        private static extern boolean vstidrv_Init( byte* dll_path, int block_size, int sample_rate );

        //int  vstidrv_SendEvent( unsigned char *src, int *deltaFrames, int numEvents, int targetTrack );
        [DllImport( "vstidrv.dll" )]
        private static extern int vstidrv_SendEvent( byte* src, int* deltaFrames, int numEvents, int targetTrack );

        //int vstidrv_StartRendering( __int64 total_samples, double amplify_left, double amplify_right, int error_samples, boolean event_enabled, boolean direct_play_enabled, wchar_t** files, int num_files, double wave_read_offset_seconds );
        [DllImport( "vstidrv.dll" )]
        private static extern int vstidrv_StartRendering( long total_samples,
            double amplify_left,
            double amplify_right,
            int error_samples,
            boolean event_enabled,
            boolean direct_play_enabled,
            char** files,
            int num_files,
            double wave_read_offset_seconds,
            boolean mode_infinite );


        //void vstidrv_AbortRendering();
        [DllImport( "vstidrv.dll" )]
        private static extern void vstidrv_AbortRendering();

        //double vstidrv_GetProgress();        
        [DllImport( "vstidrv.dll" )]
        private static extern double vstidrv_GetProgress();

        //float vstidrv_GetPlayTime();
        [DllImport( "vstidrv.dll" )]
        private static extern float vstidrv_GetPlayTime();

        //void vstidrv_WaveOutReset();
        [DllImport( "vstidrv.dll" )]
        private static extern void vstidrv_WaveOutReset();

        //void vstidrv_Terminate();
        [DllImport( "vstidrv.dll" )]
        private static extern void vstidrv_Terminate();

        //int vstidrv_JoyInit();
        [DllImport( "vstidrv.dll" )]
        private static extern int vstidrv_JoyInit();

        //boolean vstidrv_JoyIsJoyAttatched( int index );
        [DllImport( "vstidrv.dll" )]
        private static extern boolean vstidrv_JoyIsJoyAttatched( int index );

        //boolean vstidrv_JoyGetStatus( int index, unsigned char *buttons, int *pov );
        [DllImport( "vstidrv.dll" )]
        private static extern boolean vstidrv_JoyGetStatus( int index, byte* buttons, int len, int* pov );

        //int vstidrv_JoyGetNumButtons( int index );
        [DllImport( "vstidrv.dll" )]
        private static extern int vstidrv_JoyGetNumButtons( int index );

        //void vstidrv_JoyReset();
        [DllImport( "vstidrv.dll" )]
        private static extern void vstidrv_JoyReset();

        //int vstidrv_JoyGetNumJoyDev();
        [DllImport( "vstidrv.dll" )]
        private static extern int vstidrv_JoyGetNumJoyDev();

    }
#endif
}
