#if ENABLE_AQUESTONE
/*
 * AquesToneDriver.cs
 * Copyright (c) 2009 kbinani
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
using System;
using System.Text;
using bocoree;
using bocoree.java.io;
using VstSdk;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    public class AquesToneDriver : vstidrv {
        public System.Windows.Forms.Form pluginUi = null;

        private static String[] phones = new String[] { 
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

        public override boolean open( string dll_path, int block_size, int sample_rate ) {
            int strlen = 260;
            StringBuilder sb = new StringBuilder( strlen );
            win32.GetProfileString( "AquesTone", "FileKoe_00", "", sb, (uint)strlen );
            String koe_old = sb.ToString();

            String required = getKoeFilePath();
            boolean refresh_winini = false;
            if ( !required.Equals( koe_old ) && !koe_old.Equals( "" ) ) {
                refresh_winini = true;
                win32.WriteProfileString( "AquesTone", "FileKoe_00", required );
            }
            boolean ret = false;
            try {
                ret = base.open( dll_path, block_size, sample_rate );
            } catch ( Exception ex ) {
                ret = false;
#if DEBUG
                PortUtil.stderr.println( "AquesToneDriver#open; ex=" + ex );
#endif
            }

            try {
                pluginUi = new System.Windows.Forms.Form();
                pluginUi.Text = "AquesToneWindow";
                pluginUi.ClientSize = new System.Drawing.Size( 373, 158 );
                pluginUi.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                pluginUi.Location = new System.Drawing.Point( int.MinValue, int.MinValue );
                pluginUi.Show();
                pluginUi.Refresh();
                pluginUi.Hide();
                pluginUi.Location = new System.Drawing.Point( 0, 0 );
                unsafe {
                    aEffect.Dispatch( ref aEffect, AEffectOpcodes.effEditOpen, 0, 0, (void*)pluginUi.Handle.ToPointer(), 0.0f );
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AquesToneDriver#open; ex=" + ex );
            }

            if ( refresh_winini ) {
#if DEBUG
                PortUtil.println( "AquesToneDriver#open; refresh_winini; koe_old=" + koe_old );
#endif
                win32.WriteProfileString( "AquesTone", "FileKoe_00", koe_old );
            }
            return ret;
        }

        public override void close() {
            if ( pluginUi != null ) {
                pluginUi.Close();
            }
            base.close();
        }

        private static String getKoeFilePath() {
            String ret = PortUtil.combinePath( AppManager.getCadenciiTempDir(), "jphonefifty.txt" );
            if ( !PortUtil.isFileExists( ret ) ) {
                BufferedWriter bw = null;
                try {
                    bw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( ret ), "Shift_JIS" ) );
                    foreach ( String s in phones ) {
                        bw.write( s ); bw.newLine();
                    }
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "AquesToneDriver#getKoeFilePath; ex=" + ex );
                } finally {
                    if ( bw != null ) {
                        try {
                            bw.close();
                        } catch ( Exception ex2 ) {
                            PortUtil.stderr.println( "AquesToneDriver#getKoeFilePath; ex=" + ex2 );
                        }
                    }
                }
            }
            return ret;
        }
    }

}
#endif
