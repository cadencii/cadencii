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

package com.github.cadencii;

import java.io.*;
import java.util.*;
import com.github.cadencii.*;
import com.github.cadencii.media.*;
import com.github.cadencii.vsq.*;

#else

using System;
using System.Threading;
using com.github.cadencii;
using com.github.cadencii.java.util;
using com.github.cadencii.media;
using com.github.cadencii.vsq;
using com.github.cadencii.java.io;

namespace com.github.cadencii {
    using boolean = System.Boolean;
#endif

#if JAVA
    class VocaloidDaemon
    {
        public BufferedOutputStream outputStream;
        public BufferedInputStream inputStream;
        private Process mProcess;
        // 一時ディレクトリの実際のパス
        private String mTempPathUnixName;

        public VocaloidDaemon( Process p, String temp_path_unix_name )
        {
            if( p == null ){
                return;
            }
            mProcess = p;
            outputStream = new BufferedOutputStream( mProcess.getOutputStream() );
            inputStream = new BufferedInputStream( mProcess.getInputStream() );
            mTempPathUnixName = temp_path_unix_name;
        }
        
        public String getTempPathUnixName()
        {
            return mTempPathUnixName;
        }
        
        public void terminate()
        {
            if( fsys.isDirectoryExists( mTempPathUnixName ) ){
                String stop = fsys.combine( mTempPathUnixName, "stop" );
                if( fsys.isFileExists( stop ) ){
                    try{
                        PortUtil.deleteFile( stop );
                    }catch( Exception ex ){
                        ex.printStackTrace();
                    }
                }
                try{
                    PortUtil.deleteDirectory( mTempPathUnixName );
                }catch( Exception ex ){
                    ex.printStackTrace();
                }
            }
            if( mProcess == null ){
                return;
            }
            mProcess.destroy();
        }
    }
#endif

