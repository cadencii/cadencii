/*
 * MidiEvent.cs
 * Copyright © 2009-2011 kbinani
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
using System.IO;
using cadencii;
using cadencii.java.io;

namespace cadencii.vsq
{

    /// <summary>
    /// midiイベント。メタイベントは、メタイベントのデータ長をData[1]に格納せず、生のデータをDataに格納するので、注意が必要
    /// </summary>
    public struct MidiEvent : IComparable<MidiEvent>
    {
        public long clock;
        public int firstByte;
        public int[] data;

        private static void writeDeltaClock(Stream stream, long number)
        {
            bool[] bits = new bool[64];
            long val = 0x1;
            bits[0] = (number & val) == val;
            for (int i = 1; i < 64; i++) {
                val = val << 1;
                bits[i] = (number & val) == val;
            }
            int first = 0;
            for (int i = 63; i >= 0; i--) {
                if (bits[i]) {
                    first = i;
                    break;
                }
            }
            // 何バイト必要か？
            int bytes = first / 7 + 1;
            for (int i = 1; i <= bytes; i++) {
                long num = 0;
                long count = 0x80;
                for (int j = (bytes - i + 1) * 7 - 1; j >= (bytes - i + 1) * 7 - 6 - 1; j--) {
                    count = count >> 1;
                    if (bits[j]) {
                        num += count;
                    }
                }
                if (i != bytes) {
                    num += 0x80;
                }
                stream.WriteByte((byte)num);
            }
        }

        private static long readDeltaClock(Stream stream)
        {
            long ret = 0;
            while (true) {
                int i = stream.ReadByte();
                if (i < 0) {
                    break;
                }
                int d = i;
                ret = (ret << 7) | ((long)d & 0x7f);
                if ((d & 0x80) == 0x00) {
                    break;
                }
            }
            return ret;
        }

        public static MidiEvent read(Stream stream, ByRef<long> last_clock, ByRef<int> last_status_byte)
        {
            long delta_clock = readDeltaClock(stream);
            last_clock.value += delta_clock;
            int first_byte = stream.ReadByte();
            if (first_byte < 0x80) {
                // ランニングステータスが適用される
                long pos = stream.Position;
                stream.Seek(pos - 1, SeekOrigin.Begin);
                first_byte = last_status_byte.value;
            } else {
                last_status_byte.value = first_byte;
            }
            int ctrl = first_byte & 0xf0;
            if (ctrl == 0x80 || ctrl == 0x90 || ctrl == 0xA0 || ctrl == 0xB0 || ctrl == 0xE0 || first_byte == 0xF2) {
                // 3byte使用するチャンネルメッセージ：
                //     0x8*: ノートオフ
                //     0x9*: ノートオン
                //     0xA*: ポリフォニック・キープレッシャ
                //     0xB*: コントロールチェンジ
                //     0xE*: ピッチベンドチェンジ
                // 3byte使用するシステムメッセージ
                //     0xF2: ソングポジション・ポインタ
                MidiEvent me = new MidiEvent();
                me.clock = last_clock.value;
                me.firstByte = first_byte;
                me.data = new int[2];
                byte[] d = new byte[2];
                stream.Read(d, 0, 2);
                for (int i = 0; i < 2; i++) {
                    me.data[i] = 0xff & d[i];
                }
                return me;
            } else if (ctrl == 0xC0 || ctrl == 0xD0 || first_byte == 0xF1 || first_byte == 0xF2) {
                // 2byte使用するチャンネルメッセージ
                //     0xC*: プログラムチェンジ
                //     0xD*: チャンネルプレッシャ
                // 2byte使用するシステムメッセージ
                //     0xF1: クォータフレーム
                //     0xF3: ソングセレクト
                MidiEvent me = new MidiEvent();
                me.clock = last_clock.value;
                me.firstByte = first_byte;
                me.data = new int[1];
                byte[] d = new byte[1];
                stream.Read(d, 0, 1);
                me.data[0] = 0xff & d[0];
                return me;
            } else if (first_byte == 0xF6) {
                // 1byte使用するシステムメッセージ
                //     0xF6: チューンリクエスト
                //     0xF7: エンドオブエクスクルーシブ（このクラスではF0ステータスのSysExの一部として取り扱う）
                //     0xF8: タイミングクロック
                //     0xFA: スタート
                //     0xFB: コンティニュー
                //     0xFC: ストップ
                //     0xFE: アクティブセンシング
                //     0xFF: システムリセット
                MidiEvent me = new MidiEvent();
                me.clock = last_clock.value;
                me.firstByte = first_byte;
                me.data = new int[0];
                return me;
            } else if (first_byte == 0xff) {
                // メタイベント
                int meta_event_type = stream.ReadByte();
                long meta_event_length = readDeltaClock(stream);
                MidiEvent me = new MidiEvent();
                me.clock = last_clock.value;
                me.firstByte = first_byte;
                me.data = new int[(int)meta_event_length + 1];
                me.data[0] = meta_event_type;
                byte[] d = new byte[(int)meta_event_length + 1];
                stream.Read(d, 1, (int)meta_event_length);
                for (int i = 1; i < meta_event_length + 1; i++) {

                    me.data[i] = 0xff & d[i];
                }
                return me;
            } else if (first_byte == 0xf0) {
                // f0ステータスのSysEx
                MidiEvent me = new MidiEvent();
                me.clock = last_clock.value;
                me.firstByte = first_byte;
                long sysex_length = readDeltaClock(stream);
                me.data = new int[(int)sysex_length + 1];
                byte[] d = new byte[(int)sysex_length + 1];
                stream.Read(d, 0, (int)(sysex_length + 1));
                for (int i = 0; i < sysex_length + 1; i++) {
                    me.data[i] = 0xff & d[i];
                }
                return me;
            } else if (first_byte == 0xf7) {
                // f7ステータスのSysEx
                MidiEvent me = new MidiEvent();
                me.clock = last_clock.value;
                me.firstByte = first_byte;
                long sysex_length = readDeltaClock(stream);
                me.data = new int[(int)sysex_length];
                byte[] d = new byte[(int)sysex_length];
                stream.Read(d, 0, (int)sysex_length);
                for (int i = 0; i < sysex_length; i++) {
                    me.data[i] = 0xff & d[i];
                }
                return me;
            } else {
                throw new Exception("don't know how to process first_byte: 0x" + PortUtil.toHexString(first_byte));
            }
        }

        public void writeData(Stream stream)
        {
            stream.WriteByte((byte)firstByte);
            if (firstByte == 0xff) {
                stream.WriteByte((byte)data[0]);
                writeDeltaClock(stream, data.Length - 1);
                for (int i = 1; i < data.Length; i++) {
                    stream.WriteByte((byte)data[i]);
                }
            } else {
                for (int i = 0; i < data.Length; i++) {
                    stream.WriteByte((byte)data[i]);
                }
            }
        }

        public int compareTo(MidiEvent item)
        {
            if (clock != item.clock) {
                return (int)(clock - item.clock);
            } else {
                int first_this = firstByte & 0xf0;
                int first_item = item.firstByte & 0xf0;

                if ((first_this == 0x80 || first_this == 0x90) && (first_item == 0x80 || first_item == 0x90)) {
                    if (data != null && data.Length >= 2 && item.data != null && item.data.Length >= 2) {
                        if (first_item == 0x90 && item.data[1] == 0) {
                            first_item = 0x80;
                        }
                        if (first_this == 0x90 && data[1] == 0) {
                            first_this = 0x80;
                        }
                        if (data[0] == item.data[0]) {
                            if (first_this == 0x90) {
                                if (first_item == 0x80) {
                                    // ON -> OFF
                                    return 1;
                                } else {
                                    // ON -> ON
                                    return 0;
                                }
                            } else {
                                if (first_item == 0x80) {
                                    // OFF -> OFF
                                    return 0;
                                } else {
                                    // OFF -> ON
                                    return -1;
                                }
                            }
                        }
                    }
                }
                return (int)(clock - item.clock);
            }
        }

        public int CompareTo(MidiEvent item)
        {
            return compareTo(item);
        }

        public static MidiEvent generateTimeSigEvent(int clock, int numerator, int denominator)
        {
            MidiEvent ret = new MidiEvent();
            ret.clock = clock;
            ret.firstByte = 0xff;
            int b_numer = (int)(Math.Log(denominator) / Math.Log(2) + 0.1);
#if DEBUG
            sout.println("VsqEvent.generateTimeSigEvent; b_number=" + b_numer + "; denominator=" + denominator);
#endif
            ret.data = new int[] { 0x58, numerator, b_numer, 0x18, 0x08 };
            return ret;
        }

        public static MidiEvent generateTempoChangeEvent(int clock, int tempo)
        {
            MidiEvent ret = new MidiEvent();
            ret.clock = clock;
            ret.firstByte = 0xff;
            int b1 = tempo & 0xff;
            tempo = tempo >> 8;
            int b2 = tempo & 0xff;
            tempo = tempo >> 8;
            int b3 = tempo & 0xff;
            ret.data = new int[] { 0x51, b3, b2, b1 };
            return ret;
        }
    }

}
