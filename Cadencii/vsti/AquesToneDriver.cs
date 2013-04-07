#if ENABLE_AQUESTONE
/*
 * AquesToneDriver.cs
 * Copyright © 2009-2013 kbinani
 *
 * This file is part of com.github.cadencii.
 *
 * com.github.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * com.github.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package com.github.cadencii;

import com.github.cadencii.*;
import com.github.cadencii.vsq.*;

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
    public class AquesToneDriver {
#else
    public class AquesToneDriver : AquesToneDriverBase
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

        public AquesToneDriver( String dllPath ) :
            base( dllPath )
        {
        }

        public override RendererKind getRendererKind()
        {
            return RendererKind.AQUES_TONE;
        }

        public override MidiEvent[] createNoteOnEvent( int note, int dynamics, String phrase )
        {
            // noteon MIDIイベントを作成
            String katakana = KanaDeRomanization.hiragana2katakana( KanaDeRomanization.Attach( phrase ) );
            int index = -1;
            for ( int i = 0; i < AquesToneDriver.PHONES.Length; i++ ) {
                if ( katakana.Equals( AquesToneDriver.PHONES[i] ) ) {
                    index = i;
                    break;
                }
            }

            if ( index < 0 ) {
                return new MidiEvent[] { };
            } else {
                // index行目に移動するコマンドを贈る
                MidiEvent moveline = new MidiEvent();
                moveline.firstByte = 0xb0;
                moveline.data = new[] { 0x0a, index };
                MidiEvent noteon = new MidiEvent();
                noteon.firstByte = 0x90;
                noteon.data = new int[] { note, dynamics };
                return new MidiEvent[] { moveline, noteon };
            }
        }

        public override ParameterEvent[] createSingerEvent( int program )
        {
            if ( 0 > program || program >= SINGERS.Length ) {
                program = 0;
            }
            var singer = new ParameterEvent();
            singer.index = phontParameterIndex;
            singer.value = program + 0.01f;
            return new ParameterEvent[] { singer };
        }

        protected override String[] getKoeFileContents()
        {
            return PHONES;
        }

        protected override String getKoeConfigKey()
        {
            return "FileKoe_00";
        }

        protected override string getConfigSectionKey()
        {
            return "AquesTone";
        }

#endif
    }

#if !JAVA
}
#endif
#endif
