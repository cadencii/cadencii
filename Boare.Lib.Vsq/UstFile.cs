/*
 * UstFile.cs
 * Copyright (c) 2009 kbinani, PEX
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

import java.util.*;
import java.io.*;
import org.kbinani.*;
#else
using System;
using bocoree;
using bocoree.java.util;
using bocoree.java.io;

namespace org.kbinani.vsq {
    using Float = System.Single;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class UstFile implements Cloneable {
#else
    public class UstFile : ICloneable {
#endif
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
        private String m_project_name = "";
        private String m_voice_dir = "";
        private String m_out_file = "";
        private String m_cache_dir = "";
        private String m_tool1 = "";
        private String m_tool2 = "";
        private Vector<UstTrack> m_tracks = new Vector<UstTrack>();
        private Vector<TempoTableEntry> m_tempo_table;

        public UstFile( String path ) {
            BufferedReader sr = null;
            try {
                sr = new BufferedReader( new InputStreamReader( new FileInputStream( path ), "Shift_JIS" ) );
#if DEBUG
                bocoree.debug.push_log( "path=" + path );
                bocoree.debug.push_log( "(sr==null)=" + (sr == null) );
#endif
                String line = sr.readLine();
                if ( !line.Equals( "[#SETTING]" ) ) {
                    throw new Exception( "invalid ust file" );
                }
                UstTrack track = new UstTrack();
                int type = 0; //0 => reading "SETTING" section
                while ( true ) {
#if DEBUG
                    PortUtil.println( "line=" + line );
#endif
                    UstEvent ue = null;
                    if ( type == 1 ) {
                        ue = new UstEvent();
                    }
                    int index = 0;
                    if ( line.Equals( "[#TRACKEND]" ) ) {
                        break;
                    } else if ( line.ToUpper().Equals( "[#NEXT]" ) ) {
                        index = NEXT_INDEX;
                    } else if ( line.ToUpper().Equals( "[#PREV]" ) ) {
                        index = PREV_INDEX;
                    } else {
                        String s = line.Replace( "[#", "" ).Replace( "]", "" ).Trim();
                        try {
                            index = PortUtil.parseInt( s );
                        } catch ( Exception ex ) {
#if DEBUG
                            PortUtil.println( "UstFile#.ctor; ex=" + ex );
#endif
                        }
                    }
#if DEBUG
                    bocoree.debug.push_log( "index=" + index );
#endif
                    line = sr.readLine(); // "[#" 直下の行
                    if ( line == null ) {
                        break;
                    }
                    while ( !line.StartsWith( "[#" ) ) {
#if DEBUG
                        PortUtil.println( "line=" + line );
#endif
                        String[] spl = PortUtil.splitString( line, new char[] { '=' }, 2 );
                        if ( type == 0 ) {
                            // reading "SETTING" section
                            if ( spl[0].Equals( "Tempo" ) ) {
                                m_tempo = 125f;
                                float v = 125f;
                                try {
                                    v = PortUtil.parseFloat( spl[1] );
                                    m_tempo = v;
                                } catch ( Exception ex ) {
                                }
                            } else if ( spl[0].Equals( "ProjectName" ) ) {
                                m_project_name = spl[1];
                            } else if ( spl[0].Equals( "VoiceDir" ) ) {
                                m_voice_dir = spl[1];
                            } else if ( spl[0].Equals( "OutFile" ) ) {
                                m_out_file = spl[1];
                            } else if ( spl[0].Equals( "CacheDir" ) ) {
                                m_cache_dir = spl[1];
                            } else if ( spl[0].Equals( "Tool1" ) ) {
                                m_tool1 = spl[1];
                            } else if ( spl[0].Equals( "Tool2" ) ) {
                                m_tool2 = spl[1];
                            }
                        } else if ( type == 1 ) {
                            // readin event section
                            if ( spl[0].Equals( "Length" ) ) {
                                ue.setLength( 0 );
                                int v = 0;
                                try {
                                    v = PortUtil.parseInt( spl[1] );
                                    ue.setLength( v );
                                } catch ( Exception ex ) {
                                }
                            } else if ( spl[0].Equals( "Lyric" ) ) {
                                ue.Lyric = spl[1];
                            } else if ( spl[0].Equals( "NoteNum" ) ) {
                                ue.Note = 0;
                                int v = 0;
                                try {
                                    v = PortUtil.parseInt( spl[1] );
                                    ue.Note = v;
                                } catch ( Exception ex ) {
                                }
                            } else if ( spl[0].Equals( "Intensity" ) ) {
                                ue.Intensity = 64;
                                int v = 64;
                                try {
                                    v = PortUtil.parseInt( spl[1] );
                                    ue.Intensity = v;
                                } catch ( Exception ex ) {
                                }
                            } else if ( spl[0].Equals( "PBType" ) ) {
                                ue.PBType = 5;
                                int v = 5;
                                try {
                                    v = PortUtil.parseInt( spl[1] );
                                    ue.PBType = v;
                                } catch ( Exception ex ) {
                                }
                            } else if ( spl[0].Equals( "Piches" ) ) {
                                String[] spl2 = PortUtil.splitString( spl[1], ',' );
                                float[] t = new float[spl2.Length];
                                for ( int i = 0; i < spl2.Length; i++ ) {
                                    float v = 0;
                                    try {
                                        v = PortUtil.parseFloat( spl2[i] );
                                        t[i] = v;
                                    } catch ( Exception ex ) {
                                    }
                                }
                                ue.Pitches = t;
                            } else if ( spl[0].Equals( "Tempo" ) ) {
                                ue.Tempo = 125f;
                                float v;
                                try {
                                    v = PortUtil.parseFloat( spl[1] );
                                    ue.Tempo = v;
                                } catch ( Exception ex ) {
                                }
                            } else if ( spl[0].Equals( "VBR" ) ) {
                                ue.Vibrato = new UstVibrato( line );
                                /*
                                PBW=50,50,46,48,56,50,50,50,50
                                PBS=-87
                                PBY=-15.9,-20,-31.5,-26.6
                                PBM=,s,r,j,s,s,s,s,s
                                */
                            } else if ( spl[0].Equals( "PBW" ) || spl[0].Equals( "PBS" ) || spl[0].Equals( "PBY" ) || spl[0].Equals( "PBM" ) ) {
                                if ( ue.Portamento == null ) {
                                    ue.Portamento = new UstPortamento();
                                }
                                ue.Portamento.ParseLine( line );
                            } else if ( spl[0].Equals( "Envelope" ) ) {
                                ue.Envelope = new UstEnvelope( line );
                                //PreUtterance=1
                                //VoiceOverlap=6
                            } else if ( spl[0].Equals( "VoiceOverlap" ) ) {
                                if ( spl[1] != "" ) {
                                    ue.VoiceOverlap = PortUtil.parseInt( spl[1] );
                                }
                            } else if ( spl[0].Equals( "PreUtterance" ) ) {
                                if ( spl[1] != "" ) {
                                    ue.PreUtterance = PortUtil.parseInt( spl[1] );
                                }
                            } else if ( spl[0].Equals( "Flags" ) ) {
                                ue.Flags = line.Substring( 6 );
                            } else {
#if DEBUG
                                PortUtil.println( "UstFile#.ctor; info: don't know how to process this line; line=" + line );
#endif
                            }
                        }
                        if ( !sr.ready() ) {
                            break;
                        }
                        line = sr.readLine();
                    }
