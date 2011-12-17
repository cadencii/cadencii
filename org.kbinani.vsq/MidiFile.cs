/*
 * MidiFile.cs
 * Copyright © 2009-2011 kbinani
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
#define MIDI_PRINT_TO_FILE
#if JAVA
package org.kbinani.vsq;

import java.io.*;
import java.util.*;
import org.kbinani.*;
#else
using System;
using com.github.cadencii;
using com.github.cadencii.java.util;
using com.github.cadencii.java.io;

namespace com.github.cadencii.vsq
{
    using boolean = System.Boolean;
    using Long = System.Int64;
    using Integer = System.Int32;
#endif

    public class MidiFile
    {
        private Vector<Vector<MidiEvent>> m_events;
        private int m_format;
        private int m_time_format;

        public MidiFile( String path )
#if JAVA
            throws FileNotFoundException
#endif
        {
            RandomAccessFile stream = new RandomAccessFile( path, "r" );
            try {
                // ヘッダ
                byte[] byte4 = new byte[4];
                stream.read( byte4, 0, 4 );
                if ( PortUtil.make_uint32_be( byte4 ) != 0x4d546864 ) {
                    throw new Exception( "header error: MThd" );
                }

                // データ長
                stream.read( byte4, 0, 4 );
                long length = PortUtil.make_uint32_be( byte4 );

                // フォーマット
                stream.read( byte4, 0, 2 );
                m_format = PortUtil.make_uint16_be( byte4 );

                // トラック数
                int tracks = 0;
                stream.read( byte4, 0, 2 );
                tracks = (int)PortUtil.make_uint16_be( byte4 );

                // 時間分解能
                stream.read( byte4, 0, 2 );
                m_time_format = PortUtil.make_uint16_be( byte4 );

                // 各トラックを読込み
                m_events = new Vector<Vector<MidiEvent>>();
                for ( int track = 0; track < tracks; track++ ) {
                    Vector<MidiEvent> track_events = new Vector<MidiEvent>();
                    // ヘッダー
                    stream.read( byte4, 0, 4 );
                    if ( PortUtil.make_uint32_be( byte4 ) != 0x4d54726b ) {
                        throw new Exception( "header error; MTrk" );
                    }

                    // チャンクサイズ
                    stream.read( byte4, 0, 4 );
                    long size = (long)PortUtil.make_uint32_be( byte4 );
                    long startpos = stream.getFilePointer();

                    // チャンクの終わりまで読込み
                    ByRef<Long> clock = new ByRef<Long>( (long)0 );
                    ByRef<Integer> last_status_byte = new ByRef<Integer>( 0x00 );
                    while ( stream.getFilePointer() < startpos + size ) {
                        MidiEvent mi = MidiEvent.read( stream, clock, last_status_byte );
                        track_events.add( mi );
                    }
                    if ( m_time_format != 480 ) {
                        int count = track_events.size();
                        for ( int i = 0; i < count; i++ ) {
                            MidiEvent mi = track_events.get( i );
                            mi.clock = mi.clock * 480 / m_time_format;
                            track_events.set( i, mi );
                        }
                    }
                    m_events.add( track_events );
                }
                m_time_format = 480;
#if DEBUG && MIDI_PRINT_TO_FILE && !JAVA
                String dbg = fsys.combine( PortUtil.getDirectoryName( path ), PortUtil.getFileNameWithoutExtension( path ) + ".txt" );
                BufferedWriter sw = null;
                try {
                    sw = new BufferedWriter( new FileWriter( dbg ) );
                    const String format = "    {0,8} 0x{1:X4} {2,-35} 0x{3:X2} 0x{4:X2}";
                    const String format0 = "    {0,8} 0x{1:X4} {2,-35} 0x{3:X2}";
                    for ( int track = 1; track < m_events.size(); track++ ) {
                        sw.write( "MidiFile..ctor; track=" + track );
                        sw.newLine();
                        byte msb, lsb, data_msb, data_lsb;
                        msb = lsb = data_msb = data_lsb = 0x0;
                        for ( int i = 0; i < m_events.get( track ).size(); i++ ) {
                            if ( m_events.get( track ).get( i ).firstByte == 0xb0 ) {
                                switch ( m_events.get( track ).get( i ).data[0] ) {
                                    case 0x63:
                                        msb = (byte)(0xff & m_events.get( track ).get( i ).data[1]);
                                        lsb = 0x0;
                                        break;
                                    case 0x62:
                                        lsb = (byte)(0xff & m_events.get( track ).get( i ).data[1]);
                                        break;
                                    case 0x06:
                                        data_msb = (byte)(0xff & m_events.get( track ).get( i ).data[1]);
                                        ushort nrpn = (ushort)(msb << 8 | lsb);
                                        String name = NRPN.getName( nrpn );
                                        if ( name.Equals( "" ) ) {
                                            name = "* * UNKNOWN * *";
                                            sw.write( String.Format( format0, m_events.get( track ).get( i ).clock, nrpn, name, data_msb ) );
                                            sw.newLine();
                                        } else {
                                            //if ( !NRPN.is_require_data_lsb( nrpn ) ) {
                                            sw.write( String.Format( format0, m_events.get( track ).get( i ).clock, nrpn, name, data_msb ) );
                                            sw.newLine();
                                            //}
                                        }
                                        break;
                                    case 0x26:
                                        data_lsb = (byte)(0xff & m_events.get( track ).get( i ).data[1]);
                                        ushort nrpn2 = (ushort)(msb << 8 | lsb);
                                        String name2 = NRPN.getName( nrpn2 );
                                        if ( name2.Equals( "" ) ) {
                                            name2 = "* * UNKNOWN * *";
                                        }
                                        sw.write( String.Format( format, m_events.get( track ).get( i ).clock, nrpn2, name2, data_msb, data_lsb ) );
                                        sw.newLine();
                                        break;
                                }
                            }
                        }
                    }
                } catch ( Exception ex ) {
                } finally {
                    if ( sw != null ) {
                        try {
                            sw.close();
                        } catch ( Exception ex2 ) {
                        }
                    }
                }
#endif
            } catch ( Exception ex ) {
                serr.println( "MidiFile#.ctor; ex=" + ex );
            } finally {
                if ( stream != null ) {
                    try {
                        stream.close();
                    } catch ( Exception ex2 ) {
                        serr.println( "MidiFile#.ctor; ex2=" + ex2 );
                    }
                }
            }
        }

        public Vector<MidiEvent> getMidiEventList( int track )
        {
            if ( m_events == null ) {
                return new Vector<MidiEvent>();
            } else if ( 0 <= track && track < m_events.size() ) {
                return m_events.get( track );
            } else {
                return new Vector<MidiEvent>();
            }
        }

        public int getTrackCount()
        {
            if ( m_events == null ) {
                return 0;
            } else {
                return m_events.size();
            }
        }

        public void close()
        {
            if ( m_events != null ) {
                int c = m_events.size();
                for ( int i = 0; i < c; i++ ) {
                    m_events.get( i ).clear();
                }
                m_events.clear();
            }
        }
    }

#if !JAVA
}
#endif