    /// <summary>
    /// VSTiのDLLを管理するクラス
    /// </summary>
#if JAVA
    public class VSTiDllManager {
#else
    public static class VSTiDllManager {
#endif
        //public static int SAMPLE_RATE = 44100;
        const float a0 = -17317.563f;
        const float a1 = 86.7312112f;
        const float a2 = -0.237323499f;
        /// <summary>
        /// 使用するボカロの最大バージョン．2までリリースされているので今は2
        /// </summary>
        const int MAX_VOCALO_VERSION = 2;
        /// <summary>
        /// Wineでインストールされている（かもしれない）AquesToneのvsti dllのパス．windowsのパス区切り形式で代入すること
        /// </summary>
        public static String WineAquesToneDll = "C:\\Program Files\\Steinberg\\VSTplugins\\AquesTone.dll";

#if ENABLE_VOCALOID
#if JAVA
        /// <summary>
        /// vocaloidrv.exeのプロセス
        /// </summary>
        public static VocaloidDaemon[] vocaloidrvDaemon = null;
#else
        public static Vector<VocaloidDriver> vocaloidDriver = new Vector<VocaloidDriver>();
#endif
#endif

#if ENABLE_AQUESTONE
        /// <summary>
        /// AquesTone VSTi のドライバ
        /// </summary>
        private static AquesToneDriver aquesToneDriver;
        /// <summary>
        /// AquesTone2 VSTi のドライバ
        /// </summary>
        private static AquesTone2Driver aquesTone2Driver;
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
                return new AquesToneWaveGenerator( getAquesToneDriver() );
            } else if ( kind == RendererKind.AQUES_TONE2 ) {
                return new AquesTone2WaveGenerator( getAquesTone2Driver() );
#endif
            } else if ( kind == RendererKind.VCNT ) {
                return new VConnectWaveGenerator();
            } else if ( kind == RendererKind.UTAU ) {
                return new UtauWaveGenerator();
            } else if ( kind == RendererKind.VOCALOID1 ||
                        kind == RendererKind.VOCALOID2 ) {
#if ENABLE_VOCALOID
                return new VocaloidWaveGenerator();
#endif
            }
            return new EmptyWaveGenerator();
        }

#if JAVA
        /// <summary>
        /// createtempdir.exeユーティリティを呼び出して，wine内の一時ディレクトリに
        /// 新しいディレクトリを作成します．drive_cから直接作ってもいいけど，
        /// 一時ディレクトリがどこかはwindowsでGetTempPathを呼ばない限り分からないので．
        /// </summary>
        private static String createTempPath()
        {
            Vector<String> list = AppManager.getWineProxyArgument();
            list.add( fsys.combine( PortUtil.getApplicationStartupPath(), "createtempdir.exe" ) );
            try{
                Process p = Runtime.getRuntime().exec( list.toArray( new String[0] ) );
                p.waitFor();
                InputStream i = p.getInputStream();
                int avail = i.available();
                char[] c = new char[avail];
                for( int j = 0; j < avail; j++ ){
                    c[j] = (char)i.read();
                }
                String ret = new String( c );
                return ret;
            }catch( Exception ex ){
                ex.printStackTrace();
            }
            return "";
        }
#endif

#if JAVA
        public static void restartVocaloidrvDaemon()
        {
#if ENABLE_VOCALOID
        if( vocaloidrvDaemon == null ){
                vocaloidrvDaemon = new VocaloidDaemon[MAX_VOCALO_VERSION];
            }
            for( int i = 0; i < vocaloidrvDaemon.length; i++ ){
                VocaloidDaemon vd = vocaloidrvDaemon[i];
                if( vd == null ){
                    continue;
                }
                vd.terminate();
            }
            Thread t = new Thread( new Runnable(){
                @Override
                public void run(){
                    for( int ver = 1; ver <= vocaloidrvDaemon.length; ver++ ){
                        // /bin/sh vocaloidrv.sh WINEPREFIX WINETOP vocaloidrv.exe midi_master.bin midi_body.bin TOTAL_SAMPLES
                        Vector<String> list = AppManager.getWineProxyArgument();
                        String vocaloidrv_exe =
                            Utility.normalizePath( fsys.combine( PortUtil.getApplicationStartupPath(), "vocaloidrv.exe" ) );
                        list.add( vocaloidrv_exe );
                        
                        SynthesizerType st = (ver == 1) ? SynthesizerType.VOCALOID1 : SynthesizerType.VOCALOID2;
                        String dll =
                            Utility.normalizePath( VocaloSysUtil.getDllPathVsti( st ) );
                        list.add( dll );
                        list.add( "-e" );
                        String tmp = createTempPath();
                        list.add( Utility.normalizePath( tmp ) );
#if DEBUG
                        sout.println( "VocaloidWaveGenerator#begin; list=" );
                        for( String s : list ){
                            sout.println( "    " + s );
                        }
#endif
                        try{
                            Process p = Runtime.getRuntime().exec( list.toArray( new String[0] ) );
                            String tmp_unix = 
                                VocaloSysUtil.combineWinePath(
                                    Utility.normalizePath( AppManager.editorConfig.WinePrefix ),
                                    tmp );
                            vocaloidrvDaemon[ver - 1] = new VocaloidDaemon( p, tmp_unix );
                            final InputStream iserr = p.getErrorStream();
                            Thread t2 = new Thread( new Runnable(){
                                @Override
                                public void run()
                                {
                                    try{
                                        final int BUFLEN = 1024;
                                        byte[] buffer = new byte[BUFLEN];
#if DEBUG
                                        byte[] line = new byte[BUFLEN];
                                        int pos = 0;
#endif
                                        while( true ){
                                            while( iserr.available() < BUFLEN ){
                                                Thread.sleep( 100 );
                                            }
                                            int i = iserr.read( buffer );
#if DEBUG
                                            if( pos + i >= line.length ){
                                                byte[] tmp = line;
                                                line = new byte[tmp.length + BUFLEN];
                                                for( int j = 0; j < tmp.length; j++ ){
                                                    line[j] = tmp[j];
                                                }
                                            }
                                            for( int j = 0; j < i; j++ ){
                                                line[pos + j] = buffer[j];
                                            }
                                            pos += i;
                                            // lineのどこかに0x0d, 0x0aが入っているか探す
                                            int indx_nl = 0;
                                            while( indx_nl >= 0 ){
                                                indx_nl = -1;
                                                for( int j = 0; j < pos; j++ ){
                                                    int code = (0xff & line[j]);
                                                    if( code == 0x0d || code == 0x0a ){
                                                        indx_nl = j;
                                                        // 次の文字も0x0d, 0x0aなら，無視するようにする
                                                        if( j + 1 < pos ){
                                                            int coden = (0xff & line[j + 1]);
                                                            if( coden == 0x0d || coden == 0x0a ){
                                                                for( int k = j + 1; k < pos - 1; k++ ){
                                                                    line[k] = line[k + 1];
                                                                }
                                                                pos--;
                                                            }
                                                        }
                                                        break;
                                                    }
                                                }
                                                if( indx_nl >= 0 ){
                                                    // 0からindx_nl - 1までをプリントアウトする
                                                    String sl = new String( line, 0, indx_nl );
                                                    if( !sl.startsWith( "fixme:font:" ) &&
                                                        !sl.startsWith( "Font metrics:" ) &&
                                                        !sl.startsWith( "err:font:" ) ){ 
                                                        System.err.println( sl );
                                                    }
                                                    for( int j = indx_nl + 1; j < pos; j++ ){
                                                        line[j - indx_nl - 1] = line[j];
                                                    }
                                                    pos -= (indx_nl + 1);
                                                }
                                            }
#endif
                                            if( i < BUFLEN ){
                                                break;
                                            }
                                        }
                                    }catch( Exception ex2 ){
                                        ex2.printStackTrace();
                                    }
                                }
                            } );
                            t2.start(); //*/
                        }catch( Exception ex ){
                            ex.printStackTrace();
                            vocaloidrvDaemon[ver - 1] = null;
                        }
                    }
                }
            } );
            t.start();
#endif // ENABLE_VOCALOID
        }
#endif // JAVA

