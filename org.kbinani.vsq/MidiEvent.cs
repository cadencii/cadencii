/*
 * MidiEvent.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.io;

namespace org.kbinani.vsq {
    using boolean = System.Boolean;
    using Long = System.Int64;
#endif

    /// <summary>
    /// midiイベント。メタイベントは、メタイベントのデータ長をData[1]に格納せず、生のデータをDataに格納するので、注意が必要
    /// </summary>
#if JAVA
    public class MidiEvent implements Comparable<MidiEvent> {
#else
    public struct MidiEvent : IComparable<MidiEvent> {
#endif
        public long clock;
        public byte firstByte;
        public byte[] data;

        private static void writeDeltaClock( RandomAccessFile stream, long number )
#if JAVA
            throws IOException
#endif
        {
            boolean[] bits = new boolean[64];
            long val = 0x1;
            bits[0] = (number & val) == val;
            for ( int i = 1; i < 64; i++ ) {
                val = val << 1;
                bits[i] = (number & val) == val;
            }
            int first = 0;
            for ( int i = 63; i >= 0; i-- ) {
                if ( bits[i] ) {
                    first = i;
                    break;
                }
            }
            // 何バイト必要か？
            int bytes = first / 7 + 1;
            for ( int i = 1; i <= bytes; i++ ) {
                long num = 0;
                long count = 0x80;
                for ( int j = (bytes - i + 1) * 7 - 1; j >= (bytes - i + 1) * 7 - 6 - 1; j-- ) {
                    count = count >> 1;
                    if ( bits[j] ) {
                        num += count;
                    }
                }
                if ( i != bytes ) {
                    num += 0x80;
                }
                stream.write( (byte)num );
            }
        }

        private static long readDeltaClock( RandomAccessFile stream )
#if JAVA
            throws IOException
#endif
        {
            long ret = 0;
            while ( true ) {
                int i = stream.read();
                if ( i < 0 ) {
                    break;
                }
                byte d = (byte)i;
                ret = (ret << 7) | ((long)d & 0x7f);
                if ( (d & 0x80) == 0x00 ) {
                    break;
                }
            }
            return ret;
        }

        public static MidiEvent read( RandomAccessFile stream, ByRef<Long> last_clock, ByRef<Byte> last_status_byte )
#if JAVA
            throws IOException, Exception
#endif
        {
            long delta_clock = readDeltaClock( stream );
            last_clock.value += delta_clock;
            byte first_byte = (byte)stream.read();
            if ( first_byte < 0x80 ) {
                // ランニングステータスが適用される
                long pos = stream.getFilePointer();
                stream.seek( pos - 1 );
                first_byte = last_status_byte.value;
            } else {
                last_status_byte.value = first_byte;
            }
            byte ctrl = (byte)(first_byte & (byte)0xf0);
            if ( ctrl == 0x80 || ctrl == 0x90 || ctrl == 0xA0 || ctrl == 0xB0 || ctrl == 0xE0 || first_byte == 0xF2 ) {
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
                me.data = new byte[2];
                stream.read( me.data, 0, 2 );
                return me;
            } else if ( ctrl == 0xC0 || ctrl == 0xD0 || first_byte == 0xF1 || first_byte == 0xF2 ) {
                // 2byte使用するチャンネルメッセージ
                //     0xC*: プログラムチェンジ
                //     0xD*: チャンネルプレッシャ
                // 2byte使用するシステムメッセージ
                //     0xF1: クォータフレーム
                //     0xF3: ソングセレクト
                MidiEvent me = new MidiEvent();
                me.clock = last_clock.value;
                me.firstByte = first_byte;
                me.data = new byte[1];
                stream.read( me.data, 0, 1 );
                return me;
            } else if ( first_byte == 0xF6 ) {
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
                me.data = new byte[0];
                return me;
            } else if ( first_byte == 0xff ) {
                // メタイベント
                byte meta_event_type = (byte)stream.read();
                long meta_event_length = readDeltaClock( stream );
                MidiEvent me = new MidiEvent();
                me.clock = last_clock.value;
                me.firstByte = first_byte;
                me.data = new byte[(int)meta_event_length + 1];
                me.data[0] = meta_event_type;
                stream.read( me.data, 1, (int)meta_event_length );
                return me;
            } else if ( first_byte == 0xf0 ) {
                // f0ステータスのSysEx
                MidiEvent me = new MidiEvent();
                me.clock = last_clock.value;
                me.firstByte = first_byte;
                long sysex_length = readDeltaClock( stream );
                me.data = new byte[(int)sysex_length + 1];
                stream.read( me.data, 0, (int)(sysex_length + 1) );
                return me;
            } else if ( first_byte == 0xf7 ) {
                // f7ステータスのSysEx
                MidiEvent me = new MidiEvent();
                me.clock = last_clock.value;
                me.firstByte = first_byte;
                long sysex_length = readDeltaClock( stream );
                me.data = new byte[(int)sysex_length];
                stream.read( me.data, 0, (int)sysex_length );
                return me;
            } else {
                throw new Exception( "don't know how to process first_byte: 0x" + PortUtil.toHexString( first_byte ) );
            }
        }

        public void writeData( RandomAccessFile stream )
#if JAVA
            throws IOException
#endif
        {
            stream.write( firstByte );
            if ( firstByte == 0xff ) {
                stream.write( data[0] );
                writeDeltaClock( stream, data.Length - 1 );
                //stream.WriteByte( (byte)(Data.Length - 1) );
                stream.write( data, 1, data.Length - 1 );
            } else {
                stream.write( data, 0, data.Length );
            }
        }

        public int compareTo( MidiEvent item ) {
            if ( clock != item.clock ) {
                return (int)(clock - item.clock);
            } else {
                int first_this = firstByte & 0xf0;
                int first_item = item.firstByte & 0xf0;

                if ( (first_this == 0x80 || first_this == 0x90) && (first_item == 0x80 || first_item == 0x90) ) {
                    if ( data != null && data.Length >= 2 && item.data != null && item.data.Length >= 2 ) {
                        if ( first_item == 0x90 && item.data[1] == 0 ) {
                            first_item = 0x80;
                        }
                        if ( first_this == 0x90 && data[1] == 0 ) {
                            first_this = 0x80;
                        }
                        if ( data[0] == item.data[0] ) {
                            if ( first_this == 0x90 ) {
                                if ( first_item == 0x80 ) {
                                    // ON -> OFF
                                    return 1;
                                } else {
                                    // ON -> ON
                                    return 0;
                                }
                            } else {
                                if ( first_item == 0x80 ) {
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

#if !JAVA
        public int CompareTo( MidiEvent item ) {
            return compareTo( item );
        }
#endif

        public static MidiEvent generateTimeSigEvent( int clock, int numerator, int denominator ) {
            MidiEvent ret = new MidiEvent();
            ret.clock = clock;
            ret.firstByte = (byte)0xff;
            byte b_numer = (byte)(Math.Log( denominator ) / Math.Log( 2 ) + 0.1);
#if DEBUG
            PortUtil.println( "VsqEvent.generateTimeSigEvent; b_number=" + b_numer + "; denominator=" + denominator );
#endif
            ret.data = new byte[] { 0x58, (byte)numerator, b_numer, 0x18, 0x08 };
            return ret;
        }

        public static MidiEvent generateTempoChangeEvent( int clock, int tempo ) {
            MidiEvent ret = new MidiEvent();
            ret.clock = clock;
            ret.firstByte = (byte)0xff;
            byte b1 = (byte)(tempo & 0xff);
            tempo = tempo >> 8;
            byte b2 = (byte)(tempo & 0xff);
            tempo = tempo >> 8;
            byte b3 = (byte)(tempo & 0xff);
            ret.data = new byte[] { (byte)0x51, b3, b2, b1 };
            return ret;
        }
    }

#if !JAVA
}
#endif
