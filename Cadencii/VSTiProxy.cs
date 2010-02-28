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
#if FAKE_AQUES_TONE_DLL_AS_VOCALOID1
#if !DEBUG
#error FAKE_AQUES_TONE_DLL_AS_VOCALOID1 is not valid definition for release build
#endif
#if !ENABLE_VOCALOID
#error FAKE_AQUES_TONE_DLL_AS_VOCALOID1 is not valid definition for ifndef ENABLE_VOCALOID
#endif
#endif
using System;
using System.Threading;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;
using org.kbinani.java.io;
using org.kbinani.cadencii.util;
//using org.kbinani.cadencii.implTrunk;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    public class VSTiProxy {
        public const String RENDERER_DSB2 = "DSB2";
        public const String RENDERER_DSB3 = "DSB3";
        public const String RENDERER_UTU0 = "UTU0";
        public const String RENDERER_STR0 = "STR0";
        public const String RENDERER_AQT0 = "AQT0";
        /// <summary>
        /// EmtryRenderingRunnerが使用される
        /// </summary>
        public const String RENDERER_NULL = "NUL0";
        public static int SAMPLE_RATE = 44100;
        const float a0 = -17317.563f;
        const float a1 = 86.7312112f;
        const float a2 = -0.237323499f;

        public static String CurrentUser = "";
        private static RendererKind s_working_renderer = RendererKind.NULL;
#if ENABLE_VOCALOID
        public static Vector<VocaloidDriver> vocaloidDriver = new Vector<VocaloidDriver>();
#endif
#if ENABLE_AQUESTONE
#if FAKE_AQUES_TONE_DLL_AS_VOCALOID1
        public static VocaloidDriver aquesToneDriver = null;
#else
        public static AquesToneDriver aquesToneDriver = null;
#endif
#endif

#if DEBUG
        delegate int PADDFUNC( int a, int b );
