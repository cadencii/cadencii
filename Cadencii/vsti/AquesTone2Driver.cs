/*
 * AquesTone2Driver.cs
 * Copyright © 2013 kbinani
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
#if ENABLE_AQUESTONE

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using cadencii.vsq;
using cadencii.java.util;

namespace cadencii
{
    using Integer = System.Int32;

    /// <summary>
    /// AquesTone2 用 VSTi ドライバ
    /// </summary>
    public class AquesTone2Driver : AquesToneDriverBase
    {
        /// <summary>
        /// koe ファイル内での、音素の格納位置を表すクラス
        /// </summary>
        private class SyllablePosition
        {
            /// <summary>
            /// 行番号。0 から始まる
            /// </summary>
            public int lineIndex;

            /// <summary>
            /// カラム番号。0 から始まる
            /// </summary>
            public int columnIndex;

            /// <summary>
            /// 行番号、カラム番号を指定して初期化する
            /// </summary>
            /// <param name="lineIndex">行番号</param>
            /// <param name="columnIndex">カラム番号</param>
            public SyllablePosition( int lineIndex, int columnIndex )
            {
                this.lineIndex = lineIndex;
                this.columnIndex = columnIndex;
            }
        }

        /// <summary>
        /// koe ファイルに記録するデータ。1 行ずつのデータの配列となっている
        /// </summary>
        private static readonly String[] koeFileContents;

        /// <summary>
        /// AquesTone の音素が、koe ファイルの何行目何カラムに保存されているかを保持したマップ
        /// キーが音素、値は行インデックス, カラムインデックスを保持した SyllablePosition
        /// </summary>
        private static readonly Dictionary<string, SyllablePosition> syllableMap;

        private static readonly SingerConfig lina_ = new SingerConfig( "Lina", 0, 0 );

        private static readonly List<SingerConfig> singers_ = new List<SingerConfig>() { lina_ };

        static AquesTone2Driver()
        {
            koeFileContents = new String[] {
                "bya", "bye", "byo", "byu", "chi", "cya", "cye", "cyo", "cyu", "dhi", "dhu", "dwu",
                "gya", "gye", "gyo", "gyu", "hya", "hye", "hyo", "hyu", "kya", "kye", "kyo", "kyu",
                "mya", "mye", "myo", "myu", "nya nyan", "nye", "nyo", "nyu", "rya", "rye", "ryo",
                "ryu", "swi", "sya", "sye", "syo", "syu", "thi", "thu", "tsa", "tse", "tsi", "tso",
                "twu", "zwi", "ba", "be", "bi", "bo", "bu", "da", "de", "do", "ha fa", "he fe",
                "hi fi", "ho fo", "fu", "ga", "ge", "gi", "go", "gu", "ja", "je", "ji", "jo", "ju",
                "ka", "ke", "ki", "ko", "ku", "ma", "me", "mi", "mo", "mu", "na", "ne", "ni", "no",
                "nu", "pa", "pe", "pi", "po", "pu", "ra", "re", "ri", "ro", "ru", "sa", "se", "si",
                "so", "su", "ta", "te", "to", "tu", "va", "ve", "vi", "vo", "vu", "wa", "we", "wi",
                "ya", "ye", "yo", "yu", "za", "ze", "zo", "zu","a", "e", "i", "n", "o wo", "u" };

            syllableMap = new Dictionary<string, SyllablePosition>();
            for( int i = 0; i < koeFileContents.Length; ++i) {
                string line = koeFileContents[i];
                if ( line.Contains( " " ) ) {
                    string[] splitted = line.Split( new char[] { ' ' } );
                    for ( int j = 0; j < splitted.Length; ++j ) {
                        syllableMap.Add( splitted[j], new SyllablePosition( i, j ) );
                    }
                } else {
                    syllableMap.Add( line, new SyllablePosition( i, 0 ) );
                }
            }
        }

        /// <summary>
        /// AquesTone2 DLL のパスを指定してドライバを初期化する
        /// </summary>
        /// <param name="dllPath">AquesTone2 VSTi DLL のパス</param>
        public AquesTone2Driver( String dllPath )
            : base( dllPath )
        {
        }

        /// <summary>
        /// 合成エンジンの種類を取得する
        /// </summary>
        /// <returns>合成エンジンの種類</returns>
        public override RendererKind getRendererKind()
        {
            return RendererKind.AQUES_TONE2;
        }

        /// <summary>
        /// Note On のための MIDI イベント列を作成する
        /// </summary>
        /// <param name="note">ノート番号</param>
        /// <param name="dynamics">Dynamics</param>
        /// <param name="phrase">歌詞</param>
        /// <returns>Note On のための MIDI イベント列</returns>
        public MidiEvent[] createNoteOnEvent( int note, int dynamics, String phrase )
        {
            var matcher = new SyllableMatcher();
            var syllable = matcher.find( phrase );
            if ( syllableMap.ContainsKey( syllable ) ) {
                var position = syllableMap[syllable];
                int lineIndex = position.lineIndex;
                int columnIndex = position.columnIndex;

                var result = new List<MidiEvent>();

                {
                    MidiEvent moveLine = new MidiEvent();
                    moveLine.firstByte = 0xB0;
                    moveLine.data = new[] { 0x31, lineIndex };
                    result.Add( moveLine );
                }
                for ( int i = 1; i <= columnIndex; ++i ) {
                    {
                        MidiEvent dummyNoteOn = new MidiEvent();
                        dummyNoteOn.firstByte = 0x90;
                        dummyNoteOn.data = new int[] { note, 0x40 };
                        result.Add( dummyNoteOn );
                    }
                    {
                        MidiEvent dummyNoteOff = new MidiEvent();
                        dummyNoteOff.firstByte = 0x80;
                        dummyNoteOff.data = new int[] { note, 0x40 };
                        result.Add( dummyNoteOff );
                    }
                }
                {
                    MidiEvent noteOn = new MidiEvent();
                    noteOn.firstByte = 0x90;
                    noteOn.data = new int[] { note, dynamics };
                    result.Add( noteOn );
                }
                return result.ToArray();
            } else {
                return new MidiEvent[] { };
            }
        }

        /// <summary>
        /// 歌手変更のためのイベントを作成する
        /// </summary>
        /// <param name="program">プログラムチェンジ</param>
        /// <returns>イベント</returns>
        private ParameterEvent[] createSingerEvent( int program )
        {
            return new ParameterEvent[] { };
        }

        protected override String[] getKoeFileContents()
        {
            return koeFileContents;
        }

        protected override String getConfigSectionKey()
        {
            return "AquesTone2";
        }

        protected override String getKoeConfigKey()
        {
            return "FileKoe";
        }

        /// <summary>
        /// プログラムチェンジの値から、該当する歌手設定を取得する
        /// </summary>
        /// <param name="program_change">プログラムチェンジ</param>
        /// <returns>歌手設定。該当する歌手設定がなければ null を返す</returns>
        public static SingerConfig getSingerConfig( int program_change )
        {
            return singers_.FirstOrDefault( ( singer_config ) => singer_config.Program == program_change );
        }

        /// <summary>
        /// 歌手情報を列挙する
        /// </summary>
        public static IEnumerable<SingerConfig> Singers
        {
            get { return singers_; }
        }
    }

}

#endif
