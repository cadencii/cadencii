/*
 * MidiEventEx.java
 * Copyright (c) 2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

import java.io.*;

/**
 * midiイベントを表すクラス。
 * メタイベントは、メタイベントのデータ長をData[1]に格納せず、生のデータをDataに格納するので、注意が必要。
 */
public class MidiEventEx implements Comparable<MidiEventEx>{
    public long clock;
    public int firstByte;
    public byte[] data;

    private static void writeDeltaClock( RandomAccessFile stream, long number ) throws IOException{
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
            int num = 0;
            int count = 0x80;
            for ( int j = (bytes - i + 1) * 7 - 1; j >= (bytes - i + 1) * 7 - 6 - 1; j-- ) {
                count = count >>> 1;
                if ( bits[j] ) {
                    num += count;
                }
            }
            if ( i != bytes ) {
                num += 0x80;
            }
            stream.write( num );
        }
    }

    private static long readDeltaClock( RandomAccessFile stream ) throws IOException{
        long ret = 0;
        while ( true ) {
            int i = stream.read();
            if ( i < 0 ) {
                break;
            }
            byte d = (byte)i;
            ret = (ret << 7) | (d & 0x7f);
            if ( (d & 0x80) == 0x00 ) {
                break;
            }
        }

        return ret;
    }

    public static MidiEventEx read( RandomAccessFile stream, MidiEventParserStatus status ) throws IOException, Exception{
        long delta_clock = readDeltaClock( stream );
        status.lastClock += delta_clock;
        int first_byte = stream.read();
        if ( first_byte < 0x80 ) {
            // ランニングステータスが適用される
            stream.seek( stream.getFilePointer() - 1 );
            first_byte = status.lastStatusByte;
        } else {
            status.lastStatusByte = first_byte;
        }
        int ctrl = first_byte & 0xf0;
        if ( ctrl == 0x80 || ctrl == 0x90 || ctrl == 0xA0 || ctrl == 0xB0 || ctrl == 0xE0 || first_byte == 0xF2 ) {
            // 3byte使用するチャンネルメッセージ：
            //     0x8*: ノートオフ
            //     0x9*: ノートオン
            //     0xA*: ポリフォニック・キープレッシャ
            //     0xB*: コントロールチェンジ
            //     0xE*: ピッチベンドチェンジ
            // 3byte使用するシステムメッセージ
            //     0xF2: ソングポジション・ポインタ
            MidiEventEx me = new MidiEventEx();
            me.clock = status.lastClock;
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
            MidiEventEx me = new MidiEventEx();
            me.clock = status.lastClock;
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
            MidiEventEx me = new MidiEventEx();
            me.clock = status.lastClock;
            me.firstByte = first_byte;
            me.data = new byte[0];
            return me;
        } else if ( first_byte == 0xff ) {
            // メタイベント
            byte meta_event_type = (byte)stream.read();
            int meta_event_length = (int)readDeltaClock( stream );
            MidiEventEx me = new MidiEventEx();
            me.clock = status.lastClock;
            me.firstByte = first_byte;
            me.data = new byte[meta_event_length + 1];
            me.data[0] = meta_event_type;
            stream.read( me.data, 1, (int)meta_event_length );
            return me;
        } else if ( first_byte == 0xf0 ) {
            // f0ステータスのSysEx
            MidiEventEx me = new MidiEventEx();
            me.clock = status.lastClock;
            me.firstByte = first_byte;
            int sysex_length = (int)readDeltaClock( stream );
            me.data = new byte[sysex_length + 1];
            stream.read( me.data, 0, (int)(sysex_length + 1) );
            return me;
        } else if ( first_byte == 0xf7 ) {
            // f7ステータスのSysEx
            MidiEventEx me = new MidiEventEx();
            me.clock = status.lastClock;
            me.firstByte = first_byte;
            int sysex_length = (int)readDeltaClock( stream );
            me.data = new byte[sysex_length];
            stream.read( me.data, 0, (int)sysex_length );
            return me;
        } else {
            throw new Exception( "don't know how to process first_byte: 0x" + Integer.toHexString( first_byte ) );
        }
    }

    public void writeData( RandomAccessFile stream ) throws IOException{
        stream.write( firstByte );
        if ( firstByte == 0xff ) {
            stream.write( data[0] );
            writeDeltaClock( stream, data.length - 1 );
            stream.write( data, 1, data.length - 1 );
        } else {
            stream.write( data, 0, data.length );
        }
    }

    public static MidiEventEx generateTimeSigEvent( int clock_, int numerator, int denominator ) {
        MidiEventEx ret = new MidiEventEx();
        ret.clock = clock_;
        ret.firstByte = 0xff;
        byte b_numer = (byte)(Math.log( denominator ) / Math.log( 2.0 ) + 0.1);
        ret.data = new byte[] { 0x58, (byte)numerator, b_numer, 0x18, 0x08 };
        return ret;
    }

    public static MidiEventEx generateTempoChangeEvent( int clock, int tempo ) {
        MidiEventEx ret = new MidiEventEx();
        ret.clock = clock;
        ret.firstByte = 0xff;
        byte b1 = (byte)(tempo & 0xff);
        tempo = tempo >>> 8;
        byte b2 = (byte)(tempo & 0xff);
        tempo = tempo >>> 8;
        byte b3 = (byte)(tempo & 0xff);
        ret.data = new byte[] { 0x51, b3, b2, b1 };
        return ret;
    }

    public int compareTo( MidiEventEx item ) {
        return (int)(clock - item.clock);
    }
}