        public static void init() {
#if ENABLE_VOCALOID
#if JAVA
            restartVocaloidrvDaemon();
#else
            int default_dse_version = VocaloSysUtil.getDefaultDseVersion();
            String editor_dir = VocaloSysUtil.getEditorPath( SynthesizerType.VOCALOID1 );
            String ini = "";
            if( !editor_dir.Equals( "" ) ){
                ini = fsys.combine( PortUtil.getDirectoryName( editor_dir ), "VOCALOID.ini" );
            }
            String vocalo2_dll_path = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 );
            String vocalo1_dll_path = VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID1 );
            if ( !str.compare( vocalo2_dll_path, "" ) &&
                    fsys.isFileExists( vocalo2_dll_path ) &&
                    !AppManager.editorConfig.DoNotUseVocaloid2 ) {
                VocaloidDriver vr = new VocaloidDriver( RendererKind.VOCALOID2 );
                vr.path = vocalo2_dll_path;
                vr.loaded = false;
                vocaloidDriver.add( vr );
            }
            if ( !str.compare( vocalo1_dll_path, "" ) &&
                    fsys.isFileExists( vocalo1_dll_path ) &&
                    !AppManager.editorConfig.DoNotUseVocaloid1 ) {
                VocaloidDriver vr = new VocaloidDriver( RendererKind.VOCALOID1 );
                vr.path = vocalo1_dll_path;
                vr.loaded = false;
                vocaloidDriver.add( vr );
            }

            for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                String dll_path = vocaloidDriver.get( i ).path;
                boolean loaded = false;
                try {
                    if ( dll_path != "" ) {
                        // 読込み。
                        loaded = vocaloidDriver.get( i ).open( 44100, 44100 );
                    } else {
                        loaded = false;
                    }
                    vocaloidDriver.get( i ).loaded = loaded;
                } catch ( Exception ex ) {
                    serr.println( "VSTiProxy#initCor; ex=" + ex );
                }
            }
