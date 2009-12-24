/*
 * VsqFileEx.cs
 * Copyright (C) 2008-2009 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import java.io.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.vsq.*;
import org.kbinani.xml.*;
#else
using System;
using org.kbinani.vsq;
using org.kbinani;
using org.kbinani.java.io;
using org.kbinani.java.util;
using org.kbinani.xml;

namespace org.kbinani.cadencii {
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

        public const String TAGNAME_AQUESTONE_RELEASE = "org.kbinani.cadencii.AquesToneRelease";
#if JAVA
        static {
            s_vsq_serializer = new XmlSerializer( VsqFileEx.class );
        }
#else
        static VsqFileEx() {
            s_vsq_serializer = new XmlSerializer( typeof( VsqFileEx ) );
        }
#endif

        public static String getEventTag( VsqEvent item, String name ) {
            if ( name == null || (name != null && name.Equals( "" ) ) ){
                return "";
            }
            if ( item.Tag != null ) {
                String[] spl = PortUtil.splitString( item.Tag, ';' );
                foreach ( String s in spl ) {
                    String[] spl2 = PortUtil.splitString( s, ':' );
                    if ( spl2.Length == 2 ) {
                        if ( name.Equals( spl2[0] ) ) {
                            return spl2[1];
                        }
                    }
                }
            }
            return "";
        }

        public static void setEventTag( VsqEvent item, String name, String value ) {
            if ( name == null ){
                return;
            }
            if ( name.Equals( "" ) ){
                return;
            }
            String v = value.Replace( ":", "" ).Replace( ";", "" );
            if ( item.Tag == null ) {
                item.Tag = name + ":" + value;
            } else {
                String newtag = "";
                String[] spl = PortUtil.splitString( item.Tag, ';' );
                boolean is_first = true;
                foreach ( String s in spl ) {
                    String[] spl2 = PortUtil.splitString( s, ':' );
                    if ( spl2.Length == 2 ) {
                        String add = "";
                        if ( name.Equals( spl2[0] ) ) {
                            add = name + ":" + v;
                        } else {
                            add = spl2[0] + ":" + spl2[1];
                        }
                        newtag += add + (is_first ? "" : ";");
                        is_first = false;
                    }
                }
                item.Tag = newtag;
            }
        }

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
                    double sec_end = this.getSecFromClock( clock + item.ID.getLength() ) - premeasure_sec_target + premeasure_sec_tempo;
                    int clock_start = (int)tempo.getClockFromSec( sec_start );
                    int clock_end = (int)tempo.getClockFromSec( sec_end );
                    item.Clock = clock_start;
                    item.ID.setLength( clock_end - clock_start );
                    if ( item.ID.VibratoHandle != null ) {
                        double sec_vib_start = this.getSecFromClock( clock + item.ID.VibratoDelay ) - premeasure_sec_target + premeasure_sec_tempo;
                        int clock_vib_start = (int)tempo.getClockFromSec( sec_vib_start );
                        item.ID.VibratoDelay = clock_vib_start - clock_start;
                        item.ID.VibratoHandle.setLength( clock_end - clock_vib_start );
                    }
                }

                // コントロールカーブをシフト
                for ( int j = 0; j < AppManager.CURVE_USAGE.Length; j++ ) {
                    CurveType ct = AppManager.CURVE_USAGE[j];
                    VsqBPList item = this.Track.get( track ).getCurve( ct.getName() );
                    if ( item == null ) {
                        continue;
                    }
                    VsqBPList repl = new VsqBPList( item.getName(), item.getDefault(), item.getMinimum(), item.getMaximum() );
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
            boolean first = true; // 負になった最初のアイテムかどうか

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
                        double t_end = vsq.getSecFromClock( item.Clock + item.ID.getLength() ) + sec;
                        int clock_end = (int)vsq.getClockFromSec( t_end );
                        int length = clock_end - clock;

                        if ( clock < pre_measure_clocks ) {
                            if ( pre_measure_clocks < clock_end ) {
                                // 音符の開始位置がプリメジャーよりも早く，音符の開始位置がプリメジャーより後の場合
                                clock = pre_measure_clocks;
                                length = clock_end - pre_measure_clocks;
                                // ビブラート
                                if ( item.ID.VibratoHandle != null ) {
                                    double vibrato_percent = item.ID.VibratoHandle.getLength() / (double)item.ID.getLength() * 100.0;
                                    double t_clock = vsq.getSecFromClock( clock ); // 音符の開始時刻
                                    double t_vibrato = t_end - (t_end - t_clock) * vibrato_percent / 100.0; // ビブラートの開始時刻
                                    int clock_vibrato_start = (int)vsq.getClockFromSec( t_vibrato );
                                    item.ID.VibratoHandle.setLength( clock_end - clock_vibrato_start );
                                    item.ID.VibratoDelay = clock_vibrato_start - clock;
                                }
                                item.Clock = clock;
                                item.ID.setLength( length );
                            } else {
                                // 範囲外なので削除
                                remove_required_event.add( k );
                            }
                        } else {
                            // ビブラート
                            if ( item.ID.VibratoHandle != null ) {
                                double t_vibrato_start = vsq.getSecFromClock( item.Clock + item.ID.getLength() - item.ID.VibratoHandle.getLength() ) + sec;
                                int clock_vibrato_start = (int)vsq.getClockFromSec( t_vibrato_start );
                                item.ID.VibratoHandle.setLength( clock_vibrato_start - clock );
                                item.ID.VibratoDelay = clock_vibrato_start - clock;
                            }
                            item.Clock = clock;
                            item.ID.setLength( length );
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
                    VsqBPList repl = new VsqBPList( item.getName(), item.getDefault(), item.getMinimum(), item.getMaximum() );
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
                                BezierChain new_chain = null;
                                try {
                                    new_chain = chain.extractPartialBezier( pre_measure_clocks, end );
                                    new_chain.id = chain.id;
                                    list.set( j, new_chain );
                                } catch ( Exception ex ) {
                                    PortUtil.stderr.println( "VsqFileEx#shift; ex=" + ex );
                                }
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
            ret.AttachedCurves = (AttachedCurve)AttachedCurves.clone();
            /*ret.m_pitch.Clear();
            for ( int i = 0; i < m_pitch.Count; i++ ) {
                ret.m_pitch.Add( (VsqBPList)m_pitch[i].clone() );
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
            command.args = new Object[1];
            command.args[0] = track;
            return command;
        }

        public static CadenciiCommand generateCommandTrackReplace( int track, VsqTrack item, BezierCurves attached_curve ) {
            //public static CadenciiCommand generateCommandTrackReplace( int track, VsqTrack item, BezierCurves attached_curve, VsqBPList pitch ) {
            CadenciiCommand command = new CadenciiCommand();
            command.type = CadenciiCommandType.TRACK_REPLACE;
            command.args = new Object[3];
            command.args[0] = track;
            command.args[1] = item.clone();
            command.args[2] = attached_curve.clone();
            //command.Args[3] = pitch.clone();
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
            command.args = new Object[4];
            command.args[0] = track.clone();
            command.args[1] = mixer;
            command.args[2] = position;
            command.args[3] = attached_curve.clone();
            //command.Args[4] = pitch.clone();
            return command;
        }

        public static CadenciiCommand generateCommandAddBezierChain( int track, CurveType curve_type, int chain_id, int clock_resolution, BezierChain chain ) {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.BEZIER_CHAIN_ADD;
            ret.args = new Object[5];
            ret.args[0] = track;
            ret.args[1] = curve_type;
            ret.args[2] = (BezierChain)chain.clone();
            ret.args[3] = clock_resolution;
            ret.args[4] = chain_id;
            return ret;
        }

        public static CadenciiCommand generateCommandDeleteBezierChain( int track, CurveType curve_type, int chain_id, int clock_resolution ) {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.BEZIER_CHAIN_DELETE;
            ret.args = new Object[4];
            ret.args[0] = track;
            ret.args[1] = curve_type;
            ret.args[2] = chain_id;
            ret.args[3] = clock_resolution;
            return ret;
        }

        public static CadenciiCommand generateCommandReplaceBezierChain( int track, CurveType curve_type, int chain_id, BezierChain chain, int clock_resolution ) {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.BEZIER_CHAIN_REPLACE;
            ret.args = new Object[5];
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
            ret.args = new Object[1];
            ret.args[0] = (VsqFileEx)vsq.clone();
            return ret;
        }

        public static CadenciiCommand generateCommandReplaceAttachedCurveRange( int track, TreeMap<CurveType, Vector<BezierChain>> attached_curves ) {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.ATTACHED_CURVE_REPLACE_RANGE;
            ret.args = new Object[2];
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
                    int track = (Integer)command.args[0];
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
                    int track = (Integer)command.args[0];
                    CurveType curve_type = (CurveType)command.args[1];
                    int chain_id = (Integer)command.args[2];
                    int clock_resolution = (Integer)command.args[3];
                    BezierChain chain = (BezierChain)AttachedCurves.get( track - 1 ).getBezierChain( curve_type, chain_id ).clone();
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
                    int clock_resolution = (Integer)command.args[4];
                    BezierChain target = (BezierChain)AttachedCurves.get( track - 1 ).getBezierChain( curve_type, chain_id ).clone();
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
                    VsqFileEx inv = (VsqFileEx)this.clone();
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
                    AttachedCurves = (AttachedCurve)vsq.AttachedCurves.clone();
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
                    int track = (Integer)command.args[0];
                    TreeMap<CurveType, Vector<BezierChain>> curves = (TreeMap<CurveType, Vector<BezierChain>>)command.args[1];
                    TreeMap<CurveType, Vector<BezierChain>> inv = new TreeMap<CurveType, Vector<BezierChain>>();
                    for ( Iterator itr = curves.keySet().iterator(); itr.hasNext(); ) {
                        CurveType ct = (CurveType)itr.next();
                        Vector<BezierChain> chains = new Vector<BezierChain>();
                        Vector<BezierChain> src = this.AttachedCurves.get( track - 1 ).get( ct );
                        for ( int i = 0; i < src.size(); i++ ) {
                            chains.add( (BezierChain)src.get( i ).clone() );
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
                    int position = (Integer)command.args[2];
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
                    int track = (Integer)command.args[0];
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
                    int track = (Integer)command.args[0];
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

        public VsqFileEx()
#if JAVA
        {
#else
            :
#endif
            this( "Miku", 1, 4, 4, 500000 )
#if JAVA
            ;
#else
        {
#endif
            Track.clear();
            TempoTable.clear();
            TimesigTable.clear();
        }

        public VsqFileEx( String singer, int pre_measure, int numerator, int denominator, int tempo )
#if JAVA
        {
#else
            :
#endif
            base( singer, pre_measure, numerator, denominator, tempo )
#if JAVA
            ;
#else
        {
#endif
            AttachedCurves = new AttachedCurve();
            int count = Track.size();
            for ( int i = 1; i < count; i++ ) {
                AttachedCurves.add( new BezierCurves() );
            }
        }

        public VsqFileEx( UstFile ust )
#if JAVA
        {
#else
            :
#endif
            base( ust )
#if JAVA
            ;
#else
        {
#endif
            AttachedCurves = new AttachedCurve();
            int count = Track.size();
            for ( int i = 1; i < count; i++ ) {
                AttachedCurves.add( new BezierCurves() );
            }
        }

        public VsqFileEx( String _fpath, String encoding )
#if JAVA
            throws FileNotFoundException
        {
#else
            :
#endif
            base( _fpath, encoding )
#if JAVA
            ;
#else
        {
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
                    PortUtil.stderr.println( "VsqFileEx#.ctor; ex=" + ex );
                } finally {
                    if ( fs != null ) {
                        try {
                            fs.close();
                        } catch ( Exception ex2 ) {
                            PortUtil.stderr.println( "VsqFileEx#.ctor; ex2=" + ex2 );
                        }
                    }
                }
                if ( tmp != null ) {
                    for ( Iterator itr = tmp.getCurves().iterator(); itr.hasNext(); ) {
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

        public void writeAsXml( String file ) {
            FileOutputStream xw = null;
            try {
                xw = new FileOutputStream( file );
                s_vsq_serializer.serialize( xw, this );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "VsqFileEx#writeAsXml; ex=" + ex );
            } finally {
                if ( xw != null ) {
                    try {
                        xw.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "VsqFileEx#writeAsXml; ex2=" + ex2 );
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
                PortUtil.stderr.println( "VsqFileEx#readFromXml; ex=" + ex );
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "VsqFileEx#readFromXml; ex2=" + ex2 );
                    }
                }
            }

            if ( ret == null ) {
                return null;
            }

            // ベジエ曲線のIDを播番
            if ( ret.AttachedCurves != null ) {
                for ( Iterator itr = ret.AttachedCurves.getCurves().iterator(); itr.hasNext(); ) {
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

            // VsqBPListのNameを更新
            int c = ret.Track.size();
            for ( int i = 0; i < c; i++ ) {
                VsqTrack track = ret.Track.get( i );
                foreach ( CurveType s in AppManager.CURVE_USAGE ) {
                    VsqBPList list = track.getCurve( s.getName() );
                    if ( list != null ) {
                        list.setName( s.getName().ToLower() );
                    }
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

#if !JAVA
}
#endif
