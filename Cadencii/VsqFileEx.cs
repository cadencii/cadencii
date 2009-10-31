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
#if JAVA
package org.kbinani.Cadencii;

import java.io.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.vsq.*;
import org.kbinani.xml.*;
#else
using System;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.io;
using bocoree.util;
using bocoree.xml;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
    using Integer = Int32;
    using Long = System.Int64;
#endif

#if JAVA
    public class VsqFileEx extends VsqFile implements Cloneable, ICommandRunnable, Serializable{
#else
    [Serializable]
    public class VsqFileEx : VsqFile, ICloneable, ICommandRunnable {
#endif

        static XmlSerializer s_vsq_serializer;

        public AttachedCurve AttachedCurves;
        public Vector<BgmFile> BgmFiles = new Vector<BgmFile>();
#if !JAVA
        [System.Xml.Serialization.XmlIgnore]
#endif
        public EditorStatus editorStatus = new EditorStatus();

#if JAVA
        static
        {
            s_vsq_serializer = new XmlSerializer( VsqFileEx.class );
        }
#else
        static VsqFileEx() {
            s_vsq_serializer = new XmlSerializer( typeof( VsqFileEx ) );
        }
#endif

        /// <summary>
        /// VsqEvent, VsqBPList, BezierCurvesの全てのクロックを、tempoに格納されているテンポテーブルに
        /// 合致するようにシフトします
        /// </summary>
        /// <param name="work"></param>
        /// <param name="tempo"></param>
        public void adjustClockToMatchWith( VsqFile tempo ) {
            double premeasure_sec_target = getSecFromClock( getPreMeasureClocks() );
            double premeasure_sec_tempo = premeasure_sec_target;
#if DEBUG
            PortUtil.println( "FormMain#ShiftClockToMatchWith; premeasure_sec_target=" + premeasure_sec_target + "; premeasre_sec_tempo=" + premeasure_sec_tempo );
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
                for ( int j = 0; j < AppManager.CURVE_USAGE.Length; j++ ) {
                    CurveType ct = AppManager.CURVE_USAGE[j];
                    VsqBPList item = this.Track.get( track ).getCurve( ct.getName() );
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
                    this.Track.get( track ).setCurve( ct.getName(), repl );
                }

                // ベジエカーブをシフト
                for ( int i = 0; i < AppManager.CURVE_USAGE.Length; i++ ) {
                    CurveType ct = AppManager.CURVE_USAGE[i];
                    Vector<BezierChain> list = this.AttachedCurves.get( track - 1 ).get( ct );
                    if ( list == null ) {
                        continue;
                    }
                    for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                        BezierChain chain = (BezierChain)itr.next();
                        for ( Iterator itr2 = chain.points.iterator(); itr2.hasNext(); ) {
                            BezierPoint point = (BezierPoint)itr2.next();
                            PointD bse = new PointD( tempo.getClockFromSec( this.getSecFromClock( point.getBase().getX() ) - premeasure_sec_target + premeasure_sec_tempo ),
                                                     point.getBase().getY() );
                            double rx = point.getBase().getX() + point.controlRight.getX();
                            double new_rx = tempo.getClockFromSec( this.getSecFromClock( rx ) - premeasure_sec_target + premeasure_sec_tempo );
                            PointD ctrl_r = new PointD( new_rx - bse.getX(), point.controlRight.getY() );

                            double lx = point.getBase().getX() + point.controlLeft.getX();
                            double new_lx = tempo.getClockFromSec( this.getSecFromClock( lx ) - premeasure_sec_target + premeasure_sec_tempo );
                            PointD ctrl_l = new PointD( new_lx - bse.getX(), point.controlLeft.getY() );
                            point.setBase( bse );
                            point.controlLeft = ctrl_l;
                            point.controlRight = ctrl_r;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 指定秒数分，アイテムの時間をずらす．
        /// </summary>
        /// <param name="vsq">編集対象</param>
        /// <param name="sec">ずらす秒数．正の場合アイテムは後ろにずれる</param>
        /// <param name="first_tempo">ずらす秒数が正の場合に，最初のテンポをいくらにするか</param>
        public static void shift( VsqFileEx vsq, double sec, int first_tempo ) {
            bool first = true; // 負になった最初のアイテムかどうか

            // 最初にテンポをずらす．
            // 古いのから情報をコピー
            VsqFile tempo = new VsqFile( "Miku", vsq.getPreMeasure(), 4, 4, 500000 );
            tempo.TempoTable.clear();
            for ( Iterator itr = vsq.TempoTable.iterator(); itr.hasNext(); ) {
                TempoTableEntry item = (TempoTableEntry)itr.next();
                tempo.TempoTable.add( item );
            }
            tempo.updateTempoInfo();
            int tempo_count = tempo.TempoTable.size();
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

                        if ( clock < pre_measure_clocks ) {
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
                for ( int k = 0; k < AppManager.CURVE_USAGE.Length; k++ ) {
                    CurveType ct = AppManager.CURVE_USAGE[k];
                    VsqBPList item = track.getCurve( ct.getName() );
                    if ( item == null ) {
                        continue;
                    }
                    VsqBPList repl = new VsqBPList( item.getDefault(), item.getMinimum(), item.getMaximum() );
                    int c = item.size();
                    first = true;
                    for ( int j = c - 1; j >= 0; j-- ) {
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
                for ( int k = 0; k < AppManager.CURVE_USAGE.Length; k++ ) {
                    CurveType ct = AppManager.CURVE_USAGE[k];
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
                            PointD bse = new PointD( vsq.getClockFromSec( vsq.getSecFromClock( point.getBase().getX() ) + sec ),
                                                     point.getBase().getY() );
                            double rx = point.getBase().getX() + point.controlRight.getX();
                            double new_rx = vsq.getClockFromSec( vsq.getSecFromClock( rx ) + sec );
                            PointD ctrl_r = new PointD( new_rx - bse.getX(), point.controlRight.getY() );

                            double lx = point.getBase().getX() + point.controlLeft.getX();
                            double new_lx = vsq.getClockFromSec( vsq.getSecFromClock( lx ) + sec );
                            PointD ctrl_l = new PointD( new_lx - bse.getX(), point.controlLeft.getY() );
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
                        for ( int m = 0; m < list_count; m++ ) {
                            if ( id == list.get( m ).id ) {
                                list.removeElementAt( m );
                                break;
                            }
                        }
                    }
                }
            }
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public Object clone() {
            VsqFileEx ret = new VsqFileEx( "Miku", 1, 4, 4, 500000 );
            ret.Track = new Vector<VsqTrack>();
            int c = Track.size();
            for ( int i = 0; i < c; i++ ) {
                ret.Track.add( (VsqTrack)Track.get( i ).clone() );
            }
            ret.TempoTable = new Vector<TempoTableEntry>();
            c = TempoTable.size();
            for ( int i = 0; i < c; i++ ) {
                ret.TempoTable.add( (TempoTableEntry)TempoTable.get( i ).clone() );
            }
            ret.TimesigTable = new Vector<TimeSigTableEntry>();
            c = TimesigTable.size();
            for ( int i = 0; i < c; i++ ) {
                ret.TimesigTable.add( (TimeSigTableEntry)TimesigTable.get( i ).clone() );
            }
            ret.m_tpq = m_tpq;
            ret.TotalClocks = TotalClocks;
            ret.m_base_tempo = m_base_tempo;
            ret.Master = (VsqMaster)Master.clone();
            ret.Mixer = (VsqMixer)Mixer.clone();
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
            command.args[1] = item.clone();
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
            command.args[0] = track.clone();
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
                ret.vsqCommand = base.executeCommand( command.vsqCommand );

                // 再レンダリングが必要になったかどうかを判定
                VsqCommandType type = command.vsqCommand.Type;
                if ( type == VsqCommandType.CHANGE_PRE_MEASURE ||
                     type == VsqCommandType.REPLACE ||
                     type == VsqCommandType.UPDATE_TEMPO ||
                     type == VsqCommandType.UPDATE_TEMPO_RANGE ) {
                    int count = Track.size();
                    for ( int i = 0; i < count - 1; i++ ) {
                        editorStatus.renderRequired[i] = true;
                    }
                } else if ( type == VsqCommandType.EVENT_ADD ||
                            type == VsqCommandType.EVENT_ADD_RANGE ||
                            type == VsqCommandType.EVENT_CHANGE_ACCENT ||
                            type == VsqCommandType.EVENT_CHANGE_CLOCK ||
                            type == VsqCommandType.EVENT_CHANGE_CLOCK_AND_ID_CONTAINTS ||
                            type == VsqCommandType.EVENT_CHANGE_CLOCK_AND_ID_CONTAINTS_RANGE ||
                            type == VsqCommandType.EVENT_CHANGE_CLOCK_AND_LENGTH ||
                            type == VsqCommandType.EVENT_CHANGE_CLOCK_AND_NOTE ||
                            type == VsqCommandType.EVENT_CHANGE_DECAY ||
                            type == VsqCommandType.EVENT_CHANGE_ID_CONTAINTS ||
                            type == VsqCommandType.EVENT_CHANGE_ID_CONTAINTS_RANGE ||
                            type == VsqCommandType.EVENT_CHANGE_LENGTH ||
                            type == VsqCommandType.EVENT_CHANGE_LYRIC ||
                            type == VsqCommandType.EVENT_CHANGE_NOTE ||
                            type == VsqCommandType.EVENT_CHANGE_VELOCITY ||
                            type == VsqCommandType.EVENT_DELETE ||
                            type == VsqCommandType.EVENT_DELETE_RANGE ||
                            type == VsqCommandType.EVENT_REPLACE ||
                            type == VsqCommandType.EVENT_REPLACE_RANGE ||
                            type == VsqCommandType.TRACK_CURVE_EDIT ||
                            type == VsqCommandType.TRACK_CURVE_EDIT_RANGE ||
                            type == VsqCommandType.TRACK_CURVE_EDIT2 ||
                            type == VsqCommandType.TRACK_CURVE_REPLACE ||
                            type == VsqCommandType.TRACK_CURVE_REPLACE_RANGE ||
                            type == VsqCommandType.TRACK_REPLACE ) {
                    int track = (Integer)command.vsqCommand.Args[0];
                    editorStatus.renderRequired[track - 1] = true;
                } else if ( type == VsqCommandType.TRACK_ADD ) {
                    int position = (Integer)command.vsqCommand.Args[2];
                    for ( int i = 15; i >= position; i-- ) {
                        editorStatus.renderRequired[i] = editorStatus.renderRequired[i - 1];
                    }
                    editorStatus.renderRequired[position - 1] = true;
                } else if ( type == VsqCommandType.TRACK_DELETE ) {
                    int track = (Integer)command.vsqCommand.Args[0];
                    for ( int i = track - 1; i < 15; i++ ) {
                        editorStatus.renderRequired[i] = editorStatus.renderRequired[i + 1];
                    }
                    editorStatus.renderRequired[15] = false;
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
                    int clock_resolution = (Integer)command.args[3];
                    int added_id = (Integer)command.args[4];
                    AttachedCurves.get( track - 1 ).addBezierChain( curve_type, chain, added_id );
                    ret = generateCommandDeleteBezierChain( track, curve_type, added_id, clock_resolution );
                    if ( chain.size() >= 1 ) {
                        // ベジエ曲線が，時間軸方向のどの範囲にわたって指定されているか判定
                        int min = (int)chain.points.get( 0 ).getBase().getX();
                        int max = min;
                        int points_size = chain.points.size();
                        for ( int i = 1; i < points_size; i++ ) {
                            int x = (int)chain.points.get( i ).getBase().getX();
                            min = Math.Min( min, x );
                            max = Math.Max( max, x );
                        }

                        int max_value = curve_type.getMaximum();
                        int min_value = curve_type.getMinimum();
                        VsqBPList list = Track.get( track ).getCurve( curve_type.getName() );
                        if ( min <= max && list != null ) {
                            // minクロック以上maxクロック以下のコントロールカーブに対して，編集を実行

                            // 最初に，min <= clock <= maxとなる範囲のデータ点を抽出（削除コマンドに指定）
                            Vector<Long> delete = new Vector<Long>();
                            int list_size = list.size();
                            for ( int i = 0; i < list_size; i++ ) {
                                int clock = list.getKeyClock( i );
                                if ( min <= clock && clock <= max ) {
                                    VsqBPPair item = list.getElementB( i );
                                    delete.add( item.id );
                                }
                            }

                            // 追加するデータ点を列挙
                            TreeMap<Integer, VsqBPPair> add = new TreeMap<Integer, VsqBPPair>();
                            if ( chain.points.size() == 1 ) {
                                BezierPoint p = chain.points.get( 0 );
                                add.put( (int)p.getBase().getX(), new VsqBPPair( (int)p.getBase().getY(), list.getMaxID() + 1 ) );
                            } else {
                                int last_value = int.MaxValue;
                                int index = 0;
                                for ( int clock = min; clock <= max; clock += clock_resolution ) {
                                    int value = (int)chain.getValue( (float)clock );
                                    if ( value < min_value ) {
                                        value = min_value;
                                    } else if ( max_value < value ) {
                                        value = max_value;
                                    }
                                    if ( value != last_value ) {
#if DEBUG
                                        PortUtil.println( "VsqFileEx#executeCommand; clock,value=" + clock + "," + value );
#endif
                                        index++;
                                        add.put( clock, new VsqBPPair( value, list.getMaxID() + index ) );
                                        last_value = value;
                                    }
                                }
                            }
                            command.vsqCommand = VsqCommand.generateCommandTrackCurveEdit2( track, curve_type.getName(), delete, add );
                        }
                    }

                    editorStatus.renderRequired[track - 1] = true;
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
                    int points_size = chain.points.size();
                    int min = (int)chain.points.get( 0 ).getBase().getX();
                    int max = min;
                    for ( int i = 1; i < points_size; i++ ) {
                        int x = (int)chain.points.get( i ).getBase().getX();
                        min = Math.Min( min, x );
                        max = Math.Max( max, x );
                    }
                    VsqBPList list = Track.get( track ).getCurve( curve_type.getName() );
                    int list_size = list.size();
                    Vector<Long> delete = new Vector<Long>();
                    for ( int i = 0; i < list_size; i++ ) {
                        int clock = list.getKeyClock( i );
                        if ( min <= clock && clock <= max ) {
                            delete.add( list.getElementB( i ).id );
                        } else if ( max < clock ) {
                            break;
                        }
                    }
                    command.vsqCommand = VsqCommand.generateCommandTrackCurveEdit2( track, curve_type.getName(), delete, new TreeMap<Integer, VsqBPPair>() );
                    editorStatus.renderRequired[track - 1] = true;
                    #endregion
                } else if ( command.type == CadenciiCommandType.BEZIER_CHAIN_REPLACE ) {
                    #region ReplaceBezierChain
                    int track = (Integer)command.args[0];
                    CurveType curve_type = (CurveType)command.args[1];
                    int chain_id = (Integer)command.args[2];
                    BezierChain chain = (BezierChain)command.args[3];
                    int clock_resolution = (int)command.args[4];
                    BezierChain target = (BezierChain)AttachedCurves.get( track - 1 ).getBezierChain( curve_type, chain_id ).Clone();
                    AttachedCurves.get( track - 1 ).setBezierChain( curve_type, chain_id, chain );
                    VsqBPList list = Track.get( track ).getCurve( curve_type.getName() );
                    ret = generateCommandReplaceBezierChain( track, curve_type, chain_id, target, clock_resolution );
                    if ( chain.size() == 1 ) {
                        // リプレース後のベジエ曲線が，1個のデータ点のみからなる場合
                        int ex_min = (int)chain.points.get( 0 ).getBase().getX();
                        int ex_max = ex_min;
                        if ( target.points.size() > 1 ) {
                            // リプレースされる前のベジエ曲線が，どの時間範囲にあったか？
                            int points_size = target.points.size();
                            for ( int i = 1; i < points_size; i++ ) {
                                int x = (int)target.points.get( i ).getBase().getX();
                                ex_min = Math.Min( ex_min, x );
                                ex_max = Math.Max( ex_max, x );
                            }
                            if ( ex_min < ex_max ) {
                                // ex_min以上ex_max以下の範囲にあるデータ点を消す
                                Vector<Long> delete = new Vector<Long>();
                                int list_size = list.size();
                                for ( int i = 0; i < list_size; i++ ) {
                                    int clock = list.getKeyClock( i );
                                    if ( ex_min <= clock && clock <= ex_max ) {
                                        delete.add( list.getElementB( i ).id );
                                    }
                                    if ( ex_max < clock ) {
                                        break;
                                    }
                                }

                                // リプレース後のデータ点は1個だけ
                                TreeMap<Integer, VsqBPPair> add = new TreeMap<Integer, VsqBPPair>();
                                PointD p = chain.points.get( 0 ).getBase();
                                add.put( (int)p.getX(), new VsqBPPair( (int)p.getY(), list.getMaxID() + 1 ) );

                                command.vsqCommand = VsqCommand.generateCommandTrackCurveEdit2( track, curve_type.getName(), delete, add );
                            }
                        }
                    } else if ( chain.size() > 1 ) {
                        // リプレース後のベジエ曲線の範囲
                        int min = (int)chain.points.get( 0 ).getBase().getX();
                        int max = min;
                        int points_size = chain.points.size();
                        for ( int i = 1; i < points_size; i++ ) {
                            int x = (int)chain.points.get( i ).getBase().getX();
                            min = Math.Min( min, x );
                            max = Math.Max( max, x );
                        }

                        // リプレース前のベジエ曲線の範囲
                        int ex_min = min;
                        int ex_max = max;
                        if ( target.points.size() > 0 ) {
                            ex_min = (int)target.points.get( 0 ).getBase().getX();
                            ex_max = ex_min;
                            int target_points_size = target.points.size();
                            for ( int i = 1; i < target_points_size; i++ ) {
                                int x = (int)target.points.get( i ).getBase().getX();
                                ex_min = Math.Min( ex_min, x );
                                ex_max = Math.Max( ex_max, x );
                            }
                        }

                        // 削除するのを列挙
                        Vector<Long> delete = new Vector<Long>();
                        int list_size = list.size();
                        for ( int i = 0; i < list_size; i++ ) {
                            int clock = list.getKeyClock( i );
                            if ( ex_min <= clock && clock <= ex_max ) {
                                delete.add( list.getElementB( i ).id );
                            }
                            if ( ex_max < clock ) {
                                break;
                            }
                        }

                        // 追加するのを列挙
                        int max_value = curve_type.getMaximum();
                        int min_value = curve_type.getMinimum();
                        TreeMap<Integer, VsqBPPair> add = new TreeMap<Integer, VsqBPPair>();
                        if ( min < max ) {
                            int last_value = int.MaxValue;
                            int index = 0;
                            for ( int clock = min; clock < max; clock += clock_resolution ) {
                                int value = (int)chain.getValue( (float)clock );
                                if ( value < min_value ) {
                                    value = min_value;
                                } else if ( max_value < value ) {
                                    value = max_value;
                                }
                                if ( last_value != value ) {
                                    index++;
                                    add.put( clock, new VsqBPPair( value, list.getMaxID() + index ) );
                                }
                            }
                        }
                        command.vsqCommand = VsqCommand.generateCommandTrackCurveEdit2( track, curve_type.getName(), delete, add );
                    }

                    editorStatus.renderRequired[track - 1] = true;
                    #endregion
                } else if ( command.type == CadenciiCommandType.REPLACE ) {
                    #region Replace
                    VsqFileEx vsq = (VsqFileEx)command.args[0];
                    VsqFileEx inv = (VsqFileEx)this.Clone();
                    Track.clear();
                    int c = vsq.Track.size();
                    for ( int i = 0; i < c; i++ ) {
                        Track.add( (VsqTrack)vsq.Track.get( i ).clone() );
                    }
                    TempoTable.clear();
                    c = vsq.TempoTable.size();
                    for ( int i = 0; i < c; i++ ) {
                        TempoTable.add( (TempoTableEntry)vsq.TempoTable.get( i ).clone() );
                    }
                    TimesigTable.clear();
                    c = vsq.TimesigTable.size();
                    for ( int i = 0; i < c; i++ ) {
                        TimesigTable.add( (TimeSigTableEntry)vsq.TimesigTable.get( i ).clone() );
                    }
                    m_tpq = vsq.m_tpq;
                    TotalClocks = vsq.TotalClocks;
                    m_base_tempo = vsq.m_base_tempo;
                    Master = (VsqMaster)vsq.Master.clone();
                    Mixer = (VsqMixer)vsq.Mixer.clone();
                    AttachedCurves = (AttachedCurve)vsq.AttachedCurves.Clone();
                    updateTotalClocks();
                    ret = generateCommandReplace( inv );

                    int count = Track.size();
                    for ( int i = 0; i < count - 1; i++ ) {
                        editorStatus.renderRequired[i] = true;
                    }
                    for ( int i = count - 1; i < 16; i++ ) {
                        editorStatus.renderRequired[i] = false;
                    }
                    #endregion
                } else if ( command.type == CadenciiCommandType.ATTACHED_CURVE_REPLACE_RANGE ) {
                    #region ReplaceAttachedCurveRange
                    int track = (int)command.args[0];
                    TreeMap<CurveType, Vector<BezierChain>> curves = (TreeMap<CurveType, Vector<BezierChain>>)command.args[1];
                    TreeMap<CurveType, Vector<BezierChain>> inv = new TreeMap<CurveType, Vector<BezierChain>>();
                    for ( Iterator itr = curves.keySet().iterator(); itr.hasNext(); ) {
                        CurveType ct = (CurveType)itr.next();
                        Vector<BezierChain> chains = new Vector<BezierChain>();
                        Vector<BezierChain> src = this.AttachedCurves.get( track - 1 ).get( ct );
                        for ( int i = 0; i < src.size(); i++ ) {
                            chains.add( (BezierChain)src.get( i ).Clone() );
                        }
                        inv.put( ct, chains );

                        this.AttachedCurves.get( track - 1 ).get( ct ).clear();
                        for ( Iterator itr2 = curves.get( ct ).iterator(); itr2.hasNext(); ) {
                            BezierChain bc = (BezierChain)itr2.next();
                            this.AttachedCurves.get( track - 1 ).get( ct ).add( bc );
                        }
                    }
                    ret = generateCommandReplaceAttachedCurveRange( track, inv );

                    editorStatus.renderRequired[track - 1] = true;
                    #endregion
                } else if ( command.type == CadenciiCommandType.TRACK_ADD ) {
                    #region AddTrack
                    VsqTrack track = (VsqTrack)command.args[0];
                    VsqMixerEntry mixer = (VsqMixerEntry)command.args[1];
                    int position = (int)command.args[2];
                    BezierCurves attached_curve = (BezierCurves)command.args[3];
                    ret = VsqFileEx.generateCommandDeleteTrack( position );
                    if ( Track.size() <= 17 ) {
                        Track.insertElementAt( (VsqTrack)track.clone(), position );
                        AttachedCurves.insertElementAt( position - 1, attached_curve );
                        Mixer.Slave.insertElementAt( (VsqMixerEntry)mixer.clone(), position - 1 );
                    }

                    for ( int i = 15; i >= position; i-- ) {
                        editorStatus.renderRequired[i] = editorStatus.renderRequired[i - 1];
                    }
                    editorStatus.renderRequired[position - 1] = true;
                    #endregion
                } else if ( command.type == CadenciiCommandType.TRACK_DELETE ) {
                    #region DeleteTrack
                    int track = (int)command.args[0];
                    ret = VsqFileEx.generateCommandAddTrack( Track.get( track ), Mixer.Slave.get( track - 1 ), track, AttachedCurves.get( track - 1 ) );
                    Track.removeElementAt( track );
                    AttachedCurves.removeElementAt( track - 1 );
                    Mixer.Slave.removeElementAt( track - 1 );
                    updateTotalClocks();

                    for ( int i = track - 1; i < 15; i++ ) {
                        editorStatus.renderRequired[i] = editorStatus.renderRequired[i + 1];
                    }
                    editorStatus.renderRequired[15] = false;
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

                    editorStatus.renderRequired[track - 1] = true;
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
                    ret.vsqCommand = base.executeCommand( command.vsqCommand );
                }
            }
            return ret;
        }

#if JAVA
        public VsqFileEx()
        {
            this( "Miku", 1, 4, 4, 500000 );
#else
        public VsqFileEx()
            : this( "Miku", 1, 4, 4, 500000 ) {
#endif
            Track.clear();
            TempoTable.clear();
            TimesigTable.clear();
        }

#if JAVA
        public VsqFileEx( String singer, int pre_measure, int numerator, int denominator, int tempo )
        {
            super( singer, pre_measure, numerator, denominator, tempo );
#else
        public VsqFileEx( String singer, int pre_measure, int numerator, int denominator, int tempo ) :
            base( singer, pre_measure, numerator, denominator, tempo ) {
#endif
            AttachedCurves = new AttachedCurve();
            int count = Track.size();
            for ( int i = 1; i < count; i++ ) {
                AttachedCurves.add( new BezierCurves() );
            }
        }

#if JAVA
        public VsqFileEx( String _fpath, String encoding ){
            super( _fpath, encoding );
#else
        public VsqFileEx( String _fpath, String encoding ) :
            base( _fpath, encoding ) {
#endif
            AttachedCurves = new AttachedCurve();

            String xml = PortUtil.combinePath( PortUtil.getDirectoryName( _fpath ), PortUtil.getFileName( _fpath ) + ".xml" );
            if ( PortUtil.isFileExists( xml ) ) {
                AttachedCurve tmp = null;
                FileInputStream fs = null;
                try {
                    fs = new FileInputStream( xml );
                    tmp = (AttachedCurve)AppManager.xmlSerializerListBezierCurves.deserialize( fs );
                } catch ( Exception ex ) {
                    bocoree.debug.push_log( "ex=" + ex );
                    // 1.4.xのxmlとして読み込みを試みる
                    if ( fs != null ) {
                        fs.Close();
                        fs = null;
                    }
                    //Rescue14xXml rx = new Rescue14xXml();
                    //tmp = rx.Rescue( xml, Track.size() - 1 );
                } finally {
                    if ( fs != null ) {
                        fs.Close();
                    }
                }
                if ( tmp != null ) {
                    for ( Iterator itr = tmp.Curves.iterator(); itr.hasNext(); ) {
                        BezierCurves bc = (BezierCurves)itr.next();
                        for ( int k = 0; k < AppManager.CURVE_USAGE.Length; k++ ) {
                            CurveType ct = AppManager.CURVE_USAGE[k];
                            Vector<BezierChain> list = bc.get( ct );
                            for ( int i = 0; i < list.size(); i++ ) {
                                list.get( i ).id = i + 1;
                                for ( int j = 0; j < list.get( i ).points.size(); j++ ) {
                                    list.get( i ).points.get( j ).setID( j + 1 );
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
            int count = Track.size();
            for ( int i = 1; i < count; i++ ) {
                VsqTrack track = Track.get( i );
                for ( Iterator itr = track.getSingerEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = (VsqEvent)itr.next();
                    if ( ve.ID.IconHandle.IDS.ToLower().Equals( "utau" ) ) {
                        track.getCommon().Version = "UTU000";
                        break;
                    }
                }
            }
        }

#if JAVA
        public VsqFileEx( UstFile ust )
        {
            this( "Miku", 1, 4, 4, ust.getBaseTempo() );
#else
        public VsqFileEx( UstFile ust )
            : this( "Miku", 1, 4, 4, ust.getBaseTempo() ) {
#endif
            int clock_count = 480 * 4; //pre measure = 1、4分の4拍子としたので
            VsqBPList pitch = new VsqBPList( 0, -2400, 2400 );
            for ( Iterator itr = ust.getTrack( 0 ).getNoteEventIterator(); itr.hasNext(); ) {
                UstEvent ue = (UstEvent)itr.next();
                if ( ue.Lyric != "R" ) {
                    VsqID id = new VsqID( 0 );
                    id.Length = ue.Length;
                    ByRef<String> psymbol = new ByRef<String>( "a" );
                    if ( !SymbolTable.attatch( ue.Lyric, psymbol ) ) {
                        psymbol.value = "a";
                    }
                    id.LyricHandle = new LyricHandle( ue.Lyric, psymbol.value );
                    id.Note = ue.Note;
                    id.type = VsqIDType.Anote;
                    VsqEvent ve = new VsqEvent( clock_count, id );
                    ve.UstEvent = (UstEvent)ue.clone();
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
            //Vector<Integer> keyclocks = new Vector<Integer>( pitch.getKeys() );
            int keyclock_size = pitch.size();
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

            int parts_size = parts.size();
            for ( int i = 0; i < parts_size; i++ ) {
                int partstart = parts.get( i );
                int partend = int.MaxValue;
                if ( i + 1 < parts.size() ) {
                    partend = parts.get( i + 1 );
                }

                // まず、区間内の最大ピッチベンド幅を調べる
                double max = 0;
                for ( int j = 0; j < keyclock_size; j++ ) {
                    int clock = pitch.getKeyClock( j );
                    if ( clock < partstart ) {
                        continue;
                    }
                    if ( partend <= clock ) {
                        break;
                    }
                    max = Math.Max( max, Math.Abs( pitch.getValue( clock ) / 10000.0 ) );
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
                for ( int j = 0; j < keyclock_size; j++ ) {
                    int clock = pit.getKeyClock( j );
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
            FileOutputStream xw = null;
            try {
                xw = new FileOutputStream( file );
                s_vsq_serializer.serialize( xw, this );
            } catch ( Exception ex ) {
            } finally {
                if ( xw != null ) {
                    try {
                        xw.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        public static VsqFileEx readFromXml( String file ) {
            VsqFileEx ret = null;
            FileInputStream fs = null;
            try {
                fs = new FileInputStream( file );
                ret = (VsqFileEx)s_vsq_serializer.deserialize( fs );
            } catch ( Exception ex ) {
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }

            if ( ret == null ) {
                return null;
            }
            // ベジエ曲線のIDを播番
            if ( ret.AttachedCurves != null ) {
                for ( Iterator itr = ret.AttachedCurves.Curves.iterator(); itr.hasNext(); ) {
                    BezierCurves bc = (BezierCurves)itr.next();
                    for ( int k = 0; k < AppManager.CURVE_USAGE.Length; k++ ) {
                        CurveType ct = AppManager.CURVE_USAGE[k];
                        Vector<BezierChain> list = bc.get( ct );
                        int list_size = list.size();
                        for ( int i = 0; i < list_size; i++ ) {
                            BezierChain chain = list.get( i );
                            chain.id = i + 1;
                            int points_size = chain.points.size();
                            for ( int j = 0; j < points_size; j++ ) {
                                chain.points.get( j ).setID( j + 1 );
                            }
                        }
                    }
                }
            } else {
                int count = ret.Track.size();
                for ( int i = 1; i < count; i++ ) {
                    ret.AttachedCurves.add( new BezierCurves() );
                }
            }
            return ret;
        }

        public void write( String file ) {
            base.write( file );
        }

        public void write( String file, int msPreSend, String encoding ) {
            base.write( file, msPreSend, encoding );
        }
    }

    /*public class Rescue14xXml
    {
        public class BezierCurves
        {
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

            public BezierCurves()
            {
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

        public Boare.Cadencii.AttachedCurve Rescue( String file, int num_track )
        {
#if DEBUG
            AppManager.debugWriteLine( "VsqFileEx.Rescue14xXml.Rescue; file=" + file + "; num_track=" + num_track );
            bocoree.debug.push_log( "   constructing serializer..." );
#endif
            XmlSerializer xs = null;
            try
            {
#if JAVA
                    Vector<org.kbinani.Cadencii.VsqFileEx.Rescue14xXml.BezierCurves> dum = new Vector<org.kbinani.Cadencii.VsqFileEx.Rescue14xXml.BezierCurves>();
                    xs = new XmlSerializer( dum.getClass() );
#else
                xs = new XmlSerializer( typeof( Vector<Boare.Cadencii.Rescue14xXml.BezierCurves> ) );
#endif
            }
            catch ( Exception ex )
            {
                bocoree.debug.push_log( "    ex=" + ex );
            }
            if ( xs == null )
            {
                return null;
            }
#if DEBUG
            bocoree.debug.push_log( "    ...done" );
            bocoree.debug.push_log( "    constructing FileStream..." );
#endif
            FileInputStream fs = new FileInputStream( file );
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
            Vector<Boare.Cadencii.Rescue14xXml.BezierCurves> list = (Vector<Boare.Cadencii.Rescue14xXml.BezierCurves>)xs.deserialize( fs );
#if DEBUG
            bocoree.debug.push_log( "    ...done" );
            bocoree.debug.push_log( "    (list==null)=" + (list == null) );
            bocoree.debug.push_log( "    list.Count=" + list.size() );
#endif
            if ( list.size() >= num_track )
            {
                ac = new AttachedCurve();
                ac.Curves = new Vector<Boare.Cadencii.BezierCurves>();
                for ( int i = 0; i < num_track; i++ )
                {
#if DEBUG
                    bocoree.debug.push_log( "    i=" + i );
#endif
                    Boare.Cadencii.BezierCurves add = new Boare.Cadencii.BezierCurves();
                    add.Brethiness = new Vector<BezierChain>( list.get( i ).Brethiness );
                    add.Brightness = new Vector<BezierChain>( list.get( i ).Brightness );
                    add.Clearness = new Vector<BezierChain>( list.get( i ).Clearness );
                    add.Dynamics = new Vector<BezierChain>();
                    for ( int k = 0; k < list.get( i ).Dynamics.Length; k++ )
                    {
                        BezierChain bc = list.get( i ).Dynamics[k];
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
    }*/
#if !JAVA
}
#endif
