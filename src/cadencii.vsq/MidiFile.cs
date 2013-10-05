/*
 * MidiFile.cs
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
#define MIDI_PRINT_TO_FILE
using System;
using System.IO;
using System.Collections.Generic;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii.vsq
{

    public class MidiFile
    {
        private List<List<MidiEvent>> m_events;
        private int m_format;
        private int m_time_format;

        public MidiFile(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            try {
                // ヘッダ
                byte[] byte4 = new byte[4];
                stream.Read(byte4, 0, 4);
                if (PortUtil.make_uint32_be(byte4) != 0x4d546864) {
                    throw new Exception("header error: MThd");
                }

                // データ長
                stream.Read(byte4, 0, 4);
                long length = PortUtil.make_uint32_be(byte4);

                // フォーマット
                stream.Read(byte4, 0, 2);
                m_format = PortUtil.make_uint16_be(byte4);

                // トラック数
                int tracks = 0;
                stream.Read(byte4, 0, 2);
                tracks = (int)PortUtil.make_uint16_be(byte4);

                // 時間分解能
                stream.Read(byte4, 0, 2);
                m_time_format = PortUtil.make_uint16_be(byte4);

                // 各トラックを読込み
                m_events = new List<List<MidiEvent>>();
                for (int track = 0; track < tracks; track++) {
                    List<MidiEvent> track_events = new List<MidiEvent>();
                    // ヘッダー
                    stream.Read(byte4, 0, 4);
                    if (PortUtil.make_uint32_be(byte4) != 0x4d54726b) {
                        throw new Exception("header error; MTrk");
                    }

                    // チャンクサイズ
                    stream.Read(byte4, 0, 4);
                    long size = (long)PortUtil.make_uint32_be(byte4);
                    long startpos = stream.Position;

                    // チャンクの終わりまで読込み
                    ByRef<long> clock = new ByRef<long>((long)0);
                    ByRef<int> last_status_byte = new ByRef<int>(0x00);
                    while (stream.Position < startpos + size) {
                        MidiEvent mi = MidiEvent.read(stream, clock, last_status_byte);
                        track_events.Add(mi);
                    }
                    if (m_time_format != 480) {
                        int count = track_events.Count;
                        for (int i = 0; i < count; i++) {
                            MidiEvent mi = track_events[i];
                            mi.clock = mi.clock * 480 / m_time_format;
                            track_events[i] = mi;
                        }
                    }
                    m_events.Add(track_events);
                }
                m_time_format = 480;
#if DEBUG && MIDI_PRINT_TO_FILE
                string dbg = Path.Combine(PortUtil.getDirectoryName(path), PortUtil.getFileNameWithoutExtension(path) + ".txt");
                StreamWriter sw = null;
                try {
                    sw = new StreamWriter(dbg);
                    const string format = "    {0,8} 0x{1:X4} {2,-35} 0x{3:X2} 0x{4:X2}";
                    const string format0 = "    {0,8} 0x{1:X4} {2,-35} 0x{3:X2}";
                    for (int track = 1; track < m_events.Count; track++) {
                        sw.WriteLine("MidiFile..ctor; track=" + track);
                        byte msb, lsb, data_msb, data_lsb;
                        msb = lsb = data_msb = data_lsb = 0x0;
                        for (int i = 0; i < m_events[track].Count; i++) {
                            if (m_events[track][i].firstByte == 0xb0) {
                                switch (m_events[track][i].data[0]) {
                                    case 0x63:
                                    msb = (byte)(0xff & m_events[track][i].data[1]);
                                    lsb = 0x0;
                                    break;
                                    case 0x62:
                                    lsb = (byte)(0xff & m_events[track][i].data[1]);
                                    break;
                                    case 0x06:
                                    data_msb = (byte)(0xff & m_events[track][i].data[1]);
                                    ushort nrpn = (ushort)(msb << 8 | lsb);
                                    string name = NRPN.getName(nrpn);
                                    if (name.Equals("")) {
                                        name = "* * UNKNOWN * *";
                                        sw.WriteLine(string.Format(format0, m_events[track][i].clock, nrpn, name, data_msb));
                                    } else {
                                        //if ( !NRPN.is_require_data_lsb( nrpn ) ) {
                                        sw.WriteLine(string.Format(format0, m_events[track][i].clock, nrpn, name, data_msb));
                                        //}
                                    }
                                    break;
                                    case 0x26:
                                    data_lsb = (byte)(0xff & m_events[track][i].data[1]);
                                    ushort nrpn2 = (ushort)(msb << 8 | lsb);
                                    string name2 = NRPN.getName(nrpn2);
                                    if (name2.Equals("")) {
                                        name2 = "* * UNKNOWN * *";
                                    }
                                    sw.WriteLine(string.Format(format, m_events[track][i].clock, nrpn2, name2, data_msb, data_lsb));
                                    break;
                                }
                            }
                        }
                    }
                } catch (Exception ex) {
                } finally {
                    if (sw != null) {
                        try {
                            sw.Close();
                        } catch (Exception ex2) {
                        }
                    }
                }
#endif
            } catch (Exception ex) {
                serr.println("MidiFile#.ctor; ex=" + ex);
            } finally {
                if (stream != null) {
                    try {
                        stream.Close();
                    } catch (Exception ex2) {
                        serr.println("MidiFile#.ctor; ex2=" + ex2);
                    }
                }
            }
        }

        public List<MidiEvent> getMidiEventList(int track)
        {
            if (m_events == null) {
                return new List<MidiEvent>();
            } else if (0 <= track && track < m_events.Count) {
                return m_events[track];
            } else {
                return new List<MidiEvent>();
            }
        }

        public int getTrackCount()
        {
            if (m_events == null) {
                return 0;
            } else {
                return m_events.Count;
            }
        }

        public void close()
        {
            if (m_events != null) {
                int c = m_events.Count;
                for (int i = 0; i < c; i++) {
                    m_events[i].Clear();
                }
                m_events.Clear();
            }
        }
    }

}
