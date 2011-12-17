#if ENABLE_AQUESTONE
/*
 * AquesToneDriver.cs
 * Copyright © 2009-2011 kbinani
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

import org.kbinani.*;
import org.kbinani.vsq.*;

#else

using System;
using System.Text;
using com.github.cadencii;
using com.github.cadencii.java.io;
using com.github.cadencii.vsq;

namespace com.github.cadencii
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class AquesToneDriver{
#else
    public class AquesToneDriver : vstidrv
    {
#endif
        public static readonly String[] PHONES = new String[] { 
            "ア", "イ", "ウ", "エ", "オ",
            "カ", "キ", "ク", "ケ", "コ",
            "サ", "シ", "ス", "セ", "ソ",
            "タ", "チ", "ツ", "テ", "ト",
            "ナ", "ニ", "ヌ", "ネ", "ノ",
            "ハ", "ヒ", "フ", "ヘ", "ホ",
            "マ", "ミ", "ム", "メ", "モ",
            "ヤ", "ユ", "イェ", "ヨ",
            "ラ", "リ", "ル", "レ", "ロ",
            "ワ", "ヲ",
            "ン",
            "ガ", "ギ", "グ", "ゲ", "ゴ",
            "ザ", "ジ", "ズ", "ゼ", "ゾ",
            "ダ", "デ", "ド",
            "バ", "ビ", "ブ", "ベ", "ボ",
            "パ", "ピ", "プ", "ペ", "ポ",
            "キャ", "キュ", "キェ", "キョ",
            "シャ", "シュ", "シェ", "ショ",
            "チャ", "チュ", "チェ", "チョ",
            "ニャ", "ニュ", "ニェ", "ニョ",
            "ヒャ", "ヒュ", "ヒェ", "ヒョ",
            "ミャ", "ミュ", "ミェ", "ミョ",
            "リャ", "リュ", "リェ", "リョ",
            "ギャ", "ギュ", "ギェ", "ギョ",
            "ジャ", "ジュ", "ジェ", "ジョ",
            "ウィ", "ウェ", "ウォ",
            "ツァ", "ツィ", "ツェ", "ツォ",
            "ファ", "フィ", "フェ", "フォ",
            "ビャ", "ビュ", "ビェ", "ビョ",
            "ピャ", "ピュ", "ピェ", "ピョ",
            "スィ", "ティ", "ズィ", "ディ",
            "トゥ", "ドゥ", "デュ", "テュ",
        };
        private static readonly SingerConfig female_f1 = new SingerConfig( "Female_F1", 0, 0 );
        private static readonly SingerConfig auto_f1 = new SingerConfig( "Auto_F1", 1, 1 );
        private static readonly SingerConfig male_hk = new SingerConfig( "Male_HK", 2, 2 );
        private static readonly SingerConfig auto_hk = new SingerConfig( "Auto_HK", 3, 3 );

        public static readonly SingerConfig[] SINGERS = new SingerConfig[] { female_f1, auto_f1, male_hk, auto_hk };

        private static AquesToneDriver mInstance = null;

#if ENABLE_AQUESTONE

        public int haskyParameterIndex = 0;
        public int resonancParameterIndex = 1;
        public int yomiLineParameterIndex = 2;
        public int volumeParameterIndex = 3;
        public int releaseParameterIndex = 4;
        public int portaTimeParameterIndex = 5;
        public int vibFreqParameterIndex = 6;
        public int bendLblParameterIndex = 7;
        public int phontParameterIndex = 8;

        private AquesToneDriver()
        {
        }

        public static void unload()
        {
            if ( mInstance != null ) {
                try {
                    mInstance.close();
                } catch ( Exception ex ) {
                    serr.println( "AquesToneDriver#unload; ex=" + ex );
                }
            }
        }

        public static AquesToneDriver getInstance()
        {
            if ( mInstance == null ) {
                reload();
            }
            return mInstance;
        }

        public static AquesToneDriver getInstance( int sample_rate )
        {
            if ( mInstance == null ) {
                reload( sample_rate );
            } else {
                if ( sample_rate != mInstance.getSampleRate() ) {
                    mInstance.setSampleRate( sample_rate );
                }
            }
            return mInstance;
        }

        public static void reload()
        {
            reload( 44100 );
        }

        public static void reload( int sample_rate )
        {
            String aques_tone = AppManager.editorConfig.PathAquesTone;
            if ( mInstance == null ) {
                mInstance = new AquesToneDriver();
                mInstance.loaded = false;
                mInstance.kind = RendererKind.AQUES_TONE;
            }
            if ( mInstance.loaded ) {
                mInstance.close();
                mInstance.loaded = false;
            }
            mInstance.path = aques_tone;
            if ( !aques_tone.Equals( "" ) && fsys.isFileExists( aques_tone ) && !AppManager.editorConfig.DoNotUseAquesTone ) {
                boolean loaded = false;
                try {
                    loaded = mInstance.open( sample_rate, sample_rate );
                } catch ( Exception ex ) {
                    serr.println( "VSTiProxy#realoadAquesTone; ex=" + ex );
                    loaded = false;
                    Logger.write( typeof( AquesToneDriver ) + ".reload; ex=" + ex + "\n" );
                }
#if DEBUG
                sout.println( "AquesToneDriver#reload(int); loaded=" + loaded + "; sample_rate=" + sample_rate );
#endif
                mInstance.loaded = loaded;
            }

#if DEBUG
            sout.println( "AquesToneDriver#initCor; aquesToneDriver.loaded=" + mInstance.loaded );
#endif
        }

        public override boolean open( int block_size, int sample_rate )
        {
#if DEBUG
            sout.println( "AquesToneDriver#open" );
#endif
            int strlen = 260;
            StringBuilder sb = new StringBuilder( strlen );
            win32.GetProfileString( "AquesTone", "FileKoe_00", "", sb, (uint)strlen );
            String koe_old = sb.ToString();

            String required = getKoeFilePath();
            boolean refresh_winini = false;
            if ( !required.Equals( koe_old ) && !koe_old.Equals( "" ) ) {
                refresh_winini = true;
            }
            win32.WriteProfileString( "AquesTone", "FileKoe_00", required );
            boolean ret = false;
            try {
                ret = base.open( block_size, sample_rate );
            } catch ( Exception ex ) {
                ret = false;
                serr.println( "AquesToneDriver#open; ex=" + ex );
                Logger.write( typeof( AquesToneDriver ) + ".open; ex=" + ex + "\n" );
            }

            if ( refresh_winini ) {
                win32.WriteProfileString( "AquesTone", "FileKoe_00", koe_old );
            }
#if DEBUG
            sout.println( "AquesToneDriver#open; done; ret=" + ret );
#endif
            return ret;
        }

        private static String getKoeFilePath()
        {
            String ret = fsys.combine( AppManager.getCadenciiTempDir(), "jphonefifty.txt" );
            BufferedWriter bw = null;
            try {
                bw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( ret ), "Shift_JIS" ) );
                foreach ( String s in PHONES ) {
                    bw.write( s ); bw.newLine();
                }
            } catch ( Exception ex ) {
                Logger.write( typeof( AquesToneDriver ) + ".getKoeFilePath; ex=" + ex + "\n" );
                serr.println( "AquesToneDriver#getKoeFilePath; ex=" + ex );
            } finally {
                if ( bw != null ) {
                    try {
                        bw.close();
                    } catch ( Exception ex2 ) {
                        Logger.write( typeof( AquesToneDriver ) + ".getKoeFilePath; ex=" + ex2 + "\n" );
                        serr.println( "AquesToneDriver#getKoeFilePath; ex=" + ex2 );
                    }
                }
            }
            return ret;
        }
#endif
    }

#if !JAVA
}
#endif
#endif
