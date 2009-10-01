/*
 * MidiFile.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Collections.Generic;

using bocoree;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    /// <summary>
    /// midiイベント。メタイベントは、メタイベントのデータ長をData[1]に格納せず、生のデータをDataに格納するので、注意が必要
    /// </summary>
    public struct MidiEvent : IComparable<MidiEvent> {
        public long clock;
        public byte firstByte;
        public byte[] data;

        public long Clock {
            get {
                return clock;
            }
            set {
                clock = value;
            }
        }

        public byte FirstByte {
            get {
                return firstByte;
            }
            set {
                firstByte = value;
            }
        }

        public byte[] Data {
            get {
                return data;
            }
            set {
                data = value;
            }
        }

        private static void writeDeltaClock( RandomAccessFile stream, long number ) {
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
                uint num = 0;
                uint count = 0x80;
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

        private static long readDeltaClock( RandomAccessFile stream ) {
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

        public static MidiEvent read( RandomAccessFile stream, ref long last_clock, ref byte last_status_byte ) {
            long delta_clock = readDeltaClock( stream );
            last_clock += delta_clock;
            byte first_byte = (byte)stream.read();
            if ( first_byte < 0x80 ) {
                // ランニングステータスが適用される
                long pos = stream.getFilePointer();
                stream.seek( pos - 1 );
                first_byte = last_status_byte;
            } else {
                last_status_byte = first_byte;
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
                me.clock = last_clock;
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
                me.clock = last_clock;
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
                me.clock = last_clock;
                me.firstByte = first_byte;
                me.data = new byte[0];
                return me;
            } else if ( first_byte == 0xff ) {
                // メタイベント
                byte meta_event_type = (byte)stream.read();
                long meta_event_length = readDeltaClock( stream );
                MidiEvent me = new MidiEvent();
                me.clock = last_clock;
                me.firstByte = first_byte;
                me.data = new byte[meta_event_length + 1];
                me.data[0] = meta_event_type;
                stream.read( me.data, 1, (int)meta_event_length );
                return me;
            } else if ( first_byte == 0xf0 ) {
                // f0ステータスのSysEx
                MidiEvent me = new MidiEvent();
                me.clock = last_clock;
                me.firstByte = first_byte;
                long sysex_length = readDeltaClock( stream );
                me.data = new byte[sysex_length + 1];
                stream.read( me.data, 0, (int)(sysex_length + 1) );
                return me;
            } else if ( first_byte == 0xf7 ) {
                // f7ステータスのSysEx
                MidiEvent me = new MidiEvent();
                me.clock = last_clock;
                me.firstByte = first_byte;
                long sysex_length = readDeltaClock( stream );
                me.data = new byte[sysex_length];
                stream.read( me.data, 0, (int)sysex_length );
                return me;
            } else {
                throw new ApplicationException( "don't know how to process first_byte: 0x" + Convert.ToString( first_byte, 16 ) );
            }
        }

        public void writeData( RandomAccessFile stream ) {
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

        public int CompareTo( MidiEvent item ) {
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

        public static MidiEvent generateTimeSigEvent( int clock, int numerator, int denominator ) {
            MidiEvent ret = new MidiEvent();
            ret.clock = clock;
            ret.firstByte = 0xff;
            byte b_numer = (byte)(Math.Log( denominator, 2 ) + 0.1);
#if DEBUG
            Console.WriteLine( "VsqEvent.generateTimeSigEvent; b_number=" + b_numer + "; denominator=" + denominator );
#endif
            ret.data = new byte[5] { 0x58, (byte)numerator, b_numer, 0x18, 0x08 };
            return ret;
        }

        public static MidiEvent generateTempoChangeEvent( int clock, int tempo ) {
            MidiEvent ret = new MidiEvent();
            ret.clock = clock;
            ret.firstByte = 0xff;
            byte b1 = (byte)(tempo & 0xff);
            tempo = tempo >> 8;
            byte b2 = (byte)(tempo & 0xff);
            tempo = tempo >> 8;
            byte b3 = (byte)(tempo & 0xff);
            ret.data = new byte[4] { 0x51, b3, b2, b1 };
            return ret;
        }
    }

    public class MidiFile : IDisposable {
        private Vector<Vector<MidiEvent>> m_events;
        private ushort m_format;
        private ushort m_time_format;

        public MidiFile( String path ){
            RandomAccessFile stream = new RandomAccessFile( path, "r" );
            try {
                // ヘッダ
                byte[] byte4 = new byte[4];
                stream.read( byte4, 0, 4 );
                if ( makeUInt32( byte4 ) != 0x4d546864 ) {
                    throw new ApplicationException( "header error: MThd" );
                }

                // データ長
                stream.read( byte4, 0, 4 );
                uint length = makeUInt32( byte4 );

                // フォーマット
                stream.read( byte4, 0, 2 );
                m_format = makeUint16( byte4 );

                // トラック数
                int tracks = 0;
                stream.read( byte4, 0, 2 );
                tracks = (int)makeUint16( byte4 );

                // 時間分解能
                stream.read( byte4, 0, 2 );
                m_time_format = makeUint16( byte4 );

                // 各トラックを読込み
                m_events = new Vector<Vector<MidiEvent>>();
                for ( int track = 0; track < tracks; track++ ) {
                    // ヘッダー
                    stream.read( byte4, 0, 4 );
                    if ( makeUInt32( byte4 ) != 0x4d54726b ) {
                        throw new ApplicationException( "header error; MTrk" );
                    }
                    m_events.add( new Vector<MidiEvent>() );

                    // チャンクサイズ
                    stream.read( byte4, 0, 4 );
                    long size = (long)makeUInt32( byte4 );
                    long startpos = stream.getFilePointer();

                    // チャンクの終わりまで読込み
                    long clock = 0;
                    byte last_status_byte = 0x00;
                    while ( stream.getFilePointer() < startpos + size ) {
                        MidiEvent mi = MidiEvent.read( stream, ref clock, ref last_status_byte );
                        m_events.get( track ).add( mi );
                    }
                    if ( m_time_format != 480 ) {
                        int count = m_events.get( track ).size();
                        for ( int i = 0; i < count; i++ ) {
                            MidiEvent mi = m_events.get( track ).get( i );
                            mi.clock = mi.clock * 480 / m_time_format;
                            m_events.get( track ).set( i, mi );
                        }
                    }
                }
                m_time_format = 480;
#if DEBUG
                String dbg = PortUtil.combinePath( Path.GetDirectoryName( path ), Path.GetFileNameWithoutExtension( path ) + ".txt" );
                using ( StreamWriter sw = new StreamWriter( dbg ) ) {
                    const String format = "    {0,8} 0x{1:X4} {2,-35} 0x{3:X2} 0x{4:X2}";
                    const String format0 = "    {0,8} 0x{1:X4} {2,-35} 0x{3:X2}";
                    for ( int track = 1; track < m_events.size(); track++ ) {
                        sw.WriteLine( "MidiFile..ctor; track=" + track );
                        byte msb, lsb, data_msb, data_lsb;
                        msb = lsb = data_msb = data_lsb = 0x0;
                        for ( int i = 0; i < m_events.get( track ).size(); i++ ) {
                            if ( m_events.get( track ).get( i ).firstByte == 0xb0 ) {
                                switch ( m_events.get( track ).get( i ).data[0] ) {
                                    case 0x63:
                                        msb = m_events.get( track ).get( i ).data[1];
                                        lsb = 0x0;
                                        break;
                                    case 0x62:
                                        lsb = m_events.get( track ).get( i ).data[1];
                                        break;
                                    case 0x06:
                                        data_msb = m_events.get( track ).get( i ).data[1];
                                        ushort nrpn = (ushort)(msb << 8 | lsb);
                                        String name = NRPN.getName( nrpn );
                                        if ( name.Equals( "" ) ) {
                                            name = "* * UNKNOWN * *";
                                            sw.WriteLine( String.Format( format0, m_events.get( track ).get( i ).clock, nrpn, name, data_msb ) );
                                        } else {
                                            //if ( !NRPN.is_require_data_lsb( nrpn ) ) {
                                            sw.WriteLine( String.Format( format0, m_events.get( track ).get( i ).clock, nrpn, name, data_msb ) );
                                            //}
                                        }
                                        break;
                                    case 0x26:
                                        data_lsb = m_events.get( track ).get( i ).data[1];
                                        ushort nrpn2 = (ushort)(msb << 8 | lsb);
                                        String name2 = NRPN.getName( nrpn2 );
                                        if ( name2.Equals( "" ) ) {
                                            name2 = "* * UNKNOWN * *";
                                        }
                                        sw.WriteLine( String.Format( format, m_events.get( track ).get( i ).clock, nrpn2, name2, data_msb, data_lsb ) );
                                        break;
                                }
                            }
                        }
                    }
                }
#endif
            } catch ( Exception ex ) {
            } finally {
                if ( stream != null ) {
                    try {
                        stream.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        /*public void Write( String path ) {
        }*/

        public Vector<MidiEvent> getMidiEventList( int track ) {
            if ( m_events == null ) {
                return new Vector<MidiEvent>();
            } else if ( 0 <= track && track < m_events.size() ) {
                return m_events.get( track );
            } else {
                return new Vector<MidiEvent>();
            }
        }

        public int getTrackCount() {
            if ( m_events == null ) {
                return 0;
            } else {
                return m_events.size();
            }
        }

        public void Dispose() {
            Close();
        }

        public void Close() {
            if ( m_events != null ) {
                for ( int i = 0; i < m_events.size(); i++ ) {
                    m_events.get( i ).clear();
                }
                m_events.clear();
            }
        }

        private static UInt32 makeUInt32( byte[] value ) {
            return (uint)((uint)((uint)((uint)(value[0] << 8) | value[1]) << 8 | value[2]) << 8 | value[3]);
        }

        private static UInt16 makeUint16( byte[] value ) {
            return (ushort)((ushort)(value[0] << 8) | value[1]);
        }

        private static long readDeltaClock( Stream stream ) {
            byte[] b;
            long ret = 0;
            while ( true ) {
                byte d = (byte)stream.ReadByte();
                ret = (ret << 7) | ((long)d & 0x7f);
                if ( (d & 0x80) == 0x00 ) {
                    break;
                }
            }
            return ret;
        }
    }

}