/*
 * UstFile.cs
 * Copyright (c) 2009 kbinani, PEX
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
using System.Text;

using bocoree;

namespace Boare.Lib.Vsq{

    public class UstFile : ICloneable {
        public object Tag;
        private float m_tempo = 120.00f;
        private String m_project_name = "";
        private String m_voice_dir = "";
        private String m_out_file = "";
        private String m_cache_dir = "";
        private String m_tool1 = "";
        private String m_tool2 = "";
        private Vector<UstTrack> m_tracks = new Vector<UstTrack>();
        private Vector<TempoTableEntry> m_tempo_table;
        
        public UstFile( String path ){
            try {
                cp932reader sr = new cp932reader( path );
#if DEBUG
                bocoree.debug.push_log( "path=" + path );
                bocoree.debug.push_log( "(sr==null)=" + (sr == null) );
#endif
                String line = sr.ReadLine();
                if ( line != "[#SETTING]" ) {
                    throw new Exception( "invalid ust file" );
                }
                UstTrack track = new UstTrack();
                int type = 0; //0 => reading "SETTING" section
                while ( true ) {
#if DEBUG
                    bocoree.debug.push_log( "line=" + line );
#endif
                    UstEvent ue = null;
                    if ( type == 1 ) {
                        ue = new UstEvent();
                    }
                    int index = 0;
                    if ( line.Equals( "[#TRACKEND]" ) ) {
                        break;
                    } else if ( line.ToUpper().Equals( "[#NEXT]" ) ) {
                        index = int.MaxValue;
                    } else if ( line.ToUpper().Equals( "[#PREV]" ) ) {
                        index = int.MinValue;
                    } else {
                        String s = line.Replace( "[#", "" ).Replace( "#", "" ).Trim();
                        int.TryParse( s, out index );
                    }
#if DEBUG
                    bocoree.debug.push_log( "index=" + index );
#endif
                    line = sr.ReadLine(); // "[#" 直下の行
                    if ( line == null ) {
                        break;
                    }
                    while ( !line.StartsWith( "[#" ) ) {
#if DEBUG
                        Console.WriteLine( "line=" + line );
#endif
                        String[] spl = line.Split( "=".ToCharArray(), 2 );
                        if ( type == 0 ) {
                            // reading "SETTING" section
                            if ( spl[0].Equals( "Tempo" ) ) {
                                m_tempo = 125f;
                                float v = 125f;
                                if ( float.TryParse( spl[1], out v ) ) {
                                    m_tempo = v;
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
                                ue.Length = 0;
                                int v = 0;
                                if ( int.TryParse( spl[1], out v ) ) {
                                    ue.Length = v;
                                }
                            } else if ( spl[0].Equals( "Lyric" ) ) {
                                ue.Lyric = spl[1];
                            } else if ( spl[0].Equals( "NoteNum" ) ) {
                                ue.Note = 0;
                                int v = 0;
                                if ( int.TryParse( spl[1], out v ) ) {
                                    ue.Note = v;
                                }
                            } else if ( spl[0].Equals( "Intensity" ) ) {
                                ue.Intensity = 64;
                                int v = 64;
                                if ( int.TryParse( spl[1], out v ) ) {
                                    ue.Intensity = v;
                                }
                            } else if ( spl[0].Equals( "PBType" ) ) {
                                ue.PBType = 5;
                                int v = 5;
                                if ( int.TryParse( spl[1], out v ) ) {
                                    ue.PBType = v;
                                }
                            } else if ( spl[0].Equals( "Piches" ) ) {
                                String[] spl2 = spl[1].Split( ",".ToCharArray() );
                                float[] t = new float[spl2.Length];
                                for ( int i = 0; i < spl2.Length; i++ ) {
                                    float v = 0;
                                    float.TryParse( spl2[i], out v );
                                    t[i] = v;
                                }
                                ue.Pitches = t;
                            } else if ( spl[0].Equals( "Tempo" ) ) {
                                ue.Tempo = 125f;
                                float v;
                                if ( float.TryParse( spl[1], out v ) ) {
                                    ue.Tempo = v;
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
                                    ue.VoiceOverlap = int.Parse( spl[1] );
                                }
                            } else if ( spl[0].Equals( "PreUtterance" ) ) {
                                if ( spl[1] != "" ) {
                                    ue.PreUtterance = int.Parse( spl[1] );
                                }
                            } else if ( spl[0].Equals( "Flags" ) ) {
                                ue.Flags = line.Substring( 6 );
                            }
                        }
                        if ( sr.Peek() < 0 ) {
                            break;
                        }
                        line = sr.ReadLine();
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
                sr.Close();
                updateTempoInfo();
            } catch ( Exception ex ) {
#if DEBUG
                bocoree.debug.push_log( "ex=" + ex );
#endif
            }
        }

        private UstFile(){
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
                    count += (int)m_tracks.get( track ).getEvent( i ).Length;
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
                clock += (int)m_tracks.get( 0 ).getEvent( i ).Length;
            }
#if DEBUG
            using ( StreamWriter sw = new StreamWriter( Path.Combine( System.Windows.Forms.Application.StartupPath, "ust_tempo_info.txt" ) ) ) {
                sw.WriteLine( "Clock\tTime\tTempo" );
                for ( int i = 0; i < m_tempo_table.size(); i++ ) {
                    sw.WriteLine( m_tempo_table.get( i ).Clock + "\t" + m_tempo_table.get( i ).Time + "\t" + m_tempo_table.get( i ).Tempo );
                }
            }
#endif
        }

        /// <summary>
        /// 指定したクロックにおける、clock=0からの演奏経過時間(sec)
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public double getSecFromClock( int clock ) {
            for ( int i = m_tempo_table.size() - 1; i >= 0; i-- ) {
                if ( m_tempo_table.get( i ).Clock < clock ) {
                    double init = m_tempo_table.get( i ).Time;
                    int dclock = clock - m_tempo_table.get( i ).Clock;
                    double sec_per_clock1 = m_tempo_table.get( i ).Tempo * 1e-6 / 480.0;
                    return init + dclock * sec_per_clock1;
                }
            }
            double sec_per_clock = 0.125 / m_tempo;
            return clock * sec_per_clock;
        }

        public void write( String file ) {
            StreamWriter sw = new StreamWriter( file, false, Encoding.GetEncoding( "Shift_JIS" ) );
            sw.WriteLine( "[#SETTING]" );
            sw.WriteLine( "Tempo=" + m_tempo );
            sw.WriteLine( "Tracks=1" );
            sw.WriteLine( "ProjectName=" + m_project_name );
            sw.WriteLine( "VoiceDir=" + m_voice_dir );
            sw.WriteLine( "OutFile=" + m_out_file );
            sw.WriteLine( "CacheDir=" + m_cache_dir );
            sw.WriteLine( "Tool1=" + m_tool1 );
            sw.WriteLine( "Tool2=" + m_tool2 );
            for ( int i = 0; i < m_tracks.get( 0 ).getEventCount(); i++ ) {
                m_tracks.get( 0 ).getEvent( i ).print( sw );
            }
            sw.WriteLine( "[#TRACKEND]" );
            sw.Close();
        }

        public object Clone(){
            UstFile ret = new UstFile();
            ret.m_tempo = m_tempo;
            ret.m_project_name = m_project_name;
            ret.m_voice_dir = m_voice_dir;
            ret.m_out_file = m_out_file;
            ret.m_cache_dir = m_cache_dir;
            ret.m_tool1 = m_tool1;
            ret.m_tool2 = m_tool2;
            for ( int i = 0; i < m_tracks.size(); i++ ) {
                ret.m_tracks.set( i, (UstTrack)m_tracks.get( i ).Clone() );
            }
            ret.m_tempo_table = new Vector<TempoTableEntry>();
            for ( int i = 0; i < m_tempo_table.size(); i++ ) {
                ret.m_tempo_table.add( (TempoTableEntry)m_tempo_table.get( i ).Clone() );
            }
            return ret;
        }
    }

}
