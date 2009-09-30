/*
 * MidiFile.java
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

import java.util.*;
import java.io.*;

public class MidiFile {
    private Vector<Vector<MidiEventEx>> m_events;
    private int m_format;
    private int m_time_format;

    public MidiFile( String path ) throws FileNotFoundException, IOException, Exception{
        RandomAccessFile stream = new RandomAccessFile( path, "r" );
        // ヘッダ
        byte[] byte4 = new byte[4];
        byte[] byte2 = new byte[2];
        stream.read( byte4, 0, 4 );
        if ( makeUInt32( byte4 ) != 0x4d546864 ) {
            throw new Exception( "header error: MThd" );
        }

        // データ長
        stream.read( byte4, 0, 4 );
        int length = makeUInt32( byte4 );

        // フォーマット
        stream.read( byte2, 0, 2 );
        m_format = makeUint16( byte2 );

        // トラック数
        int tracks = 0;
        stream.read( byte2, 0, 2 );
        tracks = (int)makeUint16( byte2 );

        // 時間分解能
        stream.read( byte2, 0, 2 );
        m_time_format = makeUint16( byte2 );

        // 各トラックを読込み
        m_events = new Vector<Vector<MidiEventEx>>();
        for ( int track = 0; track < tracks; track++ ) {
            // ヘッダー
            stream.read( byte4, 0, 4 );
            if ( makeUInt32( byte4 ) != 0x4d54726b ) {
                throw new Exception( "header error; MTrk" );
            }
            m_events.add( new Vector<MidiEventEx>() );

            // チャンクサイズ
            stream.read( byte4, 0, 4 );
            long size = (long)makeUInt32( byte4 );
            long startpos = stream.getFilePointer();

            // チャンクの終わりまで読込み
            long clock = 0;
            byte last_status_byte = 0x00;
            MidiEventParserStatus status = new MidiEventParserStatus();
            status.lastClock = clock;
            status.lastStatusByte = 0;
            while ( stream.getFilePointer() < startpos + size ) {
                MidiEventEx mi = MidiEventEx.read( stream, status );
                m_events.get( track ).add( mi );
            }
            if ( m_time_format != 480 ) {
                int count = m_events.get( track ).size();
                for ( int i = 0; i < count; i++ ) {
                    MidiEventEx mi = m_events.get( track ).get( i );
                    mi.clock = mi.clock * 480 / m_time_format;
                    m_events.get( track ).set( i, mi );
                }
            }
        }
        m_time_format = 480;
        stream.close();
    }

    public Vector<MidiEventEx> getMidiEventList( int track ) {
        if ( m_events == null ) {
            return new Vector<MidiEventEx>();
        } else if ( 0 <= track && track < m_events.size() ) {
            return m_events.get( track );
        } else {
            return new Vector<MidiEventEx>();
        }
    }

    public int getTrackCount() {
        if ( m_events == null ) {
            return 0;
        } else {
            return m_events.size();
        }
    }

    public void close() {
        if ( m_events != null ) {
            for ( int i = 0; i < m_events.size(); i++ ) {
                m_events.get( i ).clear();
            }
            m_events.clear();
        }
    }

    private static int makeUInt32( byte[] value ) {
        int ret = (0xff & value[0]);
        ret = (ret << 8) | (0xff & value[1]);
        ret = (ret << 8) | (0xff & value[2]);
        ret = (ret << 8) | (0xff & value[3]);
        return ret;
    }

    private static int makeUint16( byte[] value ) {
        int ret = (0xff & value[0]);
        return (ret << 8) | (0xff & value[1]);
    }

    private static long readDeltaClock( RandomAccessFile stream ) throws IOException{
        byte[] b;
        long ret = 0;
        while ( true ) {
            byte d = (byte)stream.read();
            ret = (ret << 7) | ((long)d & 0x7f);
            if ( (d & 0x80) == 0x00 ) {
                break;
            }
        }
        return ret;
    }
}
