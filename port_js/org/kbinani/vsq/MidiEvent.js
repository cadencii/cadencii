/*
 * MidiEvent.js
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
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};

if( org.kbinani.vsq.MidiEvent == undefined ){

    org.kbinani.vsq.MidiEvent = function(){
        this.clock = 0;
        this.firstByte = 0;
        this.data = new Array();
    };

    /*
     * @param clock [int]
     * @param numerator [int]
     * @param denominator [int]
     * @return [MidiEvent]
     */
    org.kbinani.vsq.MidiEvent.generateTimeSigEvent = function( clock, numerator, denominator ){
        var ret = new org.kbinani.vsq.MidiEvent();
        ret.clock = clock;
        ret.firstByte = 0xff;
        var b_numer = Math.floor( Math.log( denominator ) / Math.log( 2 ) + 0.1 );
        ret.data = new Array( 0x58, numerator, b_numer, 0x18, 0x08 );
        return ret;
    };

    /**
     * @param clock [int]
     * @param tempo [int]
     * @return [MidiEvent]
     */
    org.kbinani.vsq.MidiEvent.generateTempoChangeEvent = function( clock, tempo ) {
        var ret = new MidiEvent();
        ret.clock = clock;
        ret.firstByte = 0xff;
        var b1 = tempo & 0xff;
        tempo = tempo >> 8;
        var b2 = tempo & 0xff;
        tempo = tempo >> 8;
        var b3 = tempo & 0xff;
        ret.data = new Array( 0x51, b3, b2, b1 );
        return ret;
    };

    org.kbinani.vsq.MidiEvent.writeDeltaClock = function( stream, number ){
        var bits = new Array( 64 ); // boolean[]
        var val = 0x1; // [long]
        bits[0] = (number & val) == val;
        for ( var i = 1; i < 64; i++ ) {
            val = val << 1;
            bits[i] = (number & val) == val;
        }
        var first = 0;
        for ( var i = 63; i >= 0; i-- ) {
            if ( bits[i] ) {
                first = i;
                break;
            }
        }
        // 何バイト必要か？
        var bytes = first / 7 + 1; // [int]
        for ( var i = 1; i <= bytes; i++ ) {
            var num = 0; // [long]
            var count = 0x80; // [long]
            for ( var j = (bytes - i + 1) * 7 - 1; j >= (bytes - i + 1) * 7 - 6 - 1; j-- ) {
                count = count >>> 1;
                if ( bits[j] ) {
                    num += count;
                }
            }
            if ( i != bytes ) {
                num += 0x80;
            }
            stream.writeByte( num );
        }
    };

    /**
     * @param stream [ByteArrayInputStream]
     * @return [long]
     */
    org.kbinani.vsq.MidiEvent.readDeltaClock = function( stream ){
        var ret = 0; // [long]
        while ( true ) {
            var i = stream.read();
            if ( i < 0 ) {
                break;
            }
            var d = i; // [byte]
            ret = (ret << 7) | (d & 0x7f);
            if ( (d & 0x80) == 0x00 ) {
                break;
            }
        }
        return ret;
    };

    /**
     * @param stream [ByteArrayInputStream]
     * @param last_clock [ByRef<Long>]
     * @param last_status_byte [ByRef<Integer>]
     */
    org.kbinani.vsq.MidiEvent.read = function( stream, last_clock, last_status_byte ){
        var delta_clock = this.readDeltaClock( stream ); // [long]
        last_clock.value += delta_clock;
        var first_byte = stream.read(); // [int]
        if ( first_byte < 0x80 ) {
            // ランニングステータスが適用される
            var pos = stream.getFilePointer();
            stream.seek( pos - 1 );
            first_byte = last_status_byte.value;
        } else {
            last_status_byte.value = first_byte;
        }
        var ctrl = first_byte & 0xf0;
        if ( ctrl == 0x80 || ctrl == 0x90 || ctrl == 0xA0 || ctrl == 0xB0 || ctrl == 0xE0 || first_byte == 0xF2 ) {
            // 3byte使用するチャンネルメッセージ：
            //     0x8*: ノートオフ
            //     0x9*: ノートオン
            //     0xA*: ポリフォニック・キープレッシャ
            //     0xB*: コントロールチェンジ
            //     0xE*: ピッチベンドチェンジ
            // 3byte使用するシステムメッセージ
            //     0xF2: ソングポジション・ポインタ
            var me = new org.kbinani.vsq.MidiEvent(); // [MidiEvent]
            me.clock = last_clock.value;
            me.firstByte = first_byte;
            me.data = new Array( 2 ); //int[2];
            var d = new Array( 2 ); // byte[2];
            stream.readArray( d, 0, 2 );
            for ( var i = 0; i < 2; i++ ) {
                me.data[i] = 0xff & d[i];
            }
            return me;
        } else if ( ctrl == 0xC0 || ctrl == 0xD0 || first_byte == 0xF1 || first_byte == 0xF2 ) {
            // 2byte使用するチャンネルメッセージ
            //     0xC*: プログラムチェンジ
            //     0xD*: チャンネルプレッシャ
            // 2byte使用するシステムメッセージ
            //     0xF1: クォータフレーム
            //     0xF3: ソングセレクト
            var me = new org.kbinani.vsq.MidiEvent(); // [MidiEvent]
            me.clock = last_clock.value;
            me.firstByte = first_byte;
            me.data = new Array( 1 );// int[1];
            var d = new Array( 1 );// byte[1];
            stream.readArray( d, 0, 1 );
            me.data[0] = 0xff & d[0];
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
            var me = new org.kbinani.vsq.MidiEvent(); // [MidiEvent]
            me.clock = last_clock.value;
            me.firstByte = first_byte;
            me.data = new Array(); //int[0];
            return me;
        } else if ( first_byte == 0xff ) {
            // メタイベント
            var meta_event_type = stream.read(); //[int]
            var meta_event_length = this.readDeltaClock( stream ); // [long]
            var me = new org.kbinani.vsq.MidiEvent(); //[MidiEvent]
            me.clock = last_clock.value;
            me.firstByte = first_byte;
            me.data = new Array( meta_event_length + 1 ); // int[]
            me.data[0] = meta_event_type;
            var d = new Array( meta_event_length + 1 ); // byte[]
            stream.readArray( d, 1, meta_event_length );
            for ( var i = 1; i < meta_event_length + 1; i++ ) {
                me.data[i] = 0xff & d[i];
            }
            return me;
        } else if ( first_byte == 0xf0 ) {
            // f0ステータスのSysEx
            var me = new org.kbinani.vsq.MidiEvent();// [MidiEvent]
            me.clock = last_clock.value;
            me.firstByte = first_byte;
            var sysex_length = this.readDeltaClock( stream ); // [long]
            me.data = new Array( sysex_length + 1 ); // int[]
            var d = new Array( sysex_length + 1 ); // byte[]
            stream.readArray( d, 0, sysex_length + 1 );
            for ( var i = 0; i < sysex_length + 1; i++ ) {
                me.data[i] = 0xff & d[i];
            }
            return me;
        } else if ( first_byte == 0xf7 ) {
            // f7ステータスのSysEx
            var me = new org.kbinani.vsq.MidiEvent();
            me.clock = last_clock.value;
            me.firstByte = first_byte;
            var sysex_length = this.readDeltaClock( stream );
            me.data = new Array( sysex_length );
            var d = new Array( sysex_length );//byte[]
            stream.readArray( d, 0, sysex_length );
            for ( var i = 0; i < sysex_length; i++ ) {
                me.data[i] = 0xff & d[i];
            }
            return me;
        } else {
            throw new Exception( "don't know how to process first_byte: 0x" + PortUtil.toHexString( first_byte ) );
        }
    };

    /// <summary>
    /// midiイベント。メタイベントは、メタイベントのデータ長をData[1]に格納せず、生のデータをDataに格納するので、注意が必要
    /// </summary>
    org.kbinani.vsq.MidiEvent.prototype = {
        /**
         * @param stream [RandomAccessFile]
         */
        writeData : function( stream ){
            stream.write( firstByte );
            if ( firstByte == 0xff ) {
                stream.write( data[0] );
                this.writeDeltaClock( stream, data.length - 1 );
                for ( var i = 1; i < data.length; i++ ) {
                    stream.writeByte( data[i] );
                }
                //stream.write( data, 1, data.Length - 1 );
            } else {
                for ( var i = 0; i < data.length; i++ ) {
                    stream.writeByte( data[i] );
                }
                //stream.write( data, 0, data.Length );
            }
        },

        /**
         * @param item [MidiEvent]
         * @return [int]
         */
        compareTo : function( item ) {
            if ( clock != item.clock ) {
                return clock - item.clock;
            } else {
                var first_this = firstByte & 0xf0;
                var first_item = item.firstByte & 0xf0;

                if ( (first_this == 0x80 || first_this == 0x90) && (first_item == 0x80 || first_item == 0x90) ) {
                    if ( data != null && data.length >= 2 && item.data != null && item.data.length >= 2 ) {
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
                return clock - item.clock;
            }
        },
    };

}
