/*
 * VSTiProxy.cs
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import java.io.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.media.*;
import org.kbinani.vsq.*;
#else
//#define TEST
using System;
using System.Threading;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    public class VSTiProxy {
        public const String RENDERER_DSB2 = "DSB2";
        public const String RENDERER_DSB3 = "DSB3";
        public const String RENDERER_UTU0 = "UTU0";
        public const String RENDERER_STR0 = "STR0";
        public const String RENDERER_AQT0 = "AQT0";
        public const int SAMPLE_RATE = 44100;
        public const int BLOCK_SIZE = 4410;
        const float a0 = -17317.563f;
        const float a1 = 86.7312112f;
        const float a2 = -0.237323499f;

        public static String CurrentUser = "";
        private static String s_working_renderer = "";
#if ENABLE_VOCALOID
        public static Vector<VocaloidDriver> vocaloidDriver = new Vector<VocaloidDriver>();
#endif
#if ENABLE_AQUESTONE
        public static AquesToneDriver aquesToneDriver = null;
#endif

        private static RenderingRunner s_rendering_context;

        public static void init() {
            initCor();
        }

        public static void initCor() {
            PlaySound.init( SAMPLE_RATE, SAMPLE_RATE, 3 );

#if ENABLE_VOCALOID
#if !DEBUG
            try {
#endif
                String vocalo2_dll_path = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 );
                String vocalo1_dll_path = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID1 );
                if ( vocalo2_dll_path != "" && PortUtil.isFileExists( vocalo2_dll_path ) ) {
                    VocaloidDriver vr = new VocaloidDriver();
                    vr.path = vocalo2_dll_path;
                    vr.loaded = false;
                    //vr.dllInstance = new VocaloidDriver();
                    vr.name = RENDERER_DSB3;
                    vocaloidDriver.add( vr );
                }
                if ( vocalo1_dll_path != "" && PortUtil.isFileExists( vocalo1_dll_path ) ) {
                    VocaloidDriver vr = new VocaloidDriver();
                    vr.path = vocalo1_dll_path;
                    vr.loaded = false;
                    //vr.dllInstance = new VocaloidDriver();
                    vr.name = RENDERER_DSB2;
                    vocaloidDriver.add( vr );
                }
#if !DEBUG
            } catch ( Exception ex ){
                AppManager.debugWriteLine( "    ex=" + ex );
                org.kbinani.debug.push_log( "    ex=" + ex );
            }
#endif
            for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                String dll_path = vocaloidDriver.get( i ).path;
                boolean loaded = false;
                try {
                    if ( dll_path != "" ) {
                        loaded = vocaloidDriver.get( i ).open( dll_path, BLOCK_SIZE, SAMPLE_RATE );
                    } else {
                        loaded = false;
                    }
                    vocaloidDriver.get( i ).loaded = loaded;
                } catch ( Exception ex ) {
                }
            }
#endif

#if ENABLE_AQUESTONE
            reloadAquesTone();
#endif
        }

#if ENABLE_AQUESTONE
        public static void reloadAquesTone() {
            String aques_tone = AppManager.editorConfig.PathAquesTone;
            if ( aquesToneDriver == null ) {
                aquesToneDriver = new AquesToneDriver();
                aquesToneDriver.loaded = false;
                aquesToneDriver.name = RENDERER_AQT0;
            }
            if ( aquesToneDriver.loaded ) {
                aquesToneDriver.close();
                aquesToneDriver.loaded = false;
            }
            aquesToneDriver.path = aques_tone;
            if ( !aques_tone.Equals( "" ) && PortUtil.isFileExists( aques_tone ) ) {
                boolean loaded = false;
                try {
                    loaded = aquesToneDriver.open( aques_tone, BLOCK_SIZE, SAMPLE_RATE );
                } catch ( Exception ex ) {
                    loaded = false;
                }
                aquesToneDriver.loaded = loaded;
            }
#if DEBUG
            PortUtil.println( "VSTiProxy#initCor; aquesToneDriver.loaded=" + aquesToneDriver.loaded );
#endif
        }
#endif

        public static boolean isRendererAvailable( String renderer ) {
#if ENABLE_VOCALOID
            for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                if ( renderer.StartsWith( vocaloidDriver.get( i ).name ) && vocaloidDriver.get( i ).loaded ) {
                    return true;
                }
            }
#endif

#if ENABLE_AQUESTONE
            if ( renderer.StartsWith( RENDERER_AQT0 ) && aquesToneDriver != null && aquesToneDriver.loaded ) {
                return true;
            }
#endif

            if ( renderer.StartsWith( RENDERER_UTU0 ) ) {
                if ( !AppManager.editorConfig.PathResampler.Equals( "" ) && PortUtil.isFileExists( AppManager.editorConfig.PathResampler ) &&
                     !AppManager.editorConfig.PathWavtool.Equals( "" ) && PortUtil.isFileExists( AppManager.editorConfig.PathWavtool ) ) {
                    if ( AppManager.editorConfig.UtauSingers.size() > 0 ) {
                        return true;
                    }
                }
            }
            if ( renderer.StartsWith( RENDERER_STR0 ) ) {
                if ( PortUtil.isFileExists( PortUtil.combinePath( PortUtil.getApplicationStartupPath(), StraightRenderingRunner.STRAIGHT_SYNTH ) ) ) {
                    int count = AppManager.editorConfig.UtauSingers.size();
                    for ( int i = 0; i < count; i++ ) {
                        String analyzed = PortUtil.combinePath( AppManager.editorConfig.UtauSingers.get( i ).VOICEIDSTR, "analyzed" );
                        if ( PortUtil.isDirectoryExists( analyzed ) ) {
                            String analyzed_oto_ini = PortUtil.combinePath( analyzed, "oto.ini" );
                            if ( PortUtil.isFileExists( analyzed_oto_ini ) ) {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static void terminate() {
#if ENABLE_VOCALOID
            for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                if ( vocaloidDriver.get( i ) != null ) {
                    vocaloidDriver.get( i ).close();
                }
            }
#endif

#if ENABLE_AQUESTONE
            if ( aquesToneDriver != null ) {
                try {
                    aquesToneDriver.close();
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "VSTiProxy#terminate; ex=" + ex );
                }
            }
#endif
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
        public static void render(
            VsqFileEx vsq,
            int track,
            WaveWriter wave_writer,
            double start_sec,
            double end_sec,
            int ms_presend,
            boolean direct_play,
            WaveReader[] files,
            double wave_read_offset_seconds,
            boolean mode_infinite,
            String temp_dir,
            boolean reflect_amp_to_wave
        ) {
            s_working_renderer = VSTiProxy.RENDERER_DSB3;
            if ( direct_play ) {
                PlaySound.reset();
            }
            Vector<WaveReader> reader = new Vector<WaveReader>();
            for ( int i = 0; i < files.Length; i++ ) {
                reader.add( files[i] );
            }

            String version = vsq.Track.get( track ).getCommon().Version;
            if ( version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                s_working_renderer = VSTiProxy.RENDERER_DSB2;
            } else if ( version.StartsWith( VSTiProxy.RENDERER_UTU0 ) ) {
                s_working_renderer = VSTiProxy.RENDERER_UTU0;
            } else if ( version.StartsWith( VSTiProxy.RENDERER_STR0 ) ) {
                s_working_renderer = VSTiProxy.RENDERER_STR0;
            } else if ( version.StartsWith( VSTiProxy.RENDERER_AQT0 ) ) {
                s_working_renderer = VSTiProxy.RENDERER_AQT0;
            }
#if DEBUG
            org.kbinani.debug.push_log( "s_working_renderer=" + s_working_renderer );
#endif
            VsqFileEx split = (VsqFileEx)vsq.clone();
            split.updateTotalClocks();
            int clock_start = (int)vsq.getClockFromSec( start_sec );
            int clock_end = (int)vsq.getClockFromSec( end_sec );

            if ( clock_end < vsq.TotalClocks ) {
                split.removePart( clock_end, split.TotalClocks + 480 );
            }

            int extra_note_clock = (int)vsq.getClockFromSec( (float)end_sec + 10.0f );
            int extra_note_clock_end = (int)vsq.getClockFromSec( (float)end_sec + 10.0f + 3.1f ); //ブロックサイズが1秒分で、バッファの個数が3だから +3.1f。0.1fは安全のため。
            VsqEvent extra_note = new VsqEvent( extra_note_clock, new VsqID( 0 ) );
            extra_note.ID.type = VsqIDType.Anote;
            extra_note.ID.Note = 60;
            extra_note.ID.setLength( extra_note_clock_end - extra_note_clock );
            extra_note.ID.VibratoHandle = null;
            extra_note.ID.LyricHandle = new LyricHandle( "a", "a" );
            split.Track.get( track ).addEvent( extra_note );

            double trim_sec = 0.0; // レンダリング結果から省かなければならない秒数。
            if ( clock_start < split.getPreMeasureClocks() ) {
                trim_sec = split.getSecFromClock( clock_start );
            } else {
                split.removePart( vsq.getPreMeasureClocks(), clock_start );
                trim_sec = split.getSecFromClock( split.getPreMeasureClocks() );
            }
            split.updateTotalClocks();
            long total_samples = (long)((end_sec - start_sec) * SAMPLE_RATE);
            int trim_msec = (int)(trim_sec * 1000.0);
#if DEBUG
            PortUtil.println( "VSTiProxy#render; trim_msec=" + trim_msec );
#endif

            s_rendering_context = null;
            if ( s_working_renderer.Equals( VSTiProxy.RENDERER_UTU0 ) ) {
                s_rendering_context = new UtauRenderingRunner( split,
                                                               track,
                                                               AppManager.editorConfig.UtauSingers,
                                                               AppManager.editorConfig.PathResampler,
                                                               AppManager.editorConfig.PathWavtool,
                                                               temp_dir,
                                                               AppManager.editorConfig.InvokeUtauCoreWithWine,
                                                               SAMPLE_RATE,
                                                               trim_msec,
                                                               total_samples,
                                                               mode_infinite,
                                                               wave_writer,
                                                               wave_read_offset_seconds,
                                                               reader,
                                                               direct_play,
                                                               reflect_amp_to_wave );
            } else if ( s_working_renderer.Equals( VSTiProxy.RENDERER_STR0 ) ){
                s_rendering_context = new StraightRenderingRunner( split,
                                                                   track,
                                                                   AppManager.editorConfig.UtauSingers,
                                                                   SAMPLE_RATE,
                                                                   trim_msec,
                                                                   total_samples,
                                                                   mode_infinite,
                                                                   wave_writer,
                                                                   wave_read_offset_seconds,
                                                                   reader,
                                                                   direct_play,
                                                                   reflect_amp_to_wave );
#if ENABLE_AQUESTONE
            } else if ( s_working_renderer.Equals( VSTiProxy.RENDERER_AQT0 ) ){
                s_rendering_context = new AquesToneRenderingRunner( aquesToneDriver,
                                                                    split,
                                                                    track,
                                                                    temp_dir,
                                                                    SAMPLE_RATE,
                                                                    trim_msec,
                                                                    total_samples,
                                                                    mode_infinite,
                                                                    wave_writer,
                                                                    wave_read_offset_seconds,
                                                                    reader,
                                                                    direct_play,
                                                                    reflect_amp_to_wave );
#endif
            } else {
#if ENABLE_VOCALOID
                VocaloidDriver driver = null;
                for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                    if ( vocaloidDriver.get( i ).name.Equals( s_working_renderer ) ) {
                        driver = vocaloidDriver.get( i );
                        break;
                    }
                }
                VsqNrpn[] nrpn = VsqFile.generateNRPN( split, track, ms_presend );
                NrpnData[] nrpn_data = VsqNrpn.convert( nrpn );
#if DEBUG
                TempoTableEntry[] tempoinfo = split.TempoTable.toArray( new TempoTableEntry[] { } );
                PortUtil.println( "VSTiPRoxy#render; tempoinfo.Length=" + tempoinfo.Length );
                for ( int i = 0; i < tempoinfo.Length; i++ ) {
                    TempoTableEntry tte = tempoinfo[i];
                    PortUtil.println( "    #" + i + "; " + tte );
                }
#endif
                s_rendering_context = new VocaloidRenderingRunner( s_working_renderer,
                                                                 nrpn_data,
                                                                 split.TempoTable.toArray( new TempoTableEntry[]{} ),
                                                                 /*amplify_left,
                                                                 amplify_right,*/
                                                                 trim_msec,
                                                                 total_samples,
                                                                 wave_read_offset_seconds,
                                                                 mode_infinite,
                                                                 driver,
                                                                 direct_play,
                                                                 wave_writer,
                                                                 reader,
                                                                 track,
                                                                 reflect_amp_to_wave,
                                                                 SAMPLE_RATE );
