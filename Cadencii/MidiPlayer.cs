/*
 * Metronome.cs
 * Copyright (c)2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Threading;
using System.Collections.Generic;

using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public static class MidiPlayer {
        /// <summary>
        /// 通常音に使うプログラムチェンジ
        /// </summary>
        public static byte ProgramNormal = 115;
        /// <summary>
        /// ベル音に使うプログラムチェンジ
        /// </summary>
        public static byte ProgramBell = 9;
        /// <summary>
        /// 通常音のノートナンバー
        /// </summary>
        public static byte NoteNormal = 65;
        /// <summary>
        /// ベル音のノートナンバー
        /// </summary>
        public static byte NoteBell = 65;
        /// <summary>
        /// 小節ごとのベル音を鳴らすかどうか
        /// </summary>
        private static boolean s_ring_bell = true;
        /// <summary>
        /// 先行発音
        /// </summary>
        public static int PreUtterance = 10;
        public static byte ProgramGeneral = 0;

        private static System.Threading.Thread m_thread;
        private static boolean m_stop_required = false;
        private static DateTime m_started;
        private static VsqFileEx m_vsq;
        private static float m_speed = 1.0f;
        private static int m_started_clock;
        private static double m_started_sec;
        private static Vector<MidiQueue> s_queue = new Vector<MidiQueue>();
        private static boolean m_temp_exit = false;
        private static MidiDeviceImp s_device0; // メトロノーム用。一般用のデバイスIDが同じなら、こちらを共用
        private static MidiDeviceImp s_device1; // 一般用。メトロノームのデバイスIDと違うときのみ使用
        private static uint s_metronome_device = 0;
        private static uint s_general_device = 0;
        private static boolean m_stop_metronome_required = false;

        public static boolean RingBell {
            get {
                return s_ring_bell;
            }
            set {
                s_ring_bell = value;
                if ( value ) {
                    RestartMetronome();
                } else {
                    m_stop_metronome_required = true;
                }
            }
        }

        public static void RestartMetronome() {
            m_stop_metronome_required = false;
            if ( m_vsq != null ) {
                DateTime now = DateTime.Now;
                double elapsed = (now.Subtract( m_started ).TotalSeconds + 0.25) * m_speed;
                int clock = (int)m_vsq.getClockFromSec( m_started_sec + elapsed );

                int numerator, denominator, bar;
                m_vsq.getTimesigAt( clock, out numerator, out denominator, out bar );
                int clock_at_bartop = m_vsq.getClockFromBarCount( bar );
                int clock_step = 480 * 4 / denominator;
                int next_clock = clock_at_bartop + ((clock - clock_at_bartop) / clock_step + 1) * clock_step;

                MidiQueue mq = new MidiQueue();
                mq.Track = 0;
                mq.Clock = next_clock;
                mq.Channel = 14;
                mq.Program = ProgramNormal;
                mq.Note = NoteNormal;
                mq.Velocity = 0x40;
                mq.Done += new MidiQueueDoneEventHandler( ReGenerateMidiQueue );
                s_queue.add( mq );

                if ( (next_clock - clock_at_bartop) % (numerator * clock_step) == 0 ) {
                    MidiQueue mq_bell = new MidiQueue();
                    mq_bell.Track = 0;
                    mq_bell.Clock = next_clock;
                    mq_bell.Channel = 15;
                    mq_bell.Program = ProgramBell;
                    mq_bell.Note = NoteBell;
                    mq_bell.Velocity = 0x40;
                    s_queue.add( mq_bell );
                }
                Collections.sort( s_queue );
            }
        }

        public static void PlayImmediate( byte note ) {
            if ( s_metronome_device == s_general_device ) {
                if ( s_device0 == null ) {
                    s_device0 = new MidiDeviceImp( s_metronome_device );
                }
                s_device0.Play( 13, ProgramGeneral, note, 0x40 );
            } else {
                if ( s_device1 == null ) {
                    s_device1 = new MidiDeviceImp( s_general_device );
                }
                s_device1.Play( 13, ProgramGeneral, note, 0x40 );
            }
        }

        public static float GetSpeed() {
            return m_speed;
        }

        public static void SetSpeed( float speed, DateTime now ) {
            m_started = now;
            m_speed = speed;
            m_temp_exit = true;
        }

        public static uint DeviceMetronome {
            get {
                return s_metronome_device;
            }
            set {
                if ( s_metronome_device != value ) {
                    s_metronome_device = value;
                    if ( s_device0 != null ) {
                        s_device0.Terminate();
                        s_device0 = null;
                    }
                    s_device0 = new MidiDeviceImp( s_metronome_device );
                }
            }
        }

        public static uint DeviceGeneral {
            get {
                return s_general_device;
            }
            set {
                if ( s_general_device != value ){
                    s_general_device = value;
                    if ( s_general_device != s_metronome_device ) {
                        if ( s_device1 != null ) {
                            s_device1.Terminate();
                            s_device1 = null;
                        }
                        s_device1 = new MidiDeviceImp( s_general_device );
                    }
                }
            }
        }

        /// <summary>
        /// vsqファイルのstart_clockクロックからメトロノームを起動する。startは、start_clockをいつから起動したかを指定する。
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="start_clock"></param>
        /// <param name="start"></param>
        public static void Start( VsqFileEx vsq, int start_clock, DateTime start ) {
            s_queue.clear();
            m_stop_required = false;
            m_stop_metronome_required = false;
            if ( s_device0 == null ) {
                s_device0 = new MidiDeviceImp( s_metronome_device );
            }
            if ( s_metronome_device != s_general_device ) {
                if ( s_device1 == null ) {
                    s_device1 = new MidiDeviceImp( s_general_device );
                }
            }

            m_vsq = (VsqFileEx)vsq.Clone();
            m_started_sec = m_vsq.getSecFromClock( start_clock );
            int numerator, denominator, bar;
            m_vsq.getTimesigAt( start_clock, out numerator, out denominator, out bar );
            int clock_at_bartop = m_vsq.getClockFromBarCount( bar );
            int clock_step = 480 * 4 / denominator;
            int next_clock = clock_at_bartop + ((start_clock - clock_at_bartop) / clock_step + 1) * clock_step;
            m_started = start;
            m_started_clock = start_clock;
            m_temp_exit = false;

            if ( AppManager.editorConfig.MetronomeEnabled ) {
                MidiQueue mq = new MidiQueue();
                double tick_sec = m_vsq.getSecFromClock( next_clock );
                int next_bar;
                m_vsq.getTimesigAt( next_clock, out numerator, out denominator, out next_bar );
                mq.Track = 0;
                mq.Clock = next_clock;
                mq.Channel = 14;
                mq.Program = ProgramNormal;
                mq.Note = NoteNormal;
                mq.Velocity = 0x40;
                mq.Done += new MidiQueueDoneEventHandler( ReGenerateMidiQueue );
                s_queue.add( mq );

                if ( RingBell && next_bar != bar ) {
                    MidiQueue mq_bell = new MidiQueue();
                    mq_bell.Track = 0;
                    mq_bell.Clock = next_clock;
                    mq_bell.Channel = 15;
                    mq_bell.Program = ProgramBell;
                    mq_bell.Note = NoteBell;
                    mq_bell.Velocity = 0x40;
                    s_queue.add( mq_bell );
                }
            }

            for ( int track = 1; track < m_vsq.Track.size(); track++ ) {
#if DEBUG
                AppManager.debugWriteLine( "Metronome.Start; track=" + track );
#endif
                for ( Iterator itr = m_vsq.Track.get( track ).getNoteEventIterator(); itr.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr.next();
                    if ( start_clock <= item.Clock ) {
                        MidiQueue q = new MidiQueue();
                        q.Track = track;
                        q.Channel = (byte)(track - 1);
                        q.Clock = item.Clock;
                        q.Note = (byte)(item.ID.Note);
                        q.Program = 0;
                        q.Velocity = 0x40;
                        q.Done += new MidiQueueDoneEventHandler( ReGenerateMidiQueue );
                        s_queue.add( q );
                        break;
                    }
                }
            }

            Collections.sort( s_queue );

            m_thread = new Thread( new ThreadStart( ThreadProc ) );
            m_thread.IsBackground = true;
            m_thread.Priority = ThreadPriority.Highest;
            m_thread.Start();
        }

        private static Vector<MidiQueue> ReGenerateMidiQueue( MidiQueue sender ) {
            Vector<MidiQueue> ret = new Vector<MidiQueue>();
            if ( sender.Track == 0 ) {
                if ( AppManager.editorConfig.MetronomeEnabled ) {
                    int numerator, denominator, bar;
                    m_vsq.getTimesigAt( sender.Clock, out numerator, out denominator, out bar );
                    int clock_step = 480 * 4 / denominator;
                    int next_clock = sender.Clock + clock_step;

                    int next_bar;
                    m_vsq.getTimesigAt( next_clock, out numerator, out denominator, out next_bar );

                    MidiQueue mq = new MidiQueue();
                    mq.Track = 0;
                    mq.Clock = next_clock;
                    mq.Channel = 14;
                    mq.Program = ProgramNormal;
                    mq.Note = NoteNormal;
                    mq.Velocity = 0x40;
                    mq.Done += new MidiQueueDoneEventHandler( ReGenerateMidiQueue );
                    ret.add( mq );

                    if ( RingBell && next_bar != bar ) {
                        MidiQueue mq_bell = new MidiQueue();
                        mq_bell.Track = 0;
                        mq_bell.Clock = next_clock;
                        mq_bell.Channel = 15;
                        mq_bell.Program = ProgramBell;
                        mq_bell.Note = NoteBell;
                        mq_bell.Velocity = 0x40;
                        ret.add( mq_bell );
                    }
                }
            } else {
                int track = sender.Track;
                int clock = sender.Clock;
#if DEBUG
                AppManager.debugWriteLine( "Metronome.ReGenerateMidiQueue; track=" + track );
#endif
                for ( Iterator itr = m_vsq.Track.get( track ).getNoteEventIterator(); itr.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr.next();
                    if ( clock < item.Clock ) {
                        int thisclock = item.Clock;
                        boolean first = true;
                        while( !m_stop_required ) {
                            MidiQueue q = new MidiQueue();
                            q.Track = track;
                            q.Channel = (byte)(track - 1);
                            q.Clock = item.Clock;
                            q.Note = (byte)(item.ID.Note);
                            q.Program = 0;
                            q.Velocity = 0x40;
                            if ( first ) {
                                q.Done += new MidiQueueDoneEventHandler( ReGenerateMidiQueue );
                            }
                            first = false;
                            ret.add( q );

                            MidiQueue q_end = new MidiQueue(); //ノートオフ
                            q_end.Track = track;
                            q_end.Channel = (byte)(track - 1);
                            q_end.Clock = item.Clock + item.ID.Length;
                            q_end.Note = (byte)(item.ID.Note);
                            q_end.Program = 0;
                            q_end.Velocity = 0x0;
                            ret.add( q_end );
                            if ( itr.hasNext() ) {
                                item = (VsqEvent)itr.next();
                                if ( item.Clock != thisclock ) {
                                    break;
                                }
                            } else {
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            return ret;
        }

        public static void Stop() {
            m_stop_required = true;
        }

        private static void ThreadProc() {
            const int TOLERANCE_MILLISEC = 10;
            while ( !m_stop_required ) {
                if ( s_queue.size() == 0 ) {
                    Thread.Sleep( 100 );
                    continue;
                }
                int clock = s_queue.get( 0 ).Clock;
                double tick_sec = m_vsq.getSecFromClock( clock );
                DateTime next_tick = m_started.AddSeconds( (tick_sec - m_started_sec) / m_speed );
                DateTime now = DateTime.Now;
                TimeSpan ts = next_tick.Subtract( now );
                if ( ts.TotalMilliseconds <= 0 ) {
                    Vector<MidiQueue> add = new Vector<MidiQueue>();
                    while ( s_queue.size() > 0 && s_queue.get( 0 ).Clock == clock ) {
                        if ( s_queue.get( 0 ).Done != null ) {
                            add.addAll( s_queue.get( 0 ).Done( s_queue.get( 0 ) ) );
                        }
                        MidiQueue item = s_queue.get( 0 );
                        if ( item.Track == 0 || s_metronome_device == s_general_device ) {
                            s_device0.Play( item.Channel, item.Program, item.Note, item.Velocity );
                        } else {
                            s_device1.Play( item.Channel, item.Program, item.Note, item.Velocity );
                        }
                        s_queue.removeElementAt( 0 );
                    }
                    s_queue.addAll( add );
                    Collections.sort( s_queue );
                    continue;
                }
                int wait_millisec = (int)next_tick.Subtract( DateTime.Now ).TotalMilliseconds - PreUtterance;
                int thiswait = (wait_millisec > TOLERANCE_MILLISEC * 2) ? TOLERANCE_MILLISEC * 2 : wait_millisec;
#if DEBUG
                AppManager.debugWriteLine( "Metronome.ThreadProc; wait_millisec=" + wait_millisec );
#endif
                while ( thiswait > TOLERANCE_MILLISEC ) {
                    Thread.Sleep( thiswait );
                    wait_millisec = (int)next_tick.Subtract( DateTime.Now ).TotalMilliseconds - PreUtterance;
                    if ( wait_millisec < TOLERANCE_MILLISEC || m_stop_required ) {
                        break;
                    }
                    thiswait = wait_millisec;
                }
                if ( m_stop_required ) {
                    break;
                }
                if ( m_temp_exit ) {
                    m_temp_exit = false;
                    Vector<MidiQueue> add = new Vector<MidiQueue>();
                    while ( s_queue.size() > 0 && s_queue.get( 0 ).Clock == clock ) {
                        if ( s_queue.get( 0 ).Done != null ) {
                            add.addAll( s_queue.get( 0 ).Done( s_queue.get( 0 ) ) );
                        }
                        s_queue.removeElementAt( 0 );
                    }
                    s_queue.addAll( add );
                    Collections.sort( s_queue );
                    continue;
                }
                Vector<MidiQueue> adding = new Vector<MidiQueue>();
                while ( s_queue.size() > 0 && s_queue.get( 0 ).Clock == clock ) {
                    if ( s_queue.get( 0 ).Track == 0 || s_metronome_device == s_general_device ) {
                        if ( s_queue.get( 0 ).Track != 0 || (s_queue.get( 0 ).Track == 0 && !m_stop_metronome_required) ) {
                            s_device0.Play( s_queue.get( 0 ).Channel, s_queue.get( 0 ).Program, s_queue.get( 0 ).Note, s_queue.get( 0 ).Velocity );
                        }
                    } else {
                        s_device1.Play( s_queue.get( 0 ).Channel, s_queue.get( 0 ).Program, s_queue.get( 0 ).Note, s_queue.get( 0 ).Velocity );
                    }
                    if ( s_queue.get( 0 ).Done != null ) {
                        if ( s_queue.get( 0 ).Track != 0 || (s_queue.get( 0 ).Track == 0 && !m_stop_metronome_required) ) {
                            adding.addAll( s_queue.get( 0 ).Done( s_queue.get( 0 ) ) );
                        }
                    }
                    s_queue.removeElementAt( 0 );
                }
                s_queue.addAll( adding );
                Collections.sort( s_queue );
            }
        }
    }

}
