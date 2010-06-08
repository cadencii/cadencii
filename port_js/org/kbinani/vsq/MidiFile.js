/*
 * MidiFile.js
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
if( org.kbinani.vsq.MidiFile == undefined ){

    /**
     * @param stream [ByteArrayInputStream]
     */
    org.kbinani.vsq.MidiFile = function( stream ){
        this.m_events = new Array( new Array() );//private Vector<Vector<MidiEvent>> m_events;
        this.m_format; //int
        this.m_time_format; //int

        // ヘッダ
        var byte4 = new Array( 4 ); //byte[]
        stream.readArray( byte4, 0, 4 );
        alert( byte4 );
        if ( org.kbinani.PortUtil.make_uint32_be( byte4 ) != 0x4d546864 ) {
            alert( "header erro :MThd" );
            return;//throw new Exception( "header error: MThd" );
        }

        // データ長
        stream.readArray( byte4, 0, 4 );
        var length = org.kbinani.PortUtil.make_uint32_be( byte4 );

        // フォーマット
        stream.readArray( byte4, 0, 2 );
        this.m_format = org.kbinani.PortUtil.make_uint16_be( byte4 );

        // トラック数
        var tracks = 0;
        stream.readArray( byte4, 0, 2 );
        tracks = org.kbinani.PortUtil.make_uint16_be( byte4 );

        // 時間分解能
        stream.readArray( byte4, 0, 2 );
        this.m_time_format = org.kbinani.PortUtil.make_uint16_be( byte4 );

        // 各トラックを読込み
        this.m_events = new Array( new Array() );
        for ( var track = 0; track < tracks; track++ ) {
            var track_events = new Array();// Vector<MidiEvent>();
            // ヘッダー
            stream.readArray( byte4, 0, 4 );
            if ( org.kbinani.PortUtil.make_uint32_be( byte4 ) != 0x4d54726b ) {
                return;//throw new Exception( "header error; MTrk" );
            }

            // チャンクサイズ
            stream.readArray( byte4, 0, 4 );
            var size = org.kbinani.PortUtil.make_uint32_be( byte4 );
            var startpos = stream.getFilePointer();

            // チャンクの終わりまで読込み
            var clock = new org.kbinani.ByRef( 0 );
            var last_status_byte = new org.kbinani.ByRef( 0x00 );
            while ( stream.getFilePointer() < startpos + size ) {
                var mi = org.kbinani.vsq.MidiEvent.read( stream, clock, last_status_byte );
                track_events.push( mi );
            }
            if ( this.m_time_format != 480 ) {
                var count = track_events.length;
                for ( var i = 0; i < count; i++ ) {
                    var mi = track_events[i];
                    mi.clock = mi.clock * 480 / this.m_time_format;
                    track_events[i] = mi;
                }
            }
            this.m_events.push( track_events );
        }
        this.m_time_format = 480;
    };

    org.kbinani.vsq.MidiFile.prototype = {
        /**
         * @param track [int]
         * @return [Vector<MidiEvent>]
         */
        getMidiEventList : function( track ){
            if( this.m_events == null ){
                return new Array();
            } else if ( 0 <= track && track < this.m_events.length ) {
                return this.m_events[track];
            } else {
                return new Array();
            }
        },

        /**
         * @return [int]
         */
        getTrackCount : function() {
            if ( this.m_events == null ) {
                return 0;
            } else {
                return this.m_events.length;
            }
        },

        /**
         * @return [void]
         */
        close : function() {
            if ( this.m_events != null ) {
                var c = this.m_events.length;
                for ( var i = 0; i < c; i++ ) {
                    this.m_events[i].clear();
                }
                this.m_events.clear();
            }
        }
    };

}