#else
                return;
#endif
            }
            if ( direct_play ) {
#if JAVA
                Thread thread = new Thread( s_rendering_context );
                thread.start();
#else
                Thread thread = new Thread( new ParameterizedThreadStart( renderWithDirectPlay ) );
                thread.Priority = ThreadPriority.Normal;
                thread.Start( s_rendering_context );
#endif
            } else {
                s_rendering_context.run();
            }
        }

#if JAVA
        private class RenderWithDirectPlayProc extends Thread{
        public void run(){
            Object argument = s_rendering_context;
#else
        private static void renderWithDirectPlay( Object argument ) {
#endif
#if ENABLE_VOCALOID
            if ( argument is VocaloidRenderingRunner ) {
                VocaloidRenderingRunner sra = (VocaloidRenderingRunner)argument;
                sra.run();
            } else
#endif
            if ( argument is UtauRenderingRunner ) {
                UtauRenderingRunner arg = (UtauRenderingRunner)argument;
                arg.run();
            } else if ( argument is StraightRenderingRunner ) {
                StraightRenderingRunner arg = (StraightRenderingRunner)argument;
                arg.run();
            }
#if ENABLE_AQUESTONE
            else if ( argument is AquesToneRenderingRunner ) {
                AquesToneRenderingRunner arg = (AquesToneRenderingRunner)argument;
                arg.run();
            }
#endif
        }
#if JAVA
        }
#endif

        public static double computeRemainintSeconds() {
            if ( s_rendering_context != null ) {
                return s_rendering_context.computeRemainingSeconds();
            } else {
                return 0.0;
            }
        }

        public static double getElapsedSeconds() {
            if ( s_rendering_context != null ) {
                return s_rendering_context.getElapsedSeconds();
            } else {
                return 0.0;
            }
        }

        public static double getProgress() {
            if ( s_rendering_context == null ) {
                return 0.0;
            } else {
                return s_rendering_context.getProgress();
            }
        }

        public static void abortRendering() {
            if ( s_rendering_context != null ){
                s_rendering_context.abortRendering();
            }
        }

        public static int getErrorSamples( float tempo ) {
            if ( tempo <= 240 ) {
                return 4666;
            } else {
                float x = tempo - 240;
                return (int)((a2 * x + a1) * x + a0);
            }
        }

        public static float getPlayTime() {
            double pos = PlaySound.getPosition();
            return (float)pos;
        }
    }

#if !JAVA
}
#endif
