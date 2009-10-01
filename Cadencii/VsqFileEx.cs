/*
 * VsqFileEx.cs
 * Copyright (c) 2008-2009 kbinani
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
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;
    using Integer = Int32;

    [Serializable]
    public class VsqFileEx : VsqFile, ICloneable, ICommandRunnable {
        static XmlSerializer s_vsq_serializer;

        public AttachedCurve AttachedCurves;
        public Vector<BgmFile> BgmFiles = new Vector<BgmFile>();

        static VsqFileEx() {
            s_vsq_serializer = new XmlSerializer( typeof( VsqFileEx ) );
        }

        /// <summary>
        /// VsqEvent, VsqBPList, BezierCurvesの全てのクロックを、tempoに格納されているテンポテーブルに
        /// 合致するようにシフトします
        /// </summary>
        /// <param name="work"></param>
        /// <param name="tempo"></param>
        public void adjustClockToMatchWith( VsqFile tempo  ) {
            double premeasure_sec_target = getSecFromClock( getPreMeasureClocks() );
            double premeasure_sec_tempo = premeasure_sec_target;
#if DEBUG
            Console.WriteLine( "FormMain#ShiftClockToMatchWith; premeasure_sec_target=" + premeasure_sec_target + "; premeasre_sec_tempo=" + premeasure_sec_tempo );
#endif

            // テンポをリプレースする場合。
            // まずクロック値を、リプレース後のモノに置き換え
            for ( int track = 1; track < this.Track.size(); track++ ) {
                // ノート・歌手イベントをシフト
                for ( Iterator itr = this.Track.get( track ).getEventIterator(); itr.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr.next();
                    if ( item.ID.type == VsqIDType.Singer && item.Clock == 0 ) {
                        continue;
                    }
                    int clock = item.Clock;
                    double sec_start = this.getSecFromClock( clock ) - premeasure_sec_target + premeasure_sec_tempo;
                    double sec_end = this.getSecFromClock( clock + item.ID.Length ) - premeasure_sec_target + premeasure_sec_tempo;
                    int clock_start = (int)tempo.getClockFromSec( sec_start );
                    int clock_end = (int)tempo.getClockFromSec( sec_end );
                    item.Clock = clock_start;
                    item.ID.Length = clock_end - clock_start;
                    if ( item.ID.VibratoHandle != null ) {
                        double sec_vib_start = this.getSecFromClock( clock + item.ID.VibratoDelay ) - premeasure_sec_target + premeasure_sec_tempo;
                        int clock_vib_start = (int)tempo.getClockFromSec( sec_vib_start );
                        item.ID.VibratoDelay = clock_vib_start - clock_start;
                        item.ID.VibratoHandle.Length = clock_end - clock_vib_start;
                    }
                }

                // コントロールカーブをシフト
                foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                    VsqBPList item = this.Track.get( track ).getCurve( ct.Name );
                    if ( item == null ) {
                        continue;
                    }
                    VsqBPList repl = new VsqBPList( item.getDefault(), item.getMinimum(), item.getMaximum() );
                    for ( int i = 0; i < item.size(); i++ ) {
                        int clock = item.getKeyClock( i );
                        int value = item.getElement( i );
                        double sec = this.getSecFromClock( clock ) - premeasure_sec_target + premeasure_sec_tempo;
                        if ( sec >= premeasure_sec_tempo ) {
                            int clock_new = (int)tempo.getClockFromSec( sec );
                            repl.add( clock_new, value );
                        }
                    }
                    this.Track.get( track ).setCurve( ct.Name, repl );
                }

                // ベジエカーブをシフト
                foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                    Vector<BezierChain> list = this.AttachedCurves.get( track - 1 ).get( ct );
                    if ( list == null ) {
                        continue;
                    }
                    for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                        BezierChain chain = (BezierChain)itr.next();
                        for ( Iterator itr2 = chain.points.iterator(); itr2.hasNext(); ) {
                            BezierPoint point = (BezierPoint)itr2.next();
                            PointD bse = new PointD( tempo.getClockFromSec( this.getSecFromClock( point.getBase().X ) - premeasure_sec_target + premeasure_sec_tempo ),
                                                     point.getBase().Y );
                            double rx = point.getBase().X + point.controlRight.X;
                            double new_rx = tempo.getClockFromSec( this.getSecFromClock( rx ) - premeasure_sec_target + premeasure_sec_tempo );
                            PointD ctrl_r = new PointD( new_rx - bse.X, point.controlRight.Y );

                            double lx = point.getBase().X + point.controlLeft.X;
                            double new_lx = tempo.getClockFromSec( this.getSecFromClock( lx ) - premeasure_sec_target + premeasure_sec_tempo );
                            PointD ctrl_l = new PointD( new_lx - bse.X, point.controlLeft.Y );
                            point.setBase( bse );
                            point.controlLeft = ctrl_l;
                            point.controlRight = ctrl_r;
                        }
                    }
                }
            }
        }

        public static void shift( VsqFileEx vsq, double sec ) {
            bool first = true; // 負になった最初のアイテムかどうか

            // 最初にテンポをずらす．
            // 古いのから情報をコピー
            VsqFile tempo = new  VsqFile( "Miku", vsq.getPreMeasure(), 4, 4, 500000 );
            tempo.TempoTable.clear();
            for ( Iterator itr = vsq.TempoTable.iterator(); itr.hasNext(); ) {
                TempoTableEntry item = (TempoTableEntry)itr.next();
                tempo.TempoTable.add( item );
            }
            tempo.updateTempoInfo();
            int first_tempo = 500000;
            int tempo_count = tempo.TempoTable.size();
            if ( tempo_count > 0 ) {
                first_tempo = tempo.TempoTable.get( 0 ).Tempo;
            }
            if ( sec < 0.0 ) {
                first = true;
                for ( int i = tempo_count - 1; i >= 0; i-- ) {
                    TempoTableEntry item = tempo.TempoTable.get( i );
                    if ( item.Time + sec <= 0.0 ) {
                        if ( first ) {
                            first_tempo = item.Tempo;
                            first = false;
                        } else {
                            break;
                        }
                    }
                }
            }
            vsq.TempoTable.clear();
            vsq.TempoTable.add( new TempoTableEntry( 0, first_tempo, 0.0 ) );
            for ( int i = 0; i < tempo_count; i++ ) {
                if ( sec >= 0.0 && i == 0 ) {
                    continue;
                }
                TempoTableEntry item = tempo.TempoTable.get( i );
                double t = item.Time + sec;
                int new_clock = (int)vsq.getClockFromSec( t );
                double new_time = vsq.getSecFromClock( new_clock );
                vsq.TempoTable.add( new TempoTableEntry( new_clock, item.Tempo, new_time ) );
            }
            vsq.updateTempoInfo();

            int tracks = vsq.Track.size();
            int pre_measure_clocks = vsq.getPreMeasureClocks();
            for ( int i = 1; i < tracks; i++ ) {
                VsqTrack track = vsq.Track.get( i );
                Vector<Integer> remove_required_event = new Vector<Integer>(); // 削除が要求されたイベントのインデクス
                
                // 歌手変更・音符イベントをシフト
                // 時刻が負になる場合は，後で考える
                int events = track.getEventCount();
                first = true;
                for ( int k = events - 1; k >= 0; k-- ) {
                    VsqEvent item = track.getEvent( k );
                    double t = vsq.getSecFromClock( item.Clock ) + sec;
                    int clock = (int)vsq.getClockFromSec( t );
                    if ( item.ID.type == VsqIDType.Anote ) {
                        // 音符の長さ
                        double t_end = vsq.getSecFromClock( item.Clock + item.ID.Length ) + sec;
                        int clock_end = (int)vsq.getClockFromSec( t_end );
                        int length = clock_end - clock;
                        
                        if ( clock < pre_measure_clocks ){
                            if ( pre_measure_clocks < clock_end ) {
                                // 音符の開始位置がプリメジャーよりも早く，音符の開始位置がプリメジャーより後の場合
                                clock = pre_measure_clocks;
                                length = clock_end - pre_measure_clocks;
                                // ビブラート
                                if ( item.ID.VibratoHandle != null ) {
                                    double vibrato_percent = item.ID.VibratoHandle.Length / (double)item.ID.Length * 100.0;
                                    double t_clock = vsq.getSecFromClock( clock ); // 音符の開始時刻
                                    double t_vibrato = t_end - (t_end - t_clock) * vibrato_percent / 100.0; // ビブラートの開始時刻
                                    int clock_vibrato_start = (int)vsq.getClockFromSec( t_vibrato );
                                    item.ID.VibratoHandle.Length = clock_end - clock_vibrato_start;
                                    item.ID.VibratoDelay = clock_vibrato_start - clock;
                                }
                                item.Clock = clock;
                                item.ID.Length = length;
                            } else {
                                // 範囲外なので削除
                                remove_required_event.add( k );
                            }
                        } else {
                            // ビブラート
                            if ( item.ID.VibratoHandle != null ) {
                                double t_vibrato_start = vsq.getSecFromClock( item.Clock + item.ID.Length - item.ID.VibratoHandle.Length ) + sec;
                                int clock_vibrato_start = (int)vsq.getClockFromSec( t_vibrato_start );
                                item.ID.VibratoHandle.Length = clock_vibrato_start - clock;
                                item.ID.VibratoDelay = clock_vibrato_start - clock;
                            }
                            item.Clock = clock;
                            item.ID.Length = length;
                        }
                    } else if ( item.ID.type == VsqIDType.Singer ) {
                        if ( item.Clock <= 0 ) {
                            if ( sec >= 0.0 ) {
                                clock = 0;
                                item.Clock = clock;
                            } else {
                                if ( first ) {
                                    clock = 0;
                                    first = false;
                                    item.Clock = clock;
                                } else {
                                    remove_required_event.add( k );
                                }
                            }
                        } else {
                            if ( clock < 0 ) {
                                if ( first ) {
                                    clock = 0;
                                    first = false;
                                } else {
                                    remove_required_event.add( k );
                                }
                            }
                            item.Clock = clock;
                        }
                    }
                }
                // 削除が要求されたものを削除
                Collections.sort( remove_required_event );
                int count = remove_required_event.size();
                for ( int j = count - 1; j >= 0; j-- ) {
                    int index = remove_required_event.get( j );
                    track.removeEvent( index );
                }

                // コントロールカーブをシフト
                foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                    VsqBPList item = track.getCurve( ct.Name );
                    if ( item == null ) {
                        continue;
                    }
                    VsqBPList repl = new VsqBPList( item.getDefault(), item.getMinimum(), item.getMaximum() );
                    int c = item.size();
                    first = true;
                    for ( int j = c - 1; j >=0; j-- ) {
                        int clock = item.getKeyClock( j );
                        int value = item.getElement( j );
                        double t = vsq.getSecFromClock( clock ) + sec;
                        int clock_new = (int)vsq.getClockFromSec( t );
                        if ( clock_new < pre_measure_clocks ) {
                            if ( first ) {
                                clock_new = pre_measure_clocks;
                                first = false;
                            } else {
                                break;
                            }
                        }
                        repl.add( clock_new, value );
                    }
                    track.setCurve( ct.getName(), repl );
                }

                // ベジエカーブをシフト
                foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                    Vector<BezierChain> list = vsq.AttachedCurves.get( i - 1 ).get( ct );
                    if ( list == null ) {
                        continue;
                    }
                    remove_required_event.clear(); //削除するBezierChainのID
                    int list_count = list.size();
                    for ( int j = 0; j < list_count; j++ ) {
                        BezierChain chain = list.get( j );
                        for ( Iterator itr2 = chain.points.iterator(); itr2.hasNext(); ) {
                            BezierPoint point = (BezierPoint)itr2.next();
                            PointD bse = new PointD( vsq.getClockFromSec( vsq.getSecFromClock( point.getBase().X ) + sec ),
                                                     point.getBase().Y );
                            double rx = point.getBase().X + point.controlRight.X;
                            double new_rx = vsq.getClockFromSec( vsq.getSecFromClock( rx ) + sec );
                            PointD ctrl_r = new PointD( new_rx - bse.X, point.controlRight.Y );

                            double lx = point.getBase().X + point.controlLeft.X;
                            double new_lx = vsq.getClockFromSec( vsq.getSecFromClock( lx ) + sec );
                            PointD ctrl_l = new PointD( new_lx - bse.X, point.controlLeft.Y );
                            point.setBase( bse );
                            point.controlLeft = ctrl_l;
                            point.controlRight = ctrl_r;
                        }
                        double start = chain.getStart();
                        double end = chain.getEnd();
                        if ( start < pre_measure_clocks ) {
                            if ( pre_measure_clocks < end ) {
                                // プリメジャーのところでカットし，既存のものと置き換える
                                BezierChain new_chain = chain.extractPartialBezier( pre_measure_clocks, end );
                                new_chain.id = chain.id;
                                list.set( j, new_chain );
                            } else {
                                remove_required_event.add( chain.id );
                            }
                        }
                    }

                    // 削除が要求されたベジエカーブを削除
                    count = remove_required_event.size();
                    for ( int j = 0; j < count; j++ ) {
                        int id = remove_required_event.get( j );
                        list_count = list.size();
                        for ( int k = 0; k < list_count; k++ ) {
                            if ( id == list.get( k ).id ) {
                                list.removeElementAt( k );
                                break;
                            }
                        }
                    }
                }
            }
        }

        public override object Clone() {
            return clone();
        }

        public Object clone() {
            VsqFileEx ret = new VsqFileEx( "Miku", 1, 4, 4, 500000 );
            ret.Track = new Vector<VsqTrack>();
            int c = Track.size();
            for ( int i = 0; i < c; i++ ) {
                ret.Track.add( (VsqTrack)Track.get( i ).Clone() );
            }
#if USE_TEMPO_LIST

#else
            ret.TempoTable = new Vector<TempoTableEntry>();
            c = TempoTable.size();
            for ( int i = 0; i < c; i++ ) {
                ret.TempoTable.add( (TempoTableEntry)TempoTable.get( i ).Clone() );
            }
#endif
            ret.TimesigTable = new Vector<TimeSigTableEntry>();
            c = TimesigTable.size();
            for ( int i = 0; i < c; i++ ) {
                ret.TimesigTable.add( (TimeSigTableEntry)TimesigTable.get( i ).Clone() );
            }
            ret.m_tpq = m_tpq;
            ret.TotalClocks = TotalClocks;
            ret.m_base_tempo = m_base_tempo;
            ret.Master = (VsqMaster)Master.Clone();
            ret.Mixer = (VsqMixer)Mixer.Clone();
            //ret.m_premeasure_clocks = m_premeasure_clocks;
            ret.AttachedCurves = (AttachedCurve)AttachedCurves.Clone();
            /*ret.m_pitch.Clear();
            for ( int i = 0; i < m_pitch.Count; i++ ) {
                ret.m_pitch.Add( (VsqBPList)m_pitch[i].Clone() );
            }*/
            c = BgmFiles.size();
            for ( int i = 0; i < c; i++ ) {
                ret.BgmFiles.add( (BgmFile)BgmFiles.get( i ).clone() );
            }

            return ret;
        }

        /// <summary>
        /// BGMリストの内容を更新するコマンドを発行します
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static CadenciiCommand generateCommandBgmUpdate( Vector<BgmFile> list ) {
            CadenciiCommand command = new CadenciiCommand();
            command.type = CadenciiCommandType.BGM_UPDATE;
            command.args = new Object[1];
            Vector<BgmFile> copy = new Vector<BgmFile>();
            int count = list.size();
            for ( int i = 0; i < count; i++ ) {
                copy.add( (BgmFile)list.get( i ).clone() );
            }
            command.args[0] = copy;
            return command;
        }

        /// <summary>
        /// トラックを削除するコマンドを発行します。VstRendererを取り扱う関係上、VsqCommandを使ってはならない。
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static CadenciiCommand generateCommandDeleteTrack( int track ) {
            CadenciiCommand command = new CadenciiCommand();
            command.type = CadenciiCommandType.TRACK_DELETE;
            command.args = new object[1];
            command.args[0] = track;
            return command;
        }

        public static CadenciiCommand generateCommandTrackReplace( int track, VsqTrack item, BezierCurves attached_curve ) {
        //public static CadenciiCommand generateCommandTrackReplace( int track, VsqTrack item, BezierCurves attached_curve, VsqBPList pitch ) {
            CadenciiCommand command = new CadenciiCommand();
            command.type = CadenciiCommandType.TRACK_REPLACE;
            command.args = new object[3];
            command.args[0] = track;
            command.args[1] = item.Clone();
            command.args[2] = attached_curve.Clone();
            //command.Args[3] = pitch.Clone();
            return command;
        }

        /// <summary>
        /// トラックを追加するコマンドを発行します．VstRendererを取り扱う関係上、VsqCommandを使ってはならない。
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static CadenciiCommand generateCommandAddTrack( VsqTrack track, VsqMixerEntry mixer, int position, BezierCurves attached_curve ) {
            CadenciiCommand command = new CadenciiCommand();
            command.type = CadenciiCommandType.TRACK_ADD;
            command.args = new object[4];
            command.args[0] = track.Clone();
            command.args[1] = mixer;
            command.args[2] = position;
            command.args[3] = attached_curve.Clone();
            //command.Args[4] = pitch.Clone();
            return command;
        }

        public static CadenciiCommand generateCommandAddBezierChain( int track, CurveType curve_type, int chain_id, int clock_resolution, BezierChain chain ) {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.BEZIER_CHAIN_ADD;
            ret.args = new object[5];
            ret.args[0] = track;
            ret.args[1] = curve_type;
            ret.args[2] = (BezierChain)chain.Clone();
            ret.args[3] = clock_resolution;
            ret.args[4] = chain_id;
            return ret;
        }

        public static CadenciiCommand generateCommandDeleteBezierChain( int track, CurveType curve_type, int chain_id, int clock_resolution ) {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.BEZIER_CHAIN_DELETE;
            ret.args = new object[4];
            ret.args[0] = track;
            ret.args[1] = curve_type;
            ret.args[2] = chain_id;
            ret.args[3] = clock_resolution;
            return ret;
        }

        public static CadenciiCommand generateCommandReplaceBezierChain( int track, CurveType curve_type, int chain_id, BezierChain chain, int clock_resolution ) {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.BEZIER_CHAIN_REPLACE;
            ret.args = new object[5];
            ret.args[0] = track;
            ret.args[1] = curve_type;
            ret.args[2] = chain_id;
            ret.args[3] = chain;
            ret.args[4] = clock_resolution;
            return ret;
        }

        public static CadenciiCommand generateCommandReplace( VsqFileEx vsq ) {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.REPLACE;
            ret.args = new object[1];
            ret.args[0] = (VsqFileEx)vsq.Clone();
            return ret;
        }

        public static CadenciiCommand generateCommandReplaceAttachedCurveRange( int track, TreeMap<CurveType, Vector<BezierChain>> attached_curves ) {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.ATTACHED_CURVE_REPLACE_RANGE;
            ret.args = new object[2];
            ret.args[0] = track;
            TreeMap<CurveType, Vector<BezierChain>> copy = new TreeMap<CurveType, Vector<BezierChain>>();
            for ( Iterator itr = attached_curves.keySet().iterator(); itr.hasNext(); ) {
                CurveType ct = (CurveType)itr.next();
                Vector<BezierChain> list = attached_curves.get( ct );
                Vector<BezierChain> copy_list = new Vector<BezierChain>();
                for ( Iterator itr2 = list.iterator(); itr2.hasNext(); ) {
                    copy_list.add( (BezierChain)((BezierChain)itr2.next()).clone() );
                }
                copy.put( ct, copy_list );
            }
            ret.args[1] = copy;
            return ret;
        }

        public VsqCommand preprocessSpecialCommand( VsqCommand command ) {
#if DEBUG
            AppManager.debugWriteLine( "preprocessSpecialCommand; command.Type=" + command.Type );
#endif
            if ( command.Type == VsqCommandType.TRACK_CURVE_EDIT ) {
                #region TrackEditCurve
                int track = (int)command.Args[0];
                String curve = (String)command.Args[1];
                Vector<BPPair> com = (Vector<BPPair>)command.Args[2];
                VsqBPList target = target = Track.get( track ).getCurve( curve );

                VsqCommand inv = null;
                Vector<BPPair> edit = new Vector<BPPair>();
                if ( com != null ) {
                    if ( com.size() > 0 ) {
                        int start_clock = com.get( 0 ).Clock;
                        int end_clock = com.get( 0 ).Clock;
                        for ( Iterator itr = com.iterator(); itr.hasNext(); ){
                            BPPair item = (BPPair)itr.next();
                            start_clock = Math.Min( start_clock, item.Clock );
                            end_clock = Math.Max( end_clock, item.Clock );
                        }
                        Track.get( track ).setEditedStart( start_clock );
                        Track.get( track ).setEditedEnd( end_clock );
                        int start_value = target.getValue( start_clock );
                        int end_value = target.getValue( end_clock );
                        for ( Iterator i = target.keyClockIterator(); i.hasNext(); ) {
                            int clock = (int)i.next();
                            if ( start_clock <= clock && clock <= end_clock ) {
                                edit.add( new BPPair( clock, target.getValue( clock ) ) );
                            }
                        }
                        boolean start_found = false;
                        boolean end_found = false;
                        for ( int i = 0; i < edit.size(); i++ ) {
                            if ( edit.get( i ).Clock == start_clock ) {
                                start_found = true;
                                edit.get( i ).Value = start_value;
                                if ( start_found && end_found ) {
                                    break;
                                }
                            }
                            if ( edit.get( i ).Clock == end_clock ) {
                                end_found = true;
                                edit.get( i ).Value = end_value;
                                if ( start_found && end_found ) {
                                    break;
                                }
                            }
                        }
                        if ( !start_found ) {
                            edit.add( new BPPair( start_clock, start_value ) );
                        }
                        if ( !end_found ) {
                            edit.add( new BPPair( end_clock, end_value ) );
                        }

                        // 並べ替え
                        Collections.sort( edit );
                        inv = VsqCommand.generateCommandTrackCurveEdit( track, curve, edit );
                    } else if ( com.size() == 0 ) {
                        inv = VsqCommand.generateCommandTrackCurveEdit( track, curve, new Vector<BPPair>() );
                    }
                }

                updateTotalClocks();
                if ( com.size() == 0 ) {
                    return inv;
                } else if ( com.size() == 1 ) {
                    boolean found = false;
                    for ( Iterator itr = target.keyClockIterator(); itr.hasNext(); ) {
                        int clock = (int)itr.next();
                        if ( clock == com.get( 0 ).Clock ) {
                            found = true;
                            target.add( clock, com.get( 0 ).Value );
                            break;
                        }
                    }
                    if ( !found ) {
                        target.add( com.get( 0 ).Clock, com.get( 0 ).Value );
                    }
                } else {
                    int start_clock = com.get( 0 ).Clock;
                    int end_clock = com.get( com.size() - 1 ).Clock;
                    boolean removed = true;
                    while ( removed ) {
                        removed = false;
                        for ( Iterator itr = target.keyClockIterator(); itr.hasNext(); ) {
                            int clock = (int)itr.next();
                            if ( start_clock <= clock && clock <= end_clock ) {
                                target.remove( clock );
                                removed = true;
                                break;
                            }
                        }
                    }
                    for ( Iterator itr = com.iterator(); itr.hasNext(); ){
                        BPPair item = (BPPair)itr.next();
                        target.add( item.Clock, item.Value );
                    }
                }
                return inv;
                #endregion
            } else if ( command.Type == VsqCommandType.TRACK_CURVE_EDIT_RANGE ) {
                #region TrackEditCurveRange
                int track = (int)command.Args[0];
                String[] curves = (String[])command.Args[1];
                Vector<BPPair>[] coms = (Vector<BPPair>[])command.Args[2];
                Vector<BPPair>[] inv_coms = new Vector<BPPair>[curves.Length];
                VsqCommand inv = null;

                for ( int k = 0; k < curves.Length; k++ ) {
                    String curve = curves[k];
                    VsqBPList target = Track.get( track ).getCurve( curve );
                    Vector<BPPair> com = coms[k];
                    Vector<BPPair> edit = new Vector<BPPair>();
                    if ( com != null ) {
                        if ( com.size() > 0 ) {
                            int start_clock = com.get( 0 ).Clock;
                            int end_clock = com.get( 0 ).Clock;
                            for ( Iterator itr = com.iterator(); itr.hasNext(); ){
                                BPPair item = (BPPair)itr.next();
                                start_clock = Math.Min( start_clock, item.Clock );
                                end_clock = Math.Max( end_clock, item.Clock );
                            }
                            Track.get( track ).setEditedStart( start_clock );
                            Track.get( track ).setEditedEnd( end_clock );
                            int start_value = target.getValue( start_clock );
                            int end_value = target.getValue( end_clock );
                            for ( Iterator itr = target.keyClockIterator(); itr.hasNext(); ) {
                                int clock = (int)itr.next();
                                if ( start_clock <= clock && clock <= end_clock ) {
                                    edit.add( new BPPair( clock, target.getValue( clock ) ) );
                                }
                            }
                            boolean start_found = false;
                            boolean end_found = false;
                            for ( int i = 0; i < edit.size(); i++ ) {
                                if ( edit.get( i ).Clock == start_clock ) {
                                    start_found = true;
                                    edit.get( i ).Value = start_value;
                                    if ( start_found && end_found ) {
                                        break;
                                    }
                                }
                                if ( edit.get( i ).Clock == end_clock ) {
                                    end_found = true;
                                    edit.get( i ).Value = end_value;
                                    if ( start_found && end_found ) {
                                        break;
                                    }
                                }
                            }
                            if ( !start_found ) {
                                edit.add( new BPPair( start_clock, start_value ) );
                            }
                            if ( !end_found ) {
                                edit.add( new BPPair( end_clock, end_value ) );
                            }

                            // 並べ替え
                            Collections.sort( edit );
                            inv_coms[k] = edit;
                        } else if ( com.size() == 0 ) {
                            inv_coms[k] = new Vector<BPPair>();
                        }
                    }

                    updateTotalClocks();
                    if ( com.size() == 0 ) {
                        return inv;
                    } else if ( com.size() == 1 ) {
                        boolean found = false;
                        for ( Iterator itr = target.keyClockIterator(); itr.hasNext(); ) {
                            int clock = (int)itr.next();
                            if ( clock == com.get( 0 ).Clock ) {
                                found = true;
                                target.add( clock, com.get( 0 ).Value );
                                break;
                            }
                        }
                        if ( !found ) {
                            target.add( com.get( 0 ).Clock, com.get( 0 ).Value );
                        }
                    } else {
                        int start_clock = com.get( 0 ).Clock;
                        int end_clock = com.get( com.size() - 1 ).Clock;
                        boolean removed = true;
                        while ( removed ) {
                            removed = false;
                            for ( Iterator itr = target.keyClockIterator(); itr.hasNext(); ) {
                                int clock = (int)itr.next();
                                if ( start_clock <= clock && clock <= end_clock ) {
                                    target.remove( clock );
                                    removed = true;
                                    break;
                                }
                            }
                        }
                        for ( Iterator itr = com.iterator(); itr.hasNext(); ){
                            BPPair item = (BPPair)itr.next();
                            target.add( item.Clock, item.Value );
                        }
                    }
                }
                return VsqCommand.generateCommandTrackCurveEditRange( track, curves, inv_coms );
                #endregion
            }
            return null;
        }

        public ICommand executeCommand( ICommand com ) {
#if DEBUG
            AppManager.debugWriteLine( "VsqFileEx.Execute" );
#endif
            CadenciiCommand command = (CadenciiCommand)com;
#if DEBUG
            AppManager.debugWriteLine( "VsqFileEx.Execute; command.Type=" + command.type );
#endif
            CadenciiCommand ret = null;
            if ( command.type == CadenciiCommandType.VSQ_COMMAND ) {
                ret = new CadenciiCommand();
                ret.type = CadenciiCommandType.VSQ_COMMAND;
                if ( command.vsqCommand.Type == VsqCommandType.TRACK_CURVE_EDIT || command.vsqCommand.Type == VsqCommandType.TRACK_CURVE_EDIT_RANGE ) {
                    ret.vsqCommand = preprocessSpecialCommand( command.vsqCommand );
                } else {
                    ret.vsqCommand = base.executeCommand( command.vsqCommand );
                }
            } else {
                if ( command.type == CadenciiCommandType.BEZIER_CHAIN_ADD ) {
                    #region AddBezierChain
#if DEBUG
                    AppManager.debugWriteLine( "    AddBezierChain" );
#endif
                    int track = (int)command.args[0];
                    CurveType curve_type = (CurveType)command.args[1];
                    BezierChain chain = (BezierChain)command.args[2];
                    int clock_resolution = (int)command.args[3];
                    int added_id = (int)command.args[4];
                    AttachedCurves.get( track - 1 ).addBezierChain( curve_type, chain, added_id );
                    ret = generateCommandDeleteBezierChain( track, curve_type, added_id, clock_resolution );
                    if ( chain.size() > 1 ) {
                        int min = (int)chain.points.get( 0 ).getBase().X;
                        int max = min;
                        for ( int i = 1; i < chain.points.size(); i++ ) {
                            min = Math.Min( min, (int)chain.points.get( i ).getBase().X );
                            max = Math.Max( max, (int)chain.points.get( i ).getBase().X );
                        }
                        int max_value = curve_type.Maximum;
                        int min_value = curve_type.Minimum;
                        if ( min < max ) {
                            Vector<BPPair> edit = new Vector<BPPair>();
                            int last_value = int.MaxValue;
                            for ( int clock = min; clock < max; clock += clock_resolution ) {
                                int value = (int)chain.getValue( (float)clock );
                                if ( value < min_value ) {
                                    value = min_value;
                                } else if ( max_value < value ) {
                                    value = max_value;
                                }
                                if ( value != last_value ) {
                                    edit.add( new BPPair( clock, value ) );
                                    last_value = value;
                                }
                            }
                            int value2;
                            value2 = Track.get( track ).getCurve( curve_type.Name ).getValue( max );
                            edit.add( new BPPair( max, value2 ) );
                            command.vsqCommand = VsqCommand.generateCommandTrackCurveEdit( track, curve_type.Name, edit );
                        }
                    }
                    #endregion
                } else if ( command.type == CadenciiCommandType.BEZIER_CHAIN_DELETE ) {
                    #region DeleteBezierChain
                    int track = (int)command.args[0];
                    CurveType curve_type = (CurveType)command.args[1];
                    int chain_id = (int)command.args[2];
                    int clock_resolution = (int)command.args[3];
                    BezierChain chain = (BezierChain)AttachedCurves.get( track - 1 ).getBezierChain( curve_type, chain_id ).Clone();
                    AttachedCurves.get( track - 1 ).remove( curve_type, chain_id );
                    ret = generateCommandAddBezierChain( track, curve_type, chain_id, clock_resolution, chain );
                    if ( command.vsqCommand != null && ret != null ) {
                        if ( command.vsqCommand.Type == VsqCommandType.TRACK_CURVE_EDIT || command.vsqCommand.Type == VsqCommandType.TRACK_CURVE_EDIT_RANGE ) {
                            ret.vsqCommand = preprocessSpecialCommand( command.vsqCommand );
                        } else {
                            ret.vsqCommand = base.executeCommand( command.vsqCommand );
                        }
                    }
                    #endregion
                } else if ( command.type == CadenciiCommandType.BEZIER_CHAIN_REPLACE ) {
                    #region ReplaceBezierChain
                    int track = (int)command.args[0];
                    CurveType curve_type = (CurveType)command.args[1];
                    int chain_id = (int)command.args[2];
                    BezierChain chain = (BezierChain)command.args[3];
                    int clock_resolution = (int)command.args[4];
                    BezierChain target = (BezierChain)AttachedCurves.get( track - 1 ).getBezierChain( curve_type, chain_id ).Clone();
                    AttachedCurves.get( track - 1 ).setBezierChain( curve_type, chain_id, chain );
                    ret = generateCommandReplaceBezierChain( track, curve_type, chain_id, target, clock_resolution );
                    if ( chain.size() == 1 ) {
                        int ex_min = (int)chain.points.get( 0 ).getBase().X;
                        int ex_max = ex_min;
                        if ( target.points.size() > 1 ) {
                            for ( int i = 1; i < target.points.size(); i++ ) {
                                ex_min = Math.Min( ex_min, (int)target.points.get( i ).getBase().X );
                                ex_max = Math.Max( ex_max, (int)target.points.get( i ).getBase().X );
                            }
                            if ( ex_min < ex_max ) {
                                int default_value = curve_type.getDefault();
                                Vector<BPPair> edit = new Vector<BPPair>();
                                edit.add( new BPPair( ex_min, default_value ) );
                                edit.add( new BPPair( ex_max, default_value ) );
                                command.vsqCommand = VsqCommand.generateCommandTrackCurveEdit( track, curve_type.getName(), edit );
                            }
                        }
                    } else if ( chain.size() > 1 ) {
                        int min = (int)chain.points.get( 0 ).getBase().X;
                        int max = min;
                        for ( int i = 1; i < chain.points.size(); i++ ) {
                            min = Math.Min( min, (int)chain.points.get( i ).getBase().X );
                            max = Math.Max( max, (int)chain.points.get( i ).getBase().X );
                        }
                        int ex_min = min;
                        int ex_max = max;
                        if ( target.points.size() > 0 ) {
                            ex_min = (int)target.points.get( 0 ).getBase().X;
                            ex_max = ex_min;
                            for ( int i = 1; i < target.points.size(); i++ ) {
                                ex_min = Math.Min( ex_min, (int)target.points.get( i ).getBase().X );
                                ex_max = Math.Max( ex_max, (int)target.points.get( i ).getBase().X );
                            }
                        }
                        int max_value = curve_type.Maximum;
                        int min_value = curve_type.Minimum;
                        int default_value = curve_type.Default;
                        Vector<BPPair> edit = new Vector<BPPair>();
                        if ( ex_min < min ) {
                            edit.add( new BPPair( ex_min, default_value ) );
                        }
                        if ( min < max ) {
                            int last_value = int.MaxValue;
                            for ( int clock = min; clock < max; clock += clock_resolution ) {
                                int value = (int)chain.getValue( (float)clock );
                                if ( value < min_value ) {
                                    value = min_value;
                                } else if ( max_value < value ) {
                                    value = max_value;
                                }
                                if ( last_value != value ) {
                                    edit.add( new BPPair( clock, value ) );
                                    last_value = value;
                                }
                            }
                            int value2 = 0;
                            if ( max < ex_max ) {
                                value2 = (int)chain.getValue( max );
                                if ( value2 < min_value ) {
                                    value2 = min_value;
                                } else if ( max_value < value2 ) {
                                    value2 = max_value;
                                }
                            } else {
                                value2 = Track.get( track ).getCurve( curve_type.Name ).getValue( max );
                            }
                            edit.add( new BPPair( max, value2 ) );
                        }
                        if ( max < ex_max ) {
                            if ( max + 1 < ex_max ) {
                                edit.add( new BPPair( max + 1, default_value ) );
                            }
                            edit.add( new BPPair( ex_max, default_value ) );
                        }
                        if ( edit.size() > 0 ) {
                            command.vsqCommand = VsqCommand.generateCommandTrackCurveEdit( track, curve_type.Name, edit );
                        }
                    }
                    #endregion
                } else if ( command.type == CadenciiCommandType.REPLACE ) {
                    #region Replace
                    VsqFileEx vsq = (VsqFileEx)command.args[0];
                    VsqFileEx inv = (VsqFileEx)this.Clone();
                    Track.clear();
                    for ( int i = 0; i < vsq.Track.size(); i++ ) {
                        Track.add( (VsqTrack)vsq.Track.get( i ).Clone() );
                    }
                    TempoTable.clear();
                    for ( int i = 0; i < vsq.TempoTable.size(); i++ ) {
                        TempoTable.add( (TempoTableEntry)vsq.TempoTable.get( i ).Clone() );
                    }
                    TimesigTable.clear();
                    for ( int i = 0; i < vsq.TimesigTable.size(); i++ ) {
                        TimesigTable.add( (TimeSigTableEntry)vsq.TimesigTable.get( i ).Clone() );
                    }
                    m_tpq = vsq.m_tpq;
                    TotalClocks = vsq.TotalClocks;
                    m_base_tempo = vsq.m_base_tempo;
                    Master = (VsqMaster)vsq.Master.Clone();
                    Mixer = (VsqMixer)vsq.Mixer.Clone();
                    AttachedCurves = (AttachedCurve)vsq.AttachedCurves.Clone();
                    updateTotalClocks();
                    ret = generateCommandReplace( inv );
                    #endregion
                } else if ( command.type == CadenciiCommandType.ATTACHED_CURVE_REPLACE_RANGE ) {
                    #region ReplaceAttachedCurveRange
                    int track = (int)command.args[0];
                    TreeMap<CurveType, Vector<BezierChain>> curves = (TreeMap<CurveType, Vector<BezierChain>>)command.args[1];
                    TreeMap<CurveType, Vector<BezierChain>> inv = new TreeMap<CurveType, Vector<BezierChain>>();
                    for ( Iterator itr = curves.keySet().iterator(); itr.hasNext(); ){
                        CurveType ct = (CurveType)itr.next();
                        Vector<BezierChain> chains = new Vector<BezierChain>();
                        Vector<BezierChain> src = this.AttachedCurves.get( track - 1 ).get( ct );
                        for ( int i = 0; i < src.size(); i++ ){
                            chains.add( (BezierChain)src.get( i ).Clone() );
                        }
                        inv.put( ct, chains );

                        this.AttachedCurves.get( track - 1 ).get( ct ).clear();
                        for ( Iterator itr2 = curves.get( ct ).iterator(); itr2.hasNext(); ){
                            BezierChain bc = (BezierChain)itr2.next();
                            this.AttachedCurves.get( track - 1 ).get( ct ).add( bc );
                        }
                    }
                    ret = generateCommandReplaceAttachedCurveRange( track, inv );
                    #endregion
                } else if ( command.type == CadenciiCommandType.TRACK_ADD ) {
                    #region AddTrack
                    VsqTrack track = (VsqTrack)command.args[0];
                    VsqMixerEntry mixer = (VsqMixerEntry)command.args[1];
                    int position = (int)command.args[2];
                    BezierCurves attached_curve = (BezierCurves)command.args[3];
                    ret = VsqFileEx.generateCommandDeleteTrack( position );
                    if ( Track.size() <= 17 ) {
                        Track.insertElementAt( (VsqTrack)track.Clone(), position );
                        AttachedCurves.insertElementAt( position - 1, attached_curve );
                        Mixer.Slave.insertElementAt( (VsqMixerEntry)mixer.Clone(), position - 1 );
                    }
                    #endregion
                } else if ( command.type == CadenciiCommandType.TRACK_DELETE ) {
                    #region DeleteTrack
                    int track = (int)command.args[0];
                    ret = VsqFileEx.generateCommandAddTrack( Track.get( track ), Mixer.Slave.get( track - 1 ), track, AttachedCurves.get( track - 1 ) );
                    Track.removeElementAt( track );
                    AttachedCurves.removeElementAt( track - 1 );
                    Mixer.Slave.removeElementAt( track - 1 );
                    updateTotalClocks();
                    #endregion
                } else if ( command.type == CadenciiCommandType.TRACK_REPLACE ) {
                    #region TrackReplace
                    int track = (int)command.args[0];
                    VsqTrack item = (VsqTrack)command.args[1];
                    BezierCurves bezier_curves = (BezierCurves)command.args[2];
                    ret = VsqFileEx.generateCommandTrackReplace( track, Track.get( track ), AttachedCurves.get( track - 1 ) );
                    Track.set( track, item );
                    AttachedCurves.set( track - 1, bezier_curves );
                    updateTotalClocks();
                    #endregion
                } else if ( command.type == CadenciiCommandType.BGM_UPDATE ) {
                    #region BGM_UPDATE
                    Vector<BgmFile> list = (Vector<BgmFile>)command.args[0];
                    ret = VsqFileEx.generateCommandBgmUpdate( BgmFiles );
                    BgmFiles.clear();
                    int count = list.size();
                    for ( int i = 0; i < count; i++ ) {
                        BgmFiles.add( list.get( i ) );
                    }
                    #endregion
                }
                if ( command.vsqCommand != null && ret != null ) {
#if DEBUG
                    AppManager.debugWriteLine( "VsqFileEx.executeCommand; command.VsqCommand.Type=" + command.vsqCommand.Type );
#endif
                    if ( command.vsqCommand.Type == VsqCommandType.TRACK_CURVE_EDIT || command.vsqCommand.Type == VsqCommandType.TRACK_CURVE_EDIT_RANGE ) {
                        ret.vsqCommand = preprocessSpecialCommand( command.vsqCommand );
                    } else {
                        ret.vsqCommand = base.executeCommand( command.vsqCommand );
                    }
                }
            }
            return ret;
        }

        public VsqFileEx()
            : this( "Miku", 1, 4, 4, 500000 ) {
            Track.clear();
            TempoTable.clear();
            TimesigTable.clear();
        }

        public VsqFileEx( String singer, int pre_measure, int numerator, int denominator, int tempo ) :
            base( singer, pre_measure, numerator, denominator, tempo ) {
            AttachedCurves = new AttachedCurve();
            for ( int i = 1; i < Track.size(); i++ ) {
                AttachedCurves.add( new BezierCurves() );
            }
        }

        public VsqFileEx( String _fpath, Encoding encoding ) :
            base( _fpath, encoding ){
            AttachedCurves = new AttachedCurve();

            String xml = Path.Combine( Path.GetDirectoryName( _fpath ), Path.GetFileName( _fpath ) + ".xml" );
            if ( PortUtil.isFileExists( xml ) ) {
                AttachedCurve tmp = null;
                FileStream fs = null;
                try {
                    fs = new FileStream( xml, FileMode.Open );
                    tmp = (AttachedCurve)AppManager.xmlSerializerListBezierCurves.Deserialize( fs );
                } catch ( Exception ex ) {
                    bocoree.debug.push_log( "ex=" + ex );
                    // 1.4.xのxmlとして読み込みを試みる
                    if ( fs != null ) {
                        fs.Close();
                        fs = null;
                    }
                    Rescue14xXml rx = new Rescue14xXml();
                    tmp = rx.Rescue( xml, Track.size() - 1 );
                } finally {
                    if ( fs != null ) {
                        fs.Close();
                    }
                }
                if ( tmp != null ) {
                    for ( Iterator itr = tmp.Curves.iterator(); itr.hasNext(); ){
                        BezierCurves bc = (BezierCurves)itr.next();
                        foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                            Vector<BezierChain> list = bc.get( ct );
                            for ( int i = 0; i < list.size(); i++ ) {
                                list.get( i ).id = i + 1;
                                for ( int j = 0; j < list.get( i ).points.size(); j++ ) {
                                    list.get( i ).points.get( j ).ID = j + 1;
                                }
                            }
                        }
                    }
                    AttachedCurves = tmp;
                }
            } else {
                for ( int i = 1; i < Track.size(); i++ ) {
                    AttachedCurves.add( new BezierCurves() );
                }
            }

            // UTAUでエクスポートしたIconHandleは、IDS=UTAUとなっているので探知する
            for ( int i = 1; i < Track.size(); i++ ) {
                for ( Iterator itr = Track.get( i ).getSingerEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = (VsqEvent)itr.next();
                    if ( ve.ID.IconHandle.IDS.ToLower().Equals( "utau" ) ) {
                        Track.get( i ).getCommon().Version = "UTU000";
                        break;
                    }
                }
            }
        }

        public VsqFileEx( UstFile ust )
            : this( "Miku", 1, 4, 4, ust.getBaseTempo() ) {
            int clock_count = 480 * 4; //pre measure = 1、4分の4拍子としたので
            VsqBPList pitch = new VsqBPList( 0, -2400, 2400 );
            for ( Iterator itr = ust.getTrack( 0 ).getNoteEventIterator(); itr.hasNext(); ) {
                UstEvent ue = (UstEvent)itr.next();
                if ( ue.Lyric != "R" ) {
                    VsqID id = new VsqID( 0 );
                    id.Length = ue.Length;
                    String psymbol = "a";
                    if ( !SymbolTable.attatch( ue.Lyric, out psymbol ) ) {
                        psymbol = "a";
                    }
                    id.LyricHandle = new LyricHandle( ue.Lyric, psymbol );
                    id.Note = ue.Note;
                    id.type = VsqIDType.Anote;
                    VsqEvent ve = new VsqEvent( clock_count, id );
                    ve.UstEvent = (UstEvent)ue.Clone();
                    Track.get( 1 ).addEvent( ve );

                    if ( ue.Pitches != null ) {
                        // PBTypeクロックごとにデータポイントがある
                        int clock = clock_count - ue.PBType;
                        for ( int i = 0; i < ue.Pitches.Length; i++ ) {
                            clock += ue.PBType;
                            pitch.add( clock, (int)ue.Pitches[i] );
                        }
                    }
                }
                if ( ue.Tempo > 0.0f ) {
                    TempoTable.add( new TempoTableEntry( clock_count, (int)(60e6 / ue.Tempo), 0.0 ) );
                }
                clock_count += ue.Length;
            }
            updateTempoInfo();
            updateTotalClocks();
            updateTimesigInfo();
            reflectPitch( this, 1, pitch );
        }

        /// <summary>
        /// master==MasterPitchControl.Pitchの場合、m_pitchからPITとPBSを再構成。
        /// master==MasterPitchControl.PITandPBSの場合、PITとPBSからm_pitchを再構成
        /// </summary>
        public static void reflectPitch( VsqFile vsq, int track, VsqBPList pitch ) {
            //double offset = AttachedCurves[track - 1].MasterTuningInCent * 100;
            Vector<Integer> keyclocks = new Vector<Integer>( pitch.getKeys() );
            VsqBPList pit = new VsqBPList( 0, -8192, 8191 );
            VsqBPList pbs = new VsqBPList( 2, 0, 24 );
            int premeasure_clock = vsq.getPreMeasureClocks();
            int lastpit = pit.getDefault();
            int lastpbs = pbs.getDefault();
            int vpbs = 24;
            int vpit = 0;

            Vector<Integer> parts = new Vector<Integer>();   // 連続した音符ブロックの先頭音符のクロック位置。のリスト
            parts.add( premeasure_clock );
            int lastclock = premeasure_clock;
            for ( Iterator itr = vsq.Track.get( track ).getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = (VsqEvent)itr.next();
                if ( ve.Clock <= lastclock ) {
                    lastclock = Math.Max( lastclock, ve.Clock + ve.ID.Length );
                } else {
                    parts.add( ve.Clock );
                    lastclock = ve.Clock + ve.ID.Length;
                }
            }

            for ( int i = 0; i < parts.size(); i++ ) {
                int partstart = parts.get( i );
                int partend = int.MaxValue;
                if ( i + 1 < parts.size() ) {
                    partend = parts.get( i + 1 );
                }

                // まず、区間内の最大ピッチベンド幅を調べる
                double max = 0;
                for ( int j = 0; j < keyclocks.size(); j++ ) {
                    if ( keyclocks.get( j ) < partstart ) {
                        continue;
                    }
                    if ( partend <= keyclocks.get( j ) ) {
                        break;
                    }
                    max = Math.Max( max, Math.Abs( pitch.getValue( keyclocks.get( j ) ) / 10000.0 ) );
                }

                // 最大ピッチベンド幅を表現できる最小のPBSを計算
                vpbs = (int)(Math.Ceiling( max * 8192.0 / 8191.0 ) + 0.1);
                if ( vpbs <= 0 ) {
                    vpbs = 1;
                }
                double pitch2 = pitch.getValue( partstart ) / 10000.0;
                if ( lastpbs != vpbs ) {
                    pbs.add( partstart, vpbs );
                    lastpbs = vpbs;
                }
                vpit = (int)(pitch2 * 8192 / (double)vpbs);
                if ( lastpit != vpit ) {
                    pit.add( partstart, vpit );
                    lastpit = vpit;
                }
                for ( int j = 0; j < keyclocks.size(); j++ ) {
                    int clock = keyclocks.get( j );
                    if ( clock < partstart ) {
                        continue;
                    }
                    if ( partend <= clock ) {
                        break;
                    }
                    if ( clock != partstart ) {
                        pitch2 = pitch.getValue( clock ) / 10000.0;
                        vpit = (int)(pitch2 * 8192 / (double)vpbs);
                        if ( lastpit != vpit ) {
                            pit.add( clock, vpit );
                            lastpit = vpit;
                        }
                    }
                }
            }
            vsq.Track.get( track ).setCurve( "pit", pit );
            vsq.Track.get( track ).setCurve( "pbs", pbs );
        }

        public void writeAsXml( String file ) {
            //reflectPitch( MasterPitchControl.Pitch );
            using ( XmlTextWriter xw = new XmlTextWriter( file, Encoding.UTF8 ) ){
                xw.Indentation = 0;
                s_vsq_serializer.Serialize( xw, this );
            }
        }

        public static VsqFileEx readFromXml( String file ) {
            VsqFileEx ret = null;
            using ( FileStream fs = new FileStream( file, FileMode.Open, FileAccess.Read ) ) {
                ret = (VsqFileEx)s_vsq_serializer.Deserialize( fs );
            }

            if( ret == null ){
                return null;
            }
            // ベジエ曲線のIDを播番
            if ( ret.AttachedCurves != null ) {
                for ( Iterator itr = ret.AttachedCurves.Curves.iterator(); itr.hasNext(); ){
                    BezierCurves bc = (BezierCurves)itr.next();
                    foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                        Vector<BezierChain> list = bc.get( ct );
                        for ( int i = 0; i < list.size(); i++ ) {
                            list.get( i ).id = i + 1;
                            for ( int j = 0; j < list.get( i ).points.size(); j++ ) {
                                list.get( i ).points.get( j ).ID = j + 1;
                            }
                        }
                    }
                }
            } else {
                for ( int i = 1; i < ret.Track.size(); i++ ) {
                    ret.AttachedCurves.add( new BezierCurves() );
                }
            }
            return ret;
        }

        public override void write( String file ) {
            base.write( file );
        }

        public override void write( String file, int msPreSend, Encoding encoding ) {
            base.write( file, msPreSend, encoding );
        }

        public class Rescue14xXml {
            public class BezierCurves {
                public BezierChain[] Dynamics;
                public BezierChain[] Brethiness;
                public BezierChain[] Brightness;
                public BezierChain[] Clearness;
                public BezierChain[] Opening;
                public BezierChain[] GenderFactor;
                public BezierChain[] PortamentoTiming;
                public BezierChain[] PitchBend;
                public BezierChain[] PitchBendSensitivity;
                public BezierChain[] VibratoRate;
                public BezierChain[] VibratoDepth;

                public BezierCurves() {
                    Dynamics = new BezierChain[0];
                    Brethiness = new BezierChain[0];
                    Brightness = new BezierChain[0];
                    Clearness = new BezierChain[0];
                    Opening = new BezierChain[0];
                    GenderFactor = new BezierChain[0];
                    PortamentoTiming = new BezierChain[0];
                    PitchBend = new BezierChain[0];
                    PitchBendSensitivity = new BezierChain[0];
                    VibratoRate = new BezierChain[0];
                    VibratoDepth = new BezierChain[0];
                }
            }

            public Boare.Cadencii.AttachedCurve Rescue( String file, int num_track ) {
#if DEBUG
                AppManager.debugWriteLine( "VsqFileEx.Rescue14xXml.Rescue; file=" + file + "; num_track=" + num_track );
                bocoree.debug.push_log( "   constructing serializer..." );
#endif
                XmlSerializer xs = null;
                try {
                    xs = new XmlSerializer( typeof( Vector<Boare.Cadencii.VsqFileEx.Rescue14xXml.BezierCurves> ) );
                } catch ( Exception ex ) {
                    bocoree.debug.push_log( "    ex=" + ex );
                }
                if ( xs == null ) {
                    return null;
                }
#if DEBUG
                bocoree.debug.push_log( "    ...done" );
                bocoree.debug.push_log( "    constructing FileStream..." );
#endif
                FileStream fs = new FileStream( file, FileMode.Open );
#if DEBUG
                bocoree.debug.push_log( "    ...done" );
#endif
                AttachedCurve ac = null;
#if !DEBUG
                try {
#endif
#if DEBUG
                bocoree.debug.push_log( "    serializing..." );
#endif
                    Vector<Boare.Cadencii.VsqFileEx.Rescue14xXml.BezierCurves> list = (Vector<Boare.Cadencii.VsqFileEx.Rescue14xXml.BezierCurves>)xs.Deserialize( fs );
#if DEBUG
                    bocoree.debug.push_log( "    ...done" );
                    bocoree.debug.push_log( "    (list==null)=" + (list == null) );
                    bocoree.debug.push_log( "    list.Count=" + list.size() );
#endif
                    if ( list.size() >= num_track ) {
                        ac = new AttachedCurve();
                        ac.Curves = new Vector<Boare.Cadencii.BezierCurves>();
                        for ( int i = 0; i < num_track; i++ ) {
#if DEBUG
                            bocoree.debug.push_log( "    i=" + i );
#endif
                            Boare.Cadencii.BezierCurves add = new Boare.Cadencii.BezierCurves();
                            add.Brethiness = new Vector<BezierChain>( list.get( i ).Brethiness );
                            add.Brightness = new Vector<BezierChain>( list.get( i ).Brightness );
                            add.Clearness = new Vector<BezierChain>( list.get( i ).Clearness );
                            add.Dynamics = new Vector<BezierChain>();
                            foreach ( BezierChain bc in list.get( i ).Dynamics ) {
                                add.Dynamics.add( (BezierChain)bc.Clone() );
                            }
                            add.FX2Depth = new Vector<BezierChain>();
                            add.GenderFactor = new Vector<BezierChain>( list.get( i ).GenderFactor );
                            add.Harmonics = new Vector<BezierChain>();
                            add.Opening = new Vector<BezierChain>( list.get( i ).Opening );
                            add.PortamentoTiming = new Vector<BezierChain>( list.get( i ).PortamentoTiming );
                            add.Reso1Amp = new Vector<BezierChain>();
                            add.Reso2Amp = new Vector<BezierChain>();
                            add.Reso3Amp = new Vector<BezierChain>();
                            add.Reso4Amp = new Vector<BezierChain>();
                            add.Reso1BW = new Vector<BezierChain>();
                            add.Reso2BW = new Vector<BezierChain>();
                            add.Reso3BW = new Vector<BezierChain>();
                            add.Reso4BW = new Vector<BezierChain>();
                            add.Reso1Freq = new Vector<BezierChain>();
                            add.Reso2Freq = new Vector<BezierChain>();
                            add.Reso3Freq = new Vector<BezierChain>();
                            add.Reso4Freq = new Vector<BezierChain>();
                            add.VibratoDepth = new Vector<BezierChain>( list.get( i ).VibratoDepth );
                            add.VibratoRate = new Vector<BezierChain>( list.get( i ).VibratoRate );
                            ac.Curves.add( add );
                        }
                    }
#if !DEBUG
                } catch ( Exception ex ) {
                    bocoree.debug.push_log( "Rescue14xXml; ex=" + ex );
                    ac = null;
                } finally {
                    if ( fs != null ) {
                        fs.Close();
                    }
                }
#endif
                return ac;
            }
        }

    }

}
