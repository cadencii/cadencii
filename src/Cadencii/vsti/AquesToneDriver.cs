#if ENABLE_AQUESTONE
/*
 * AquesToneDriver.cs
 * Copyright © 2009-2013 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using cadencii;
using cadencii.java.io;
using cadencii.vsq;



namespace cadencii
{

    public class AquesToneDriver : AquesToneDriverBase
    {
        private static readonly string[] PHONES = new string[] { 
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
        private static readonly SingerConfig female_f1 = new SingerConfig("Female_F1", 0, 0);
        private static readonly SingerConfig auto_f1 = new SingerConfig("Auto_F1", 1, 1);
        private static readonly SingerConfig male_hk = new SingerConfig("Male_HK", 2, 2);
        private static readonly SingerConfig auto_hk = new SingerConfig("Auto_HK", 3, 3);

        private static readonly SingerConfig[] SINGERS = new SingerConfig[] { female_f1, auto_f1, male_hk, auto_hk };

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

        public AquesToneDriver(string dllPath) :
            base(dllPath)
        {
        }

        public override RendererKind getRendererKind()
        {
            return RendererKind.AQUES_TONE;
        }

        /// <summary>
        /// Note On のための MIDI イベント列を作成する
        /// </summary>
        /// <param name="note">ノート番号</param>
        /// <param name="dynamics">Dynamics</param>
        /// <param name="phrase">歌詞</param>
        /// <returns>Note On のための MIDI イベント列</returns>
        public MidiEvent[] createNoteOnEvent(int note, int dynamics, string phrase)
        {
            // noteon MIDIイベントを作成
            string katakana = KanaDeRomanization.hiragana2katakana(KanaDeRomanization.Attach(phrase));
            int index = -1;
            for (int i = 0; i < AquesToneDriver.PHONES.Length; i++) {
                if (katakana.Equals(AquesToneDriver.PHONES[i])) {
                    index = i;
                    break;
                }
            }

            if (index < 0) {
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

        /// <summary>
        /// 歌手変更のためのイベントを作成する
        /// </summary>
        /// <param name="program">プログラムチェンジ</param>
        /// <returns>イベント</returns>
        public ParameterEvent[] createSingerEvent(int program)
        {
            if (0 > program || program >= SINGERS.Length) {
                program = 0;
            }
            var singer = new ParameterEvent();
            singer.index = phontParameterIndex;
            singer.value = program + 0.01f;
            return new ParameterEvent[] { singer };
        }

        protected override string[] getKoeFileContents()
        {
            return PHONES;
        }

        protected override string getKoeConfigKey()
        {
            return "FileKoe_00";
        }

        protected override string getConfigSectionKey()
        {
            return "AquesTone";
        }

        /// <summary>
        /// プログラムチェンジの値から、該当する歌手設定を取得する
        /// </summary>
        /// <param name="program_change">プログラムチェンジ</param>
        /// <returns>歌手設定。該当する歌手設定がなければ null を返す</returns>
        public static SingerConfig getSingerConfig(int program_change)
        {
            return SINGERS.FirstOrDefault((singer_config) => singer_config.Program == program_change);
        }

        /// <summary>
        /// 歌手情報を列挙する
        /// </summary>
        public static IEnumerable<SingerConfig> Singers
        {
            get { return SINGERS; }
        }
#endif
    }

}
#endif