#if DEBUG
                    bocoree.debug.push_log( "(ue==null)=" + (ue == null) );
#endif
                    if ( type == 0 ) {
                        type = 1;
                    } else if ( type == 1 ) {
                        ue.Index = index;
                        track.addEvent( ue );
                    }
                }
                m_tracks.add( track );
                updateTempoInfo();
            } catch ( Exception ex ) {
#if DEBUG
                bocoree.debug.push_log( "ex=" + ex );
#endif
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        /// <summary>
        /// vsqの指定したトラックを元に，トラックを1つだけ持つustを構築します
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track_index"></param>
        public UstFile( VsqFile vsq, int track_index ) {
            VsqTrack track = vsq.Track.get( track_index );
            
            // デフォルトのテンポ
            if( vsq.TempoTable.size() <= 0 ){
                m_tempo = 120.0f;
            }else{
                m_tempo = (float)(60e6 / (double)vsq.TempoTable.get( 0 ).Tempo);
            }
            updateTempoInfo();

            // R用音符のテンプレート
            int PBTYPE = 5;
            UstEvent template = new UstEvent();
            template.Lyric = "R";
            template.Note = 60;
            template.PreUtterance = 0;
            template.VoiceOverlap = 0;
            template.Intensity = 100;
            template.Moduration = 0;

            // 再生秒時をとりあえず無視して，ゲートタイム基準で音符を追加
            UstTrack track_add = new UstTrack();
            int last_clock = 0;
            int index = 0;
            for( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ){
                VsqEvent item = (VsqEvent)itr.next();
                if( last_clock < item.Clock ){
                    // ゲートタイム差あり，Rを追加
                    UstEvent itemust = (UstEvent)template.clone();
                    itemust.setLength( item.Clock - last_clock );
                    itemust.Index = index;
                    index++;
                    track_add.addEvent( itemust );
                }
                UstEvent item_add = new UstEvent();
                item_add.setLength( item.ID.getLength() );
                item_add.Lyric = item.ID.LyricHandle.L0.Phrase;
                item_add.Note = item.ID.Note;
                item_add.Index = index;
                item_add.Intensity = item.ID.Dynamics;
                item_add.Moduration = item.UstEvent.Moduration;
                item_add.PreUtterance = item.UstEvent.PreUtterance;
                item_add.VoiceOverlap = item.UstEvent.VoiceOverlap;
                index++;
                track_add.addEvent( item_add );
                last_clock = item.Clock + item.ID.getLength();
            }

            // 再生秒時を無視して，ピッチベンドを追加
            //VsqBPList pbs = track.getCurve( "pbs" );
            //VsqBPList pit = track.getCurve( "pit" );
            int clock = 0;
            for ( Iterator itr = track_add.getNoteEventIterator(); itr.hasNext(); ) {
                UstEvent item = (UstEvent)itr.next();
                int clock_begin = clock;
                int clock_end = clock + item.getLength();
                Vector<Float> pitch = new Vector<Float>();
                boolean allzero = true;
                for ( int cl = clock_begin; cl <= clock_end; cl += PBTYPE ) {
                    float pit = (float)track.getPitchAt( cl );
                    if ( pit != 0.0 ) {
                        allzero = false;
                    }
                    pitch.add( pit );
                }
                if ( !allzero ) {
                    item.Pitches = PortUtil.convertFloatArray( pitch.toArray( new Float[] { } ) );
                    item.PBType = PBTYPE;
                } else {
                    item.PBType = -1;
                }
                clock += item.getLength();
            }

            // 再生秒時を考慮して，適時テンポを追加
            //TODO: このへん
           // throw new NotImplementedException();

            m_tracks.add( track_add );
        }

        private UstFile() {
        }

        public String getProjectName() {
            return m_project_name;
        }

        public int getBaseTempo() {
            return (int)(6e7 / m_tempo);
        }

        public double getTotalSec() {
            int max = 0;
            for ( int track = 0; track < m_tracks.size(); track++ ) {
                int count = 0;
                for ( int i = 0; i < m_tracks.get( track ).getEventCount(); i++ ) {
                    count += (int)m_tracks.get( track ).getEvent( i ).getLength();
                }
                max = Math.Max( max, count );
            }
            return getSecFromClock( max );
        }

        public Vector<TempoTableEntry> getTempoList() {
            return m_tempo_table;
        }

        public UstTrack getTrack( int track ) {
            return m_tracks.get( track );
        }

        public int getTrackCount() {
            return m_tracks.size();
        }

        /// <summary>
        /// TempoTableの[*].Timeの部分を更新します
        /// </summary>
        /// <returns></returns>
        public void updateTempoInfo() {
            m_tempo_table = new Vector<TempoTableEntry>();
            if ( m_tracks.size() <= 0 ) {
                return;
            }
            int clock = 0;
            double time = 0.0;
            int last_tempo_clock = 0;  //最後にTempo値が代入されていたイベントのクロック
            float last_tempo = m_tempo;   //最後に代入されていたテンポの値
            for ( int i = 0; i < m_tracks.get( 0 ).getEventCount(); i++ ) {
                if ( m_tracks.get( 0 ).getEvent( i ).Tempo > 0f ) {
                    time += (clock - last_tempo_clock) / (8.0 * last_tempo);
                    if ( m_tempo_table.size() == 0 && clock != 0 ) {
                        m_tempo_table.add( new TempoTableEntry( 0, (int)(6e7 / m_tempo), 0.0 ) );
                    }
                    m_tempo_table.add( new TempoTableEntry( clock, (int)(6e7 / m_tracks.get( 0 ).getEvent( i ).Tempo), time ) );
                    last_tempo = m_tracks.get( 0 ).getEvent( i ).Tempo;
                    last_tempo_clock = clock;
                }
                clock += (int)m_tracks.get( 0 ).getEvent( i ).getLength();
            }
        }

        /// <summary>
        /// 指定したクロックにおける、clock=0からの演奏経過時間(sec)
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public double getSecFromClock( int clock ) {
            int c = m_tempo_table.size();
            for ( int i = c - 1; i >= 0; i-- ) {
                TempoTableEntry item = m_tempo_table.get( i );
                if ( item.Clock < clock ) {
                    double init = item.Time;
                    int dclock = clock - item.Clock;
                    double sec_per_clock1 = item.Tempo * 1e-6 / 480.0;
                    return init + dclock * sec_per_clock1;
                }
            }
            double sec_per_clock = 0.125 / m_tempo;
            return clock * sec_per_clock;
        }

        public void write( String file ) {
            BufferedWriter sw = null;
            try {
                sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( file ), "Shift_JIS" ) );
                sw.write( "[#SETTING]" );
                sw.newLine();
                sw.write( "Tempo=" + m_tempo );
                sw.newLine();
                sw.write( "Tracks=1" );
                sw.newLine();
                sw.write( "ProjectName=" + m_project_name );
                sw.newLine();
                sw.write( "VoiceDir=" + m_voice_dir );
                sw.newLine();
                sw.write( "OutFile=" + m_out_file );
                sw.newLine();
                sw.write( "CacheDir=" + m_cache_dir );
                sw.newLine();
                sw.write( "Tool1=" + m_tool1 );
                sw.newLine();
                sw.write( "Tool2=" + m_tool2 );
                sw.newLine();
                UstTrack target = m_tracks.get( 0 );
                int count = target.getEventCount();
                for ( int i = 0; i < count; i++ ) {
                    target.getEvent( i ).print( sw );
                }
                sw.write( "[#TRACKEND]" );
                sw.newLine();
            } catch ( Exception ex ) {
            } finally {
                if ( sw != null ) {
                    try {
                        sw.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        public Object clone() {
            UstFile ret = new UstFile();
            ret.m_tempo = m_tempo;
            ret.m_project_name = m_project_name;
            ret.m_voice_dir = m_voice_dir;
            ret.m_out_file = m_out_file;
            ret.m_cache_dir = m_cache_dir;
            ret.m_tool1 = m_tool1;
            ret.m_tool2 = m_tool2;
            for ( int i = 0; i < m_tracks.size(); i++ ) {
                ret.m_tracks.set( i, (UstTrack)m_tracks.get( i ).clone() );
            }
            ret.m_tempo_table = new Vector<TempoTableEntry>();
            for ( int i = 0; i < m_tempo_table.size(); i++ ) {
                ret.m_tempo_table.add( (TempoTableEntry)m_tempo_table.get( i ).clone() );
            }
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
