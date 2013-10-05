/*
 * UstFile.cs
 * Copyright © 2009-2011 kbinani, PEX
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
using System.Collections.Generic;
using System.IO;
using System.Text;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii.vsq
{

    public class UstFile : ICloneable
    {
        /// <summary>
        /// [#PREV]が指定されているUstEventのIndex
        /// </summary>
        public const int PREV_INDEX = int.MinValue;
        /// <summary>
        /// [#NEXT]が指定されているUstEventのIndex
        /// </summary>
        public const int NEXT_INDEX = int.MaxValue;

        public Object Tag;
        private float m_tempo = 120.00f;
        private string m_project_name = "";
        private string m_voice_dir = "";
        private string m_out_file = "";
        private string m_cache_dir = "";
        private string m_tool1 = "";
        private string m_tool2 = "";
        private List<UstTrack> m_tracks = new List<UstTrack>();
        private List<TempoTableEntry> m_tempo_table;

        public UstFile(string path)
        {
            StreamReader sr = null;
            try {
                sr = new StreamReader(path, Encoding.GetEncoding("Shift_JIS"));
#if DEBUG
                sout.println("UstFile#.ctor; path=" + path);
                sout.println("UstFile#.ctor; (sr==null)=" + (sr == null));
#endif
                string line = sr.ReadLine();

                UstTrack track = new UstTrack();
                int type = 0; //0 => reading "SETTING" section
                while (true) {
#if DEBUG
                    sout.println("UstFile#.ctor; line=" + line);
#endif
                    UstEvent ue = null;
                    if (type == 1) {
                        ue = new UstEvent();
                    }
                    int index = 0;
                    if (line == "[#TRACKEND]") {
                        break;
                    } else if (line.ToUpper() == "[#NEXT]") {
                        index = NEXT_INDEX;
                    } else if (line.ToUpper() == "[#PREV]") {
                        index = PREV_INDEX;
                    } else if (line.ToUpper() == "[#SETTING]") {
                        type = 0;
                    } else {
                        if (type != 1) {
                            ue = new UstEvent();
                            type = 1;
                        }
                        string s = line.Replace("[#", "").Replace("]", "").Trim();
                        try {
                            index = int.Parse(s);
                        } catch (Exception ex) {
#if DEBUG
                            sout.println("UstFile#.ctor; ex=" + ex);
#endif
                        }
                    }
#if DEBUG
                    sout.println("UstFile#.ctor; index=" + index);
#endif
                    line = sr.ReadLine(); // "[#" 直下の行
                    if (line == null) {
                        break;
                    }
                    while (!line.StartsWith("[#")) {
#if DEBUG
                        sout.println("line=" + line);
#endif
                        string[] spl = PortUtil.splitString(line, new char[] { '=' }, 2);
                        if (type == 0) {
                            // reading "SETTING" section
                            if (spl[0] == "Tempo") {
                                m_tempo = 125f;
                                float v = 125f;
                                try {
                                    v = (float)double.Parse(spl[1]);
                                    m_tempo = v;
                                } catch (Exception ex) {
                                }
                            } else if (spl[0] == "ProjectName") {
                                m_project_name = spl[1];
                            } else if (spl[0] == "VoiceDir") {
                                m_voice_dir = spl[1];
                            } else if (spl[0] == "OutFile") {
                                m_out_file = spl[1];
                            } else if (spl[0] == "CacheDir") {
                                m_cache_dir = spl[1];
                            } else if (spl[0] == "Tool1") {
                                m_tool1 = spl[1];
                            } else if (spl[0] == "Tool2") {
                                m_tool2 = spl[1];
                            }
                        } else if (type == 1) {
                            if (spl.Length >= 2) {
                                // readin event section
                                if (spl[0] == "Length") {
                                    ue.setLength(0);
                                    int v = 0;
                                    try {
                                        v = int.Parse(spl[1]);
                                        ue.setLength(v);
                                    } catch (Exception ex) {
                                    }
                                } else if (spl[0] == "Lyric") {
                                    ue.setLyric(spl[1]);
                                } else if (spl[0] == "NoteNum") {
                                    int v = 0;
                                    try {
                                        v = int.Parse(spl[1]);
                                        ue.setNote(v);
                                    } catch (Exception ex) {
                                    }
                                } else if (spl[0] == "Intensity") {
                                    int v = 100;
                                    try {
                                        v = int.Parse(spl[1]);
                                        ue.setIntensity(v);
                                    } catch (Exception ex) {
                                    }
                                } else if (spl[0] == "PBType") {
                                    int v = 5;
                                    try {
                                        v = int.Parse(spl[1]);
                                        ue.setPBType(v);
                                    } catch (Exception ex) {
                                    }
                                } else if (spl[0] == "Piches") {
                                    string[] spl2 = PortUtil.splitString(spl[1], ',');
                                    float[] t = new float[spl2.Length];
                                    for (int i = 0; i < spl2.Length; i++) {
                                        float v = 0;
                                        try {
                                            v = (float)double.Parse(spl2[i]);
                                            t[i] = v;
                                        } catch (Exception ex) {
                                        }
                                    }
                                    ue.setPitches(t);
                                } else if (spl[0] == "Tempo") {
                                    float v;
                                    try {
                                        v = (float)double.Parse(spl[1]);
                                        ue.setTempo(v);
                                    } catch (Exception ex) {
                                    }
                                } else if (spl[0] == "VBR") {
                                    ue.setVibrato(new UstVibrato(line));
                                    /*
                                    PBW=50,50,46,48,56,50,50,50,50
                                    PBS=-87
                                    PBY=-15.9,-20,-31.5,-26.6
                                    PBM=,s,r,j,s,s,s,s,s
                                    */
                                } else if (spl[0] == "PBW" || spl[0] == "PBS" || spl[0] == "PBY" || spl[0] == "PBM") {
                                    if (ue.getPortamento() == null) {
                                        ue.setPortamento(new UstPortamento());
                                    }
                                    ue.getPortamento().parseLine(line);
                                } else if (spl[0] == "Envelope") {
                                    ue.setEnvelope(new UstEnvelope(line));
                                    //PreUtterance=1
                                    //VoiceOverlap=6
                                } else if (spl[0] == "VoiceOverlap") {
                                    if (spl[1] != "") {
                                        ue.setVoiceOverlap((float)double.Parse(spl[1]));
                                    }
                                } else if (spl[0] == "PreUtterance") {
                                    if (spl[1] != "") {
                                        ue.setPreUtterance((float)double.Parse(spl[1]));
                                    }
                                } else if (spl[0] == "Flags") {
                                    ue.Flags = line.Substring(6);
                                } else if (spl[0] == "StartPoint") {
                                    try {
                                        float stp = (float)double.Parse(spl[1]);
                                        ue.setStartPoint(stp);
                                    } catch (Exception ex) {
                                    }
                                } else if (spl[0] == "Moduration") {
                                    try {
                                        int moduration = int.Parse(spl[1]);
                                        ue.setModuration(moduration);
                                    } catch (Exception ex) {
                                    }
                                } else {
                                    string name = spl[0];
                                    string value = spl[1];
                                    if (ue.Properties == null) {
                                        ue.Properties = new List<UstEventProperty>();
                                    }
                                    ue.Properties.Add(new UstEventProperty(name, value));
                                }
                            }
                        }
                        line = sr.ReadLine();
                        if (line == null) {
                            break;
                        }
                    }
                    if (type == 0) {
                        type = 1;
                    } else if (type == 1) {
                        ue.Index = index;
                        track.addEvent(ue);
                    }
                }
                m_tracks.Add(track);
                updateTempoInfo();
            } catch (Exception ex) {
#if DEBUG
                serr.println("UstFile#.ctor(String); ex=" + ex);
#endif
            } finally {
                if (sr != null) {
                    try {
                        sr.Close();
                    } catch (Exception ex2) {
                    }
                }
            }
        }

        public UstFile(VsqFile vsq, int track_index)
            :
 this(vsq, track_index, new SortedDictionary<int, int>())
        {
        }

        /// <summary>
        /// vsqの指定したトラックを元に，トラックを1つだけ持つustを構築します
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track_index"></param>
        /// <param name="id_map">UstEventのIndexフィールドと、元になったVsqEventのInternalIDを対応付けるマップ。キーがIndex、値がInternalIDを表す</param>
        public UstFile(VsqFile vsq, int track_index, SortedDictionary<int, int> id_map)
        {
            VsqFile work = (VsqFile)vsq.clone();
            //work.removePart( 0, work.getPreMeasureClocks() );

            VsqTrack vsq_track = work.Track[track_index];

            // デフォルトのテンポ
            if (work.TempoTable.Count <= 0) {
                m_tempo = 120.0f;
            } else {
                m_tempo = (float)(60e6 / (double)work.TempoTable[0].Tempo);
            }
            m_tempo_table = new List<TempoTableEntry>();
            m_tempo_table.Clear();
            // ustには、テンポチェンジを音符の先頭にしか入れられない
            // あとで音符に反映させるためのテンプレートを作っておく
            TempoVector tempo = new TempoVector();
            int last_clock = 0;
            int itempo = (int)(60e6 / m_tempo);
            foreach (var item in vsq_track.getNoteEventIterator()) {
                if (last_clock < item.Clock) {
                    // 休符Rの分
                    tempo.Add(new TempoTableEntry(last_clock, itempo, work.getSecFromClock(last_clock)));
                }
                tempo.Add(new TempoTableEntry(item.Clock, itempo, work.getSecFromClock(item.Clock)));
                last_clock = item.Clock + item.ID.getLength();
            }
            if (tempo.Count == 0) {
                tempo.Add(new TempoTableEntry(0, (int)(60e6 / m_tempo), 0.0));
            }
            // tempoの中の各要素の時刻が、vsq.TempoTableから計算した時刻と合致するよう調節
#if DEBUG
            sout.println("UstFile#.ctor; before; list=");
            for (int i = 0; i < tempo.Count; i++) {
                TempoTableEntry item = tempo[i];
                sout.println("    #" + i + "; c" + item.Clock + "; T" + item.Tempo + "; t" + (60e6 / item.Tempo) + "; sec" + item.Time);
            }
#endif
            TempoTableEntry prev = tempo[0];
            for (int i = 1; i < tempo.Count; i++) {
                TempoTableEntry item = tempo[i];
                double sec = item.Time - prev.Time;
                int delta = item.Clock - prev.Clock;
                // deltaクロックでsecを表現するにはテンポをいくらにすればいいか？
                int draft = (int)(480.0 * sec * 1e6 / (double)delta);
                // 丸め誤差が入るので、Timeを更新
                // ustに実際に記録されるテンポはいくらか？
                float act_tempo = (float)double.Parse(PortUtil.formatDecimal("0.00", 60e6 / draft));
                int i_act_tempo = (int)(60e6 / act_tempo);
                prev.Tempo = i_act_tempo;
                item.Time = prev.Time + 1e-6 * delta * prev.Tempo / 480.0;
                prev = item;
            }
#if DEBUG
            sout.println("UstFile#.ctor; after; list=");
            for (int i = 0; i < tempo.Count; i++) {
                TempoTableEntry item = tempo[i];
                sout.println("    #" + i + "; c" + item.Clock + "; T" + item.Tempo + "; t" + (60e6 / item.Tempo) + "; sec" + item.Time);
            }
            sout.println("UstFile#.ctor; vsq.TempoTable=");
            for (int i = 0; i < work.TempoTable.Count; i++) {
                TempoTableEntry item = work.TempoTable[i];
                sout.println("    #" + i + "; c" + item.Clock + "; T" + item.Tempo + "; t" + (60e6 / item.Tempo) + "; sec" + item.Time);
            }
#endif

            // R用音符のテンプレート
            int PBTYPE = 5;
            UstEvent template = new UstEvent();
            template.setLyric("R");
            template.setNote(60);
            template.setPreUtterance(0);
            template.setVoiceOverlap(0);
            template.setIntensity(100);
            template.setModuration(0);

            // 再生秒時をとりあえず無視して，ゲートタイム基準で音符を追加
            UstTrack track_add = new UstTrack();
            last_clock = 0;
            int index = 0;
            foreach (var item in vsq_track.getNoteEventIterator()) {
                if (last_clock < item.Clock) {
                    // ゲートタイム差あり，Rを追加
                    UstEvent itemust = (UstEvent)template.clone();
                    itemust.setLength(item.Clock - last_clock);
                    itemust.Index = index;
                    index++;
                    id_map[itemust.Index] = -1;
                    track_add.addEvent(itemust);
                }
                UstEvent item_add = (UstEvent)item.UstEvent.clone();
                item_add.setLength(item.ID.getLength());
                item_add.setLyric(item.ID.LyricHandle.L0.Phrase);
                item_add.setNote(item.ID.Note);
                item_add.Index = index;
                id_map[item_add.Index] = item.InternalID;
                if (item.UstEvent.getEnvelope() != null) {
                    item_add.setEnvelope((UstEnvelope)item.UstEvent.getEnvelope().clone());
                }
                index++;
                track_add.addEvent(item_add);
                last_clock = item.Clock + item.ID.getLength();
            }

            // テンポを格納(イベント数はあっているはず)
            if (track_add.getEventCount() > 0) {
                int size = track_add.getEventCount();
                int lasttempo = -1; // ありえない値にしておく
                for (int i = 0; i < size; i++) {
                    TempoTableEntry item = tempo[i];
                    if (lasttempo != item.Tempo) {
                        // テンポ値が変わっているもののみ追加
                        UstEvent ue = track_add.getEvent(i);
                        ue.setTempo((float)(60e6 / item.Tempo));
                        lasttempo = item.Tempo;
                        m_tempo_table.Add(item);
                    }
                }
            } else {
                // tempoはどうせ破棄されるのでクローンしなくていい
                m_tempo_table.Add(tempo[0]);
            }

            // ピッチを反映
            // まず絶対ピッチを取得
            VsqBPList abs_pit = new VsqBPList("", 600000, 0, 1280000);
            VsqBPList cpit = vsq_track.getCurve("pit");
            int clock = 0;
            int search_indx = 0;
            int pit_size = cpit.size();
            foreach (var item in track_add.getNoteEventIterator()) {
                int c = clock;
                int len = item.getLength();
                clock += len;
                if (item.getLyric() == "R") {
                    continue;
                }
                // 音符の先頭のpitは必ず入れる
                abs_pit.add(c, (int)(item.getNote() * 10000 + vsq_track.getPitchAt(c) * 100));

                // c～c+lenまで
                for (int i = search_indx; i < pit_size; i++) {
                    int c2 = cpit.getKeyClock(i);
                    if (c < c2 && c2 < clock) {
                        abs_pit.add(c2, (int)(item.getNote() * 10000 + vsq_track.getPitchAt(c2) * 100));
                        search_indx = i;
                    } else if (clock <= c2) {
                        break;
                    }
                }
            }

            // ピッチをピッチベンドに変換しながら反映
            clock = 0;
            foreach (var item in track_add.getNoteEventIterator()) {
                double sec_at_clock = tempo.getSecFromClock(clock);
                double sec_pre = item.getPreUtterance() / 1000.0;
                double sec_stp = item.getStartPoint() / 1000.0;
                double sec_at_begin = sec_at_clock - sec_pre - sec_stp;
                int clock_begin = (int)tempo.getClockFromSec(sec_at_begin);
                // 音符先頭との距離がPBTYPEの倍数になるようにする
                clock_begin -= (clock < clock_begin) ? ((clock_begin - clock) % PBTYPE)
                                                     : ((clock - clock_begin) % PBTYPE);
                // clock_beginがsec_at_beginより前方になるとNGなので修正する
                double sec_at_clock_begin = tempo.getSecFromClock(clock_begin);
                while (sec_at_clock_begin < sec_at_begin) {
                    clock_begin += PBTYPE;
                    sec_at_clock_begin = tempo.getSecFromClock(clock_begin);
                }
                int clock_end = clock + item.getLength();
                List<float> pitch = new List<float>();
                bool allzero = true;
                ByRef<int> ref_indx = new ByRef<int>(0);
                for (int cl = clock_begin; cl < clock_end; cl += PBTYPE) {
                    int abs = abs_pit.getValue(cl, ref_indx);
                    float pit = (float)(abs / 100.0) - item.getNote() * 100;
                    if (pit != 0.0) {
                        allzero = false;
                    }
                    pitch.Add(pit);
                }
                if (!allzero) {
                    item.setPitches(PortUtil.convertFloatArray(pitch.ToArray()));
                    item.setPBType(PBTYPE);
                } else {
                    item.setPBType(-1);
                }
                clock += item.getLength();
            }

            m_tracks.Add(track_add);
        }

        private UstFile()
        {
        }

        public void setWavTool(string value)
        {
            m_tool1 = value;
        }

        public string getWavTool()
        {
            return m_tool1;
        }

        public void setResampler(string value)
        {
            m_tool2 = value;
        }

        public string getResampler()
        {
            return m_tool2;
        }

        public void setVoiceDir(string value)
        {
            m_voice_dir = value;
        }

        public string getVoiceDir()
        {
            return m_voice_dir;
        }

        public string getProjectName()
        {
            return m_project_name;
        }

        public int getBaseTempo()
        {
            return (int)(6e7 / m_tempo);
        }

        public double getTotalSec()
        {
            int max = 0;
            for (int track = 0; track < m_tracks.Count; track++) {
                int count = 0;
                for (int i = 0; i < m_tracks[track].getEventCount(); i++) {
                    count += (int)m_tracks[track].getEvent(i).getLength();
                }
                max = Math.Max(max, count);
            }
            return getSecFromClock(max);
        }

        public List<TempoTableEntry> getTempoList()
        {
            return m_tempo_table;
        }

        public UstTrack getTrack(int track)
        {
            return m_tracks[track];
        }

        public int getTrackCount()
        {
            return m_tracks.Count;
        }

        /// <summary>
        /// TempoTableの[*].Timeの部分を更新します
        /// </summary>
        /// <returns></returns>
        public void updateTempoInfo()
        {
            if (m_tempo_table == null) {
                m_tempo_table = new List<TempoTableEntry>();
            } else {
                m_tempo_table.Clear();
            }
            if (m_tracks.Count <= 0) {
                return;
            }
            int clock = 0;
            double time = 0.0;
            int last_tempo_clock = 0;  //最後にTempo値が代入されていたイベントのクロック
            float last_tempo = m_tempo;   //最後に代入されていたテンポの値
            UstTrack ust_track = m_tracks[0];
            for (int i = 0; i < ust_track.getEventCount(); i++) {
                UstEvent itemi = ust_track.getEvent(i);
                if (ust_track.getEvent(i).isTempoSpecified()) {
                    time += (clock - last_tempo_clock) / (8.0 * last_tempo);
                    if (m_tempo_table.Count == 0 && clock != 0) {
                        m_tempo_table.Add(new TempoTableEntry(0, (int)(6e7 / m_tempo), 0.0));
                    }
                    m_tempo_table.Add(new TempoTableEntry(clock, (int)(6e7 / itemi.getTempo()), time));
                    last_tempo = itemi.getTempo();
                    last_tempo_clock = clock;
                }
                clock += (int)itemi.getLength();
            }
        }

        /// <summary>
        /// 指定したクロックにおける、clock=0からの演奏経過時間(sec)
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public double getSecFromClock(int clock)
        {
            int c = m_tempo_table.Count;
            for (int i = c - 1; i >= 0; i--) {
                TempoTableEntry item = m_tempo_table[i];
                if (item.Clock < clock) {
                    double init = item.Time;
                    int dclock = clock - item.Clock;
                    double sec_per_clock1 = item.Tempo * 1e-6 / 480.0;
                    return init + dclock * sec_per_clock1;
                }
            }
            double sec_per_clock = 0.125 / m_tempo;
            return clock * sec_per_clock;
        }

        public void write(string file)
        {
            UstFileWriteOptions opt = new UstFileWriteOptions();
            opt.settingCacheDir = true;
            opt.settingOutFile = true;
            opt.settingProjectName = true;
            opt.settingTempo = true;
            opt.settingTool1 = true;
            opt.settingTool2 = true;
            opt.settingTracks = true;
            opt.settingVoiceDir = true;
            opt.trackEnd = true;
            write(file, opt);
        }

        public void write(string file, bool print_track_end)
        {
            UstFileWriteOptions opt = new UstFileWriteOptions();
            opt.settingCacheDir = true;
            opt.settingOutFile = true;
            opt.settingProjectName = true;
            opt.settingTempo = true;
            opt.settingTool1 = true;
            opt.settingTool2 = true;
            opt.settingTracks = true;
            opt.settingVoiceDir = true;
            opt.trackEnd = print_track_end;
            write(file, opt);
        }

        public void write(string file, UstFileWriteOptions options)
        {
            InternalStreamWriter sw = null;
            try {
                sw = new InternalStreamWriter(file, "Shift_JIS");
                sw.write("[#SETTING]"); sw.newLine();
                if (options.settingTempo) {
                    sw.write("Tempo=" + m_tempo); sw.newLine();
                }
                if (options.settingTracks) {
                    sw.write("Tracks=1"); sw.newLine();
                }
                if (options.settingProjectName) {
                    sw.write("ProjectName=" + m_project_name); sw.newLine();
                }
                if (options.settingVoiceDir) {
                    sw.write("VoiceDir=" + m_voice_dir); sw.newLine();
                }
                if (options.settingOutFile) {
                    sw.write("OutFile=" + m_out_file); sw.newLine();
                }
                if (options.settingCacheDir) {
                    sw.write("CacheDir=" + m_cache_dir); sw.newLine();
                }
                if (options.settingTool1) {
                    sw.write("Tool1=" + m_tool1); sw.newLine();
                }
                if (options.settingTool2) {
                    sw.write("Tool2=" + m_tool2); sw.newLine();
                }
                UstTrack target = m_tracks[0];
                int count = target.getEventCount();
                for (int i = 0; i < count; i++) {
                    target.getEvent(i).print(sw);
                }
                if (options.trackEnd) {
                    sw.write("[#TRACKEND]");
                }
                sw.newLine();
            } catch (Exception ex) {
                serr.println("UstFile#write; ex=" + ex);
            } finally {
                if (sw != null) {
                    try {
                        sw.close();
                    } catch (Exception ex2) {
                        serr.println("UstFile#write; ex2=" + ex2);
                    }
                }
            }
        }

        public Object clone()
        {
            UstFile ret = new UstFile();
            ret.m_tempo = m_tempo;
            ret.m_project_name = m_project_name;
            ret.m_voice_dir = m_voice_dir;
            ret.m_out_file = m_out_file;
            ret.m_cache_dir = m_cache_dir;
            ret.m_tool1 = m_tool1;
            ret.m_tool2 = m_tool2;
            int size = m_tracks.Count;
            ret.m_tracks = new List<UstTrack>();
            for (int i = 0; i < size; i++) {
                ret.m_tracks.Add((UstTrack)m_tracks[i].clone());
            }
            ret.m_tempo_table = new List<TempoTableEntry>();
            for (int i = 0; i < m_tempo_table.Count; i++) {
                ret.m_tempo_table.Add((TempoTableEntry)m_tempo_table[i].clone());
            }
            return ret;
        }

        public object Clone()
        {
            return clone();
        }
    }

}