#endif

        private static RenderingRunner s_rendering_context;

        public static void init() {
            initCor();
        }

        public static void initCor() {
#if ENABLE_VOCALOID
            int default_dse_version = VocaloSysUtil.getDefaultDseVersion();
            String editor_dir = VocaloSysUtil.getEditorPath( SynthesizerType.VOCALOID1 );
            String ini = PortUtil.combinePath( PortUtil.getDirectoryName( editor_dir ), "VOCALOID.ini" );
            String vocalo2_dll_path = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 );
            String vocalo1_dll_path = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID1 );
            if ( vocalo2_dll_path != "" && PortUtil.isFileExists( vocalo2_dll_path ) ) {
                if ( !AppManager.editorConfig.DoNotUseVocaloid2 ) {
                    VocaloidDriver vr = new VocaloidDriver( 200 );
                    vr.path = vocalo2_dll_path;
                    vr.loaded = false;
                    vr.kind = RendererKind.VOCALOID2;
                    vocaloidDriver.add( vr );
                }
            }
            if ( vocalo1_dll_path != "" && PortUtil.isFileExists( vocalo1_dll_path ) ) {
                // VOCALOID.iniを読み込んでデフォルトのDSEVersionを調べる
#if DEBUG
                PortUtil.println( "VSTiProxy#initCor; ini=" + ini );
#endif
                if ( PortUtil.isFileExists( ini ) ) {
                    // デフォルトのDSEバージョンのVOCALOID1 VSTi DLL
                    if ( default_dse_version == 100 && !AppManager.editorConfig.DoNotUseVocaloid100 ||
                         default_dse_version == 101 && !AppManager.editorConfig.DoNotUseVocaloid101 ) {
                        VocaloidDriver vr = new VocaloidDriver( default_dse_version );
                        vr.path = vocalo1_dll_path;
                        vr.loaded = false;
                        vr.kind = (default_dse_version == 100) ? RendererKind.VOCALOID1_100 : RendererKind.VOCALOID1_101;
                        vocaloidDriver.add( vr );
                    }

                    // デフォルトじゃない方のVOCALOID1 VSTi DLLを読み込む
                    if ( AppManager.editorConfig.LoadSecondaryVocaloid1Dll ) {
                        int another_dse_version = (default_dse_version == 100) ? 101 : 100;
                        if ( another_dse_version == 100 && !AppManager.editorConfig.DoNotUseVocaloid100 ||
                             another_dse_version == 101 && !AppManager.editorConfig.DoNotUseVocaloid101 ) {
                            VocaloidDriver vr2 = new VocaloidDriver( another_dse_version );
                            vr2.path = (default_dse_version == 0) ? "" : vocalo1_dll_path; // DSEVersionが取得できない=>1.0しか使用できない
                            vr2.loaded = false;
                            vr2.kind = (another_dse_version == 100) ? RendererKind.VOCALOID1_100 : RendererKind.VOCALOID1_101;
                            vocaloidDriver.add( vr2 );
                        }
                    }
                }
            }

            for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                String dll_path = vocaloidDriver.get( i ).path;
                boolean loaded = false;
                try {
                    if ( dll_path != "" ) {
                        // VOCALOID1を読み込む場合で、かつ、DSEVersionがVOCALOID.iniの指定と異なるバージョンを読み込む場合、
                        // VOCALOID.iniの設定を一時的に書き換える。
                        boolean use_native_dll_loader = true;
                        int dse_version = vocaloidDriver.get( i ).getDseVersion();
                        if ( dse_version != 0 && dse_version != 200 && dse_version != default_dse_version ) {
                            use_native_dll_loader = false;
                        }
                        String copy_ini = "";
                        if ( !use_native_dll_loader ) {
                            // DSEVersionを強制的に書き換える。
                            copy_ini = PortUtil.createTempFile();
                            if ( PortUtil.isFileExists( ini ) ) {
                                PortUtil.deleteFile( copy_ini );
                                PortUtil.copyFile( ini, copy_ini );
                                BufferedReader br = null;
                                BufferedWriter bw = null;
                                try {
                                    br = new BufferedReader( new InputStreamReader( new FileInputStream( copy_ini ), "Shift_JIS" ) );
                                    bw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( ini ), "Shift_JIS" ) );
                                    while ( br.ready() ) {
                                        String line = br.readLine();
                                        if ( line == null ) {
                                            bw.newLine();
                                        } else if ( line.StartsWith( "DSEVersion" ) ) {
                                            bw.write( "DSEVersion = " + dse_version ); bw.newLine();
                                        } else {
                                            bw.write( line ); bw.newLine();
                                        }
                                    }
                                } catch ( Exception ex ) {
                                    PortUtil.stderr.println( "VSTiProxy#initCor; ex=" + ex );
                                } finally {
                                    if ( bw != null ) {
                                        try {
                                            bw.close();
                                        } catch ( Exception ex2 ) {
                                            PortUtil.stderr.println( "VSTiProxy#initCor; ex2=" + ex2 );
                                        }
                                    }
                                    if ( br != null ) {
                                        try {
                                            br.close();
                                        } catch ( Exception ex2 ) {
                                            PortUtil.stderr.println( "VSTiProxy#initCor; ex2=" + ex2 );
                                        }
                                    }
                                }
                            }
                        }
                        
                        // 読込み。
                        loaded = vocaloidDriver.get( i ).open( dll_path, SAMPLE_RATE, SAMPLE_RATE, use_native_dll_loader );

                        // VOCALOID.iniをもとにもどす。
                        if ( !use_native_dll_loader ) {
                            try {
                                PortUtil.deleteFile( ini );
                                PortUtil.copyFile( copy_ini, ini );
                                PortUtil.deleteFile( copy_ini );
                            } catch ( Exception ex ) {
                                PortUtil.stderr.println( "VSTiProxy#initCor; ex=" + ex );
                            }
                        }
                    } else {
                        loaded = false;
                    }
                    vocaloidDriver.get( i ).loaded = loaded;
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "VSTiProxy#initCor; ex=" + ex );
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
#if FAKE_AQUES_TONE_DLL_AS_VOCALOID1
                aquesToneDriver = new VocaloidDriver();
#else
                aquesToneDriver = new AquesToneDriver();
#endif
                aquesToneDriver.loaded = false;
                aquesToneDriver.kind = RendererKind.AQUES_TONE;
            }
            if ( aquesToneDriver.loaded ) {
                aquesToneDriver.close();
                aquesToneDriver.loaded = false;
            }
            aquesToneDriver.path = aques_tone;
            if ( !aques_tone.Equals( "" ) && PortUtil.isFileExists( aques_tone ) && !AppManager.editorConfig.DoNotUseAquesTone ) {
                boolean loaded = false;
                try {
#if FAKE_AQUES_TONE_DLL_AS_VOCALOID1
                    loaded = aquesToneDriver.open( aques_tone, SAMPLE_RATE, SAMPLE_RATE, false );
#else
                    loaded = aquesToneDriver.open( aques_tone, SAMPLE_RATE, SAMPLE_RATE, true );
#endif
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "VSTiProxy#realoadAquesTone; ex=" + ex );
                    loaded = false;
                }
                aquesToneDriver.loaded = loaded;
            }
#if DEBUG
            PortUtil.println( "VSTiProxy#initCor; aquesToneDriver.loaded=" + aquesToneDriver.loaded );
