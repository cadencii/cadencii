/*
 * VsqNote.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace cadencii.vsq
{

    /// <summary>
    /// 音階を表現するためのクラス
    /// </summary>
    [Serializable]
    public class VsqNote
    {
        /// <summary>
        /// このインスタンスが表す音階のノート値
        /// </summary>
        public int Value;
        private static readonly int[] ALTER = new int[] { 0, 1, 0, -1, 0, 0, 1, 0, 1, 0, -1, 0, 0 };
        private static readonly bool[] _KEY_TYPE = new bool[] { 
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
        };

        /// <summary>
        /// 音階のノート値からのコンストラクタ。
        /// </summary>
        /// <param name="note">この音階を初期化するためのノート値</param>
        public VsqNote(int note)
        {
            Value = note;
        }

        /// <summary>
        /// このインスタンスが表す音階が、ピアノの白鍵かどうかを返します
        /// </summary>
        public bool isWhiteKey()
        {
            return isNoteWhiteKey(Value);
        }

        /// <summary>
        /// 指定した音階が、ピアノの白鍵かどうかを返します
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static bool isNoteWhiteKey(int note)
        {
            if (0 <= note && note <= 127) {
                return _KEY_TYPE[note];
            } else {
                int odd = note % 12;
                switch (odd) {
                    case 1:
                    case 3:
                    case 6:
                    case 8:
                    case 10:
                    return false;
                    default:
                    return true;
                }
            }
        }

        /// <summary>
        /// C#4なら+1, C4なら0, Cb4なら-1
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static int getNoteAlter(int note)
        {
            return ALTER[note % 12];
        }

        /// <summary>
        /// ノート#のオクターブ部分の表記を調べます．
        /// 例：C4 => 4, D#4 => 4
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static int getNoteOctave(int note)
        {
            int odd = note % 12;
            return (note - odd) / 12 - 2;
        }

        /// <summary>
        /// ノートのオクターブ，変化記号を除いた部分の文字列表記を調べます．
        /// 例：C4 => "C", D#4 => "D"
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static string getNoteStringBase(int note)
        {
            int odd = note % 12;
            switch (odd) {
                case 0:
                case 1:
                return "C";
                case 2:
                return "D";
                case 3:
                case 4:
                return "E";
                case 5:
                case 6:
                return "F";
                case 7:
                case 8:
                return "G";
                case 9:
                return "A";
                case 10:
                case 11:
                return "B";
                default:
                return "";
            }
        }

        public static string getNoteString(int note)
        {
            int odd = note % 12;
            int order = (note - odd) / 12 - 2;
            switch (odd) {
                case 0:
                return "C" + order;
                case 1:
                return "C#" + order;
                case 2:
                return "D" + order;
                case 3:
                return "Eb" + order;
                case 4:
                return "E" + order;
                case 5:
                return "F" + order;
                case 6:
                return "F#" + order;
                case 7:
                return "G" + order;
                case 8:
                return "G#" + order;
                case 9:
                return "A" + order;
                case 10:
                return "Bb" + order;
                case 11:
                return "B" + order;
                default:
                return "";
            }
        }

        public override string ToString()
        {
            return toString();
        }

        public string toString()
        {
            return getNoteString(Value);
        }
    }

}
