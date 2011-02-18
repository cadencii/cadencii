/*
 * VSTiDllManager.cs
 * Copyright © 2008-2011 kbinani
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

    /// <summary>
    /// VSTiのDLLを管理するクラス
    /// </summary>
#if JAVA
    public class VSTiDllManager {
#else
    public static class VSTiDllManager {
#endif
        public const String RENDERER_DSB2 = "DSB2";
        public const String RENDERER_DSB3 = "DSB3";
        public const String RENDERER_UTU0 = "UTU0";
        public const String RENDERER_STR0 = "STR0";
        public const String RENDERER_AQT0 = "AQT0";
        /// <summary>
        /// EmtryRenderingRunnerが使用される
        /// </summary>
        public const String RENDERER_NULL = "NUL0";
        //public static int SAMPLE_RATE = 44100;
        const float a0 = -17317.563f;
        const float a1 = 86.7312112f;
        const float a2 = -0.237323499f;
        /// <summary>
        /// Wineでインストールされている（かもしれない）AquesToneのvsti dllのパス．windowsのパス区切り形式で代入すること
        /// </summary>
        public static String WineAquesToneDll = "C:\\Program Files\\Steinberg\\VSTplugins\\AquesTone.dll";

#if ENABLE_VOCALOID
#if !JAVA
        public static Vector<VocaloidDriver> vocaloidDriver = new Vector<VocaloidDriver>();
#endif
#endif

        /// <summary>
        /// 指定した合成器の種類に合致する合成器の新しいインスタンスを取得します
        /// </summary>
        /// <param name="kind">合成器の種類</param>
        /// <returns>指定した種類の合成器の新しいインスタンス</returns>
        public static WaveGenerator getWaveGenerator( RendererKind kind )
        {
            if ( kind == RendererKind.AQUES_TONE ) {
#if ENABLE_AQUESTONE
                return new AquesToneWaveGenerator();
#endif
            } else if ( kind == RendererKind.VCNT ) {
                return new VConnectWaveGenerator();
            } else if ( kind == RendererKind.UTAU ) {
                return new UtauWaveGenerator();
            } else if ( kind == RendererKind.VOCALOID1_100 ||
                        kind == RendererKind.VOCALOID1_101 ||
                        kind == RendererKind.VOCALOID2 ) {
#if ENABLE_VOCALOID
                return new VocaloidWaveGenerator();
#endif
            }
            return new EmptyWaveGenerator();
        }

        public static void init() {
#if ENABLE_VOCALOID
#if !JAVA
            int default_dse_version = VocaloSysUtil.getDefaultDseVersion();
            String editor_dir = VocaloSysUtil.getEditorPath( SynthesizerType.VOCALOID1 );
            String ini = "";
            if( !editor_dir.Equals( "" ) ){
                ini = fsys.combine( PortUtil.getDirectoryName( editor_dir ), "VOCALOID.ini" );
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
                sout.println( "VSTiProxy#init; ini=" + ini );
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
                                    serr.println( "VSTiProxy#initCor; ex=" + ex );
                                } finally {
                                    if ( bw != null ) {
                                        try {
                                            bw.close();
                                        } catch ( Exception ex2 ) {
                                            serr.println( "VSTiProxy#initCor; ex2=" + ex2 );
                                        }
                                    }
                                    if ( br != null ) {
                                        try {
                                            br.close();
                                        } catch ( Exception ex2 ) {
                                            serr.println( "VSTiProxy#initCor; ex2=" + ex2 );
                                        }
                                    }
                                }
                            }
                        }
                        
                        // 読込み。
                        loaded = vocaloidDriver.get( i ).open( 44100, 44100, use_native_dll_loader );

                        // VOCALOID.iniをもとにもどす。
                        if ( !use_native_dll_loader ) {
                            try {
                                PortUtil.deleteFile( ini );
                                PortUtil.copyFile( copy_ini, ini );
                                PortUtil.deleteFile( copy_ini );
                            } catch ( Exception ex ) {
                                serr.println( "VSTiProxy#initCor; ex=" + ex );
                            }
                        }
                    } else {
                        loaded = false;
                    }
                    vocaloidDriver.get( i ).loaded = loaded;
                } catch ( Exception ex ) {
                    serr.println( "VSTiProxy#initCor; ex=" + ex );
                }
            }
#endif // !JAVA
#endif // ENABLE_VOCALOID

#if ENABLE_AQUESTONE
            reloadAquesTone();
#endif
        }

#if ENABLE_AQUESTONE
        public static void reloadAquesTone() {
            AquesToneDriver.reload();
        }
#endif

        public static boolean isRendererAvailable( RendererKind renderer, String wine_prefix, String wine_top ) {
#if ENABLE_VOCALOID
#if JAVA
            if( renderer == RendererKind.VOCALOID2  || renderer == RendererKind.VOCALOID1_100 || renderer == RendererKind.VOCALOID1_101 ){
                String dll = (renderer == RendererKind.VOCALOID2) ?
                             VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 ) :
                             VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID1 );
                if( dll != null && dll.length() > 3 ){
                    String act_dll = VocaloSysUtil.combineWinePath( wine_prefix, dll );
                    String wine_exe = fsys.combine( fsys.combine( wine_top, "bin" ), "wine" );
#if DEBUG
                    sout.println( "VSTiDllManager#isRendererAvailable; act_dll=" + act_dll + "; exists=" + fsys.isFileExists( act_dll ) );
                    sout.println( "VSTiDllManager#isRendererAvailable; wine_exe=" + wine_exe + "; exists=" + fsys.isFileExists( wine_exe ) );
#endif // DEBUG
                    return fsys.isFileExists( wine_exe ) && fsys.isFileExists( act_dll );
                }
            }
#else // JAVA
            for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                if ( renderer == vocaloidDriver.get( i ).kind && vocaloidDriver.get( i ).loaded ) {
                    return true;
                }
            }
#endif // JAVA
#endif // ENABLE_VOCALOID

#if ENABLE_AQUESTONE
            AquesToneDriver aquesToneDriver = AquesToneDriver.getInstance();
            if ( renderer == RendererKind.AQUES_TONE && aquesToneDriver != null && aquesToneDriver.loaded ) {
                return true;
            }
#endif

            if ( renderer == RendererKind.UTAU ) {
                // ここでは，resamplerの内どれかひとつでも使用可能であればOKの判定にする
                boolean resampler_exists = false;
                int size = AppManager.editorConfig.getResamplerCount();
                for ( int i = 0; i < size; i++ ) {
                    String path = AppManager.editorConfig.getResamplerAt( i );
                    if ( PortUtil.isFileExists( path ) ) {
                        resampler_exists = true;
                        break;
                    }
                }
                if ( resampler_exists &&
                     !AppManager.editorConfig.PathWavtool.Equals( "" ) && PortUtil.isFileExists( AppManager.editorConfig.PathWavtool ) ) {
                    if ( AppManager.editorConfig.UtauSingers.size() > 0 ) {
                        return true;
                    }
                }
            }
            if ( renderer == RendererKind.VCNT ) {
                String synth_path = fsys.combine( PortUtil.getApplicationStartupPath(), VConnectWaveGenerator.STRAIGHT_SYNTH );
                if ( PortUtil.isFileExists( synth_path ) ) {
                    int count = AppManager.editorConfig.UtauSingers.size();
                    if ( count > 0 ) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void terminate() {
#if ENABLE_VOCALOID
#if !JAVA
            for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                if ( vocaloidDriver.get( i ) != null ) {
                    vocaloidDriver.get( i ).close();
                }
            }
            vocaloidDriver.clear();
#endif // !JAVA
#if !MONO
#if !JAVA
            if ( org.kbinani.cadencii.util.DllLoad.isInitialized() ) {
                //うおお・・・
                org.kbinani.cadencii.util.DllLoad.terminate();
            }
#endif // !MONO
#endif // !JAVA
#endif // !ENABLE_VOCALOID

#if ENABLE_AQUESTONE
            AquesToneDriver.unload();
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