#endif
        }
#endif

        public static boolean isRendererAvailable( RendererKind renderer ) {
#if ENABLE_VOCALOID
            for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                if ( renderer == vocaloidDriver.get( i ).kind && vocaloidDriver.get( i ).loaded ) {
                    return true;
                }
            }
#endif

#if ENABLE_AQUESTONE
            if ( renderer == RendererKind.AQUES_TONE && aquesToneDriver != null && aquesToneDriver.loaded ) {
                return true;
            }
#endif

            if ( renderer == RendererKind.UTAU ) {
                if ( !AppManager.editorConfig.PathResampler.Equals( "" ) && PortUtil.isFileExists( AppManager.editorConfig.PathResampler ) &&
                     !AppManager.editorConfig.PathWavtool.Equals( "" ) && PortUtil.isFileExists( AppManager.editorConfig.PathWavtool ) ) {
                    if ( AppManager.editorConfig.UtauSingers.size() > 0 ) {
                        return true;
                    }
                }
            }
            if ( renderer == RendererKind.STRAIGHT_UTAU ) {
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
            if ( DllLoad.isInitialized() ) {
                DllLoad.terminate();
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
            s_working_renderer = VsqFileEx.getTrackRendererKind( vsq.Track.get( track ) );
            Vector<WaveReader> reader = new Vector<WaveReader>();
            for ( int i = 0; i < files.Length; i++ ) {
                reader.add( files[i] );
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
            //split.Track.get( track ).addEvent( extra_note );

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
            PortUtil.println( "VSTiProxy#render; split.Track.get( track ).getEventCount()=" + split.Track.get( track ).getEventCount() );
            PortUtil.println( "VSTiProxy#render; trim_msec=" + trim_msec );
#endif

            s_rendering_context = null;
            if ( s_working_renderer == RendererKind.UTAU ) {
                s_rendering_context = new UtauRenderingRunner( split,
                                                               track,
                                                               AppManager.editorConfig.UtauSingers,
                                                               AppManager.editorConfig.PathResampler,
                                                               AppManager.editorConfig.PathWavtool,
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
            } else if ( s_working_renderer == RendererKind.STRAIGHT_UTAU ){
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
            } else if ( s_working_renderer == RendererKind.AQUES_TONE ) {
#if FAKE_AQUES_TONE_DLL_AS_VOCALOID1
                split.Track.get( track ).getCommon().Version = "DSB2";
                VsqNrpn[] nrpn = VsqFile.generateNRPN( split, track, ms_presend );
                NrpnData[] nrpn_data = VsqNrpn.convert( nrpn );
                s_rendering_context = new VocaloidRenderingRunner( s_working_renderer,
                                                                   nrpn_data,
                                                                   split.TempoTable.toArray( new TempoTableEntry[] { } ),
                                                                   trim_msec,
                                                                   total_samples,
                                                                   wave_read_offset_seconds,
                                                                   mode_infinite,
                                                                   aquesToneDriver,
                                                                   direct_play,
                                                                   wave_writer,
                                                                   reader,
                                                                   track,
                                                                   reflect_amp_to_wave,
                                                                   SAMPLE_RATE,
                                                                   ms_presend );
#else
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
#endif
            } else if ( s_working_renderer == RendererKind.NULL ){
                s_rendering_context = new EmptyRenderingRunner( track,
                                                                reflect_amp_to_wave,
                                                                wave_writer,
                                                                wave_read_offset_seconds,
                                                                reader,
                                                                direct_play,
                                                                trim_msec,
                                                                total_samples,
                                                                SAMPLE_RATE,
                                                                mode_infinite );
            } else {
#if ENABLE_VOCALOID
                VocaloidDriver driver = null;
                for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                    if ( vocaloidDriver.get( i ).kind == s_working_renderer ) {
                        driver = vocaloidDriver.get( i );
                        break;
                    }
                }
                VsqNrpn[] nrpn = VsqFile.generateNRPN( split, track, ms_presend );
                NrpnData[] nrpn_data = VsqNrpn.convert( nrpn );
                s_rendering_context = new VocaloidRenderingRunner( nrpn_data,
                                                                   split.TempoTable,
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
                                                                   SAMPLE_RATE,
                                                                   ms_presend );
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
            } else if ( argument is EmptyRenderingRunner ) {
                EmptyRenderingRunner arg = (EmptyRenderingRunner)argument;
                arg.run();
            }
#if ENABLE_AQUESTONE
 else if ( argument is AquesToneRenderingRunner ) {
                AquesToneRenderingRunner arg = (AquesToneRenderingRunner)argument;
                arg.run();
            }
#endif
            AppManager.setPlaying( false );
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