#endif // JAVA
#endif // ENABLE_VOCALOID

#if ENABLE_AQUESTONE
            reloadAquesTone();
#endif
        }

        /// <summary>
        /// 初期化した AquesTone ドライバを取得する
        /// </summary>
        /// <returns></returns>
        public static AquesToneDriver getAquesToneDriver()
        {
            if ( aquesToneDriver == null && !AppManager.editorConfig.DoNotUseAquesTone ) {
                aquesToneDriver = new AquesToneDriver( AppManager.editorConfig.PathAquesTone );
            }
            return aquesToneDriver;
        }

        /// <summary>
        /// 初期化した AquesTone2 ドライバを取得する
        /// </summary>
        /// <returns></returns>
        public static AquesTone2Driver getAquesTone2Driver()
        {
            if ( aquesTone2Driver == null && !AppManager.editorConfig.DoNotUseAquesTone2 ) {
                aquesTone2Driver = new AquesTone2Driver( AppManager.editorConfig.PathAquesTone2 );
            }
            return aquesTone2Driver;
        }

#if ENABLE_AQUESTONE
        public static void reloadAquesTone() {
            aquesToneDriver = getAquesToneDriver();
            aquesTone2Driver = getAquesTone2Driver();
        }
#endif

        public static boolean isRendererAvailable( RendererKind renderer, String wine_prefix, String wine_top ) {
#if ENABLE_VOCALOID
            for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                if ( renderer == vocaloidDriver.get( i ).getRendererKind() && vocaloidDriver.get( i ).loaded ) {
                    return true;
                }
            }
#endif // ENABLE_VOCALOID

#if ENABLE_AQUESTONE
            if ( renderer == RendererKind.AQUES_TONE && aquesToneDriver != null && aquesToneDriver.loaded ) {
                return true;
            }
            if ( renderer == RendererKind.AQUES_TONE2 && aquesTone2Driver != null && aquesTone2Driver.loaded ) {
                return true;
            }
#endif

            if ( renderer == RendererKind.UTAU ) {
                // ここでは，resamplerの内どれかひとつでも使用可能であればOKの判定にする
                boolean resampler_exists = false;
                int size = AppManager.editorConfig.getResamplerCount();
                for ( int i = 0; i < size; i++ ) {
                    String path = AppManager.editorConfig.getResamplerAt( i );
                    if ( fsys.isFileExists( path ) ) {
                        resampler_exists = true;
                        break;
                    }
                }
                if ( resampler_exists &&
                     !AppManager.editorConfig.PathWavtool.Equals( "" ) && fsys.isFileExists( AppManager.editorConfig.PathWavtool ) ) {
                    if ( AppManager.editorConfig.UtauSingers.size() > 0 ) {
                        return true;
                    }
                }
            }
            if ( renderer == RendererKind.VCNT ) {
                String synth_path = fsys.combine( PortUtil.getApplicationStartupPath(), VConnectWaveGenerator.STRAIGHT_SYNTH );
                if ( fsys.isFileExists( synth_path ) ) {
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
            for ( int i = 0; i < vocaloidDriver.size(); i++ ) {
                if ( vocaloidDriver.get( i ) != null ) {
                    vocaloidDriver.get( i ).close();
                }
            }
            vocaloidDriver.clear();
#endif // !ENABLE_VOCALOID

#if ENABLE_AQUESTONE
            if ( aquesToneDriver != null ) { aquesToneDriver.close(); }
            if ( aquesTone2Driver != null ) { aquesTone2Driver.close(); }
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
