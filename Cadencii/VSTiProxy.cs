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
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using System.Security;
using System.Windows.Forms;

using Boare.Lib.Vsq;
using Boare.Lib.Media;
using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public delegate void WaveIncomingEventHandler( double[] L, double[] R );
    public delegate void RenderingFinishedEventHandler();

    public static class VSTiProxy {
        public const String RENDERER_DSB2 = "DSB2";
        public const String RENDERER_DSB3 = "DSB3";
        public const String RENDERER_UTU0 = "UTU0";
        public const String RENDERER_STR0 = "STR0";
        public const int SAMPLE_RATE = 44100;
        private const int BLOCK_SIZE = 4410;

        public static String CurrentUser = "";
        private static String s_working_renderer = "";
        private static Vector<VstiRenderer> m_vstidrv = new Vector<VstiRenderer>();

        private static RenderingRunner s_rendering_context;

        public static void init() {
            initCor();
        }

        public static void initCor() {
#if DEBUG
            AppManager.debugWriteLine( "VSTiProxy..cctor" );
#endif
            PlaySound.Init( BLOCK_SIZE, SAMPLE_RATE );
            PlaySound.SetResolution( 4410 );
#if !DEBUG
            try {
#endif
                String vocalo2_dll_path = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 );
                String vocalo1_dll_path = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID1 );
                if ( vocalo2_dll_path != "" && PortUtil.isFileExists( vocalo2_dll_path ) ) {
                    VstiRenderer vr = new VstiRenderer();
                    vr.path = vocalo2_dll_path;
                    vr.loaded = false;
                    vr.dllInstance = new vstidrv();
                    vr.name = RENDERER_DSB3;
                    m_vstidrv.add( vr );
                }
                if ( vocalo1_dll_path != "" && PortUtil.isFileExists( vocalo1_dll_path ) ) {
                    VstiRenderer vr = new VstiRenderer();
                    vr.path = vocalo1_dll_path;
                    vr.loaded = false;
                    vr.dllInstance = new vstidrv();
                    vr.name = RENDERER_DSB2;
                    m_vstidrv.add( vr );
                }
#if !DEBUG
            } catch ( Exception ex ){
                AppManager.debugWriteLine( "    ex=" + ex );
                bocoree.debug.push_log( "    ex=" + ex );
            }
#endif

#if TEST
            bocoree.debug.push_log( "vstidrv.Count=" + m_vstidrv.size() );
#endif
            for ( int i = 0; i < m_vstidrv.size(); i++ ) {
#if TEST
                bocoree.debug.push_log( "Name=" + m_vstidrv.get( i ).name + "; Path=" + m_vstidrv.get( i ).path );
#endif
                String dll_path = m_vstidrv.get( i ).path;
                boolean loaded = false;
                try {
                    char[] str = dll_path.ToCharArray();
                    if ( dll_path != "" ) {
                        loaded = m_vstidrv.get( i ).dllInstance.Init( str, BLOCK_SIZE, SAMPLE_RATE );
                    } else {
                        loaded = false;
                    }
                    m_vstidrv.get( i ).loaded = loaded;
#if TEST && DEBUG
                    bocoree.debug.push_log( "VSTiProxy..cctor()" );
                    bocoree.debug.push_log( "    dll_path=" + dll_path );
                    bocoree.debug.push_log( "    loaded=" + loaded );

#endif
                } catch ( Exception ex ) {
#if TEST
                    bocoree.debug.push_log( "    ex=" + ex );
#endif
                }
            }
        }

        public static boolean isRendererAvailable( String renderer ) {
            for ( int i = 0; i < m_vstidrv.size(); i++ ) {
                if ( renderer.StartsWith( m_vstidrv.get( i ).name ) && m_vstidrv.get( i ).loaded ) {
                    return true;
                }
            }
            if ( renderer.StartsWith( RENDERER_UTU0 ) ) {
                if ( AppManager.editorConfig.PathResampler != "" && PortUtil.isFileExists( AppManager.editorConfig.PathResampler ) &&
                     AppManager.editorConfig.PathWavtool != "" && PortUtil.isFileExists( AppManager.editorConfig.PathWavtool ) ) {
                    if ( AppManager.editorConfig.UtauSingers.size() > 0 ) {
                        return true;
                    }
                }
            }
            if ( renderer.StartsWith( RENDERER_STR0 ) ) {
                if ( PortUtil.isFileExists( Path.Combine( Application.StartupPath, StraightRenderingRunner.STRAIGHT_SYNTH ) ) ) {
                    int count = AppManager.editorConfig.UtauSingers.size();
                    for ( int i = 0; i < count; i++ ) {
                        String analyzed = Path.Combine( AppManager.editorConfig.UtauSingers.get( i ).VOICEIDSTR, "analyzed" );
                        if ( Directory.Exists( analyzed ) ) {
                            String analyzed_oto_ini = Path.Combine( analyzed, "oto.ini" );
                            if ( PortUtil.isFileExists( analyzed_oto_ini ) ) {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static int SampleRate {
            get {
                return SAMPLE_RATE;
            }
        }

        public static void terminate() {
            for ( int i = 0; i < m_vstidrv.size(); i++ ) {
                if ( m_vstidrv.get( i ).dllInstance != null ) {
                    m_vstidrv.get( i ).dllInstance.Terminate();
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
                PlaySound.Reset();
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
            }
#if DEBUG
            bocoree.debug.push_log( "s_working_renderer=" + s_working_renderer );
#endif
            VsqFileEx split = (VsqFileEx)vsq.Clone();
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
            extra_note.ID.Length = extra_note_clock_end - extra_note_clock;
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
                                                                   mode_infinite,
                                                                   wave_writer,
                                                                   wave_read_offset_seconds,
                                                                   reader,
                                                                   direct_play,
                                                                   reflect_amp_to_wave );
            } else {
                VstiRenderer driver = null;
                for ( int i = 0; i < m_vstidrv.size(); i++ ) {
                    if ( m_vstidrv.get( i ).name.Equals( s_working_renderer ) ) {
                        driver = m_vstidrv.get( i );
                        break;
                    }
                }
                VsqNrpn[] nrpn = VsqFile.generateNRPN( split, track, ms_presend );
                NrpnData[] nrpn_data = VsqNrpn.convert( nrpn );
                s_rendering_context = new VocaloRenderingRunner( s_working_renderer,
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
                                                                 reflect_amp_to_wave );
            }
            if ( direct_play ) {
                Thread thread = new Thread( new ParameterizedThreadStart( renderWithDirectPlay ) );
                thread.Priority = ThreadPriority.Normal;
                thread.Start( s_rendering_context );
            } else {
                s_rendering_context.run();
            }
        }

        private static void renderWithDirectPlay( object argument ) {
            if ( argument is VocaloRenderingRunner ) {
                VocaloRenderingRunner sra = (VocaloRenderingRunner)argument;
                sra.run();
            } else if ( argument is UtauRenderingRunner ) {
                UtauRenderingRunner arg = (UtauRenderingRunner)argument;
                arg.run();
            } else if ( argument is StraightRenderingRunner ) {
                StraightRenderingRunner arg = (StraightRenderingRunner)argument;
                arg.run();
            }
        }

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

        public static float getPlayTime() {
            double pos = PlaySound.GetPosition();
            return (float)pos;
        }
    }

}
