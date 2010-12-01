/**
 * VSTiDllManager.cs
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

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    public class VSTiDllManager {
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

#if ENABLE_VOCALOID
        public static Vector<VocaloidDriver> vocaloidDriver = new Vector<VocaloidDriver>();
#endif

        public static WaveGenerator getWaveGenerator( RendererKind kind ) {
            switch ( kind ) {
                case RendererKind.AQUES_TONE: {
                    return new AquesToneWaveGenerator();
                }
                case RendererKind.STRAIGHT_UTAU: {
                    return new VConnectWaveGenerator();
                }
                case RendererKind.UTAU: {
                    return new UtauWaveGenerator();
                }
                case RendererKind.VOCALOID1_100:
                case RendererKind.VOCALOID1_101:
                case RendererKind.VOCALOID2: {
                    return new VocaloidWaveGenerator();
                }
                default: {
                    return new EmptyWaveGenerator();
                }
            }
        }

        public static void init() {
            initCor();
        }

        public static void initCor() {
#if ENABLE_VOCALOID
            int default_dse_version = VocaloSysUtil.getDefaultDseVersion();
            String editor_dir = VocaloSysUtil.getEditorPath( SynthesizerType.VOCALOID1 );
            String ini = "";
            if( !editor_dir.Equals( "" ) ){
                ini = PortUtil.combinePath( PortUtil.getDirectoryName( editor_dir ), "VOCALOID.ini" );
            }
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
                if ( !ini.Equals( "" ) && PortUtil.isFileExists( ini ) ) {
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
            AquesToneDriver.reload();
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
            AquesToneDriver aquesToneDriver = AquesToneDriver.getInstance();
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
                string synth_path = PortUtil.combinePath( PortUtil.getApplicationStartupPath(), VConnectWaveGenerator.STRAIGHT_SYNTH );
                if ( PortUtil.isFileExists( synth_path ) ) {
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
#if !MONO
            if ( org.kbinani.cadencii.util.DllLoad.isInitialized() ) {
                org.kbinani.cadencii.util.DllLoad.terminate();
            }
#endif
#endif

#if ENABLE_AQUESTONE
            AquesToneDriver aquesToneDriver = AquesToneDriver.getInstance();
            if ( aquesToneDriver != null ) {
                try {
                    aquesToneDriver.close();
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "VSTiProxy#terminate; ex=" + ex );
                }
            }
#endif
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
