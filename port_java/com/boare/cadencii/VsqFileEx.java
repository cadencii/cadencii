/*
 * VsqFileEx.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed : the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.cadencii;

import java.util.*;
import java.io.*;

import com.boare.vsq.*;
import com.boare.corlib.*;
import com.boare.util.*;

public class VsqFileEx extends VsqFile implements Cloneable, CommandRunnable {
    static XmlSerializer s_vsq_serializer = new XmlSerializer( VsqFileEx.class );

    public AttachedCurve AttachedCurves;

    public static boolean isXmlIgnored( String name ){
        if( name.equals( "AttachedCurves" ) ){
            return false;
        }
        return true;
    }

    public Object clone() {
        VsqFileEx ret = new VsqFileEx( "Miku", 1, 4, 4, 500000 );
        ret.tracks = new Vector<VsqTrack>();
        for ( int i = 0; i < tracks.size(); i++ ) {
            ret.tracks.add( (VsqTrack)tracks.get( i ).clone() );
        }
        ret.tempoTable = new Vector<TempoTableEntry>();
        for ( int i = 0; i < tempoTable.size(); i++ ) {
            ret.tempoTable.add( (TempoTableEntry)tempoTable.get( i ).clone() );
        }
        ret.timeSigTable = new Vector<TimeSigTableEntry>();
        for ( int i = 0; i < timeSigTable.size(); i++ ) {
            ret.timeSigTable.add( (TimeSigTableEntry)timeSigTable.get( i ).clone() );
        }
        ret.m_tpq = m_tpq;
        ret.totalClocks = totalClocks;
        ret.m_base_tempo = m_base_tempo;
        ret.master = (VsqMaster)master.clone();
        ret.mixer = (VsqMixer)mixer.clone();
        ret.AttachedCurves = (AttachedCurve)AttachedCurves.clone();
        return ret;
    }

    /// <summary>
    /// トラックを削除するコマンドを発行します。VstRendererを取り扱う関係上、VsqCommandを使ってはならない。
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public static CadenciiCommand generateCommandDeleteTrack( int track ) {
        CadenciiCommand command = new CadenciiCommand();
        command.type = CadenciiCommandType.DeleteTrack;
        command.args = new Object[1];
        command.args[0] = track;
        return command;
    }

    public static CadenciiCommand generateCommandTrackReplace( int track, VsqTrack item, BezierCurves attached_curve ) {
        CadenciiCommand command = new CadenciiCommand();
        command.type = CadenciiCommandType.TrackReplace;
        command.args = new Object[3];
        command.args[0] = track;
        command.args[1] = item.clone();
        command.args[2] = attached_curve.clone();
        return command;
    }

    /// <summary>
    /// トラックを追加するコマンドを発行します．VstRendererを取り扱う関係上、VsqCommandを使ってはならない。
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public static CadenciiCommand generateCommandAddTrack( VsqTrack track, VsqMixerEntry mixer, int position, BezierCurves attached_curve ) {
        CadenciiCommand command = new CadenciiCommand();
        command.type = CadenciiCommandType.AddTrack;
        command.args = new Object[4];
        command.args[0] = track.clone();
        command.args[1] = mixer;
        command.args[2] = position;
        command.args[3] = attached_curve.clone();
        return command;
    }

    public static CadenciiCommand generateCommandAddBezierChain( int track, CurveType curve_type, int chain_id, int clock_resolution, BezierChain chain ) {
        CadenciiCommand ret = new CadenciiCommand();
        ret.type = CadenciiCommandType.AddBezierChain;
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
        ret.type = CadenciiCommandType.DeleteBezierChain;
        ret.args = new Object[4];
        ret.args[0] = track;
        ret.args[1] = curve_type;
        ret.args[2] = chain_id;
        ret.args[3] = clock_resolution;
        return ret;
    }

    public static CadenciiCommand generateCommandReplaceBezierChain( int track, CurveType curve_type, int chain_id, BezierChain chain, int clock_resolution ) {
        CadenciiCommand ret = new CadenciiCommand();
        ret.type = CadenciiCommandType.ReplaceBezierChain;
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
        ret.type = CadenciiCommandType.Replace;
        ret.args = new Object[1];
        ret.args[0] = (VsqFileEx)vsq.clone();
        return ret;
    }

    public static CadenciiCommand generateCommandReplaceAttachedCurveRange( int track, TreeMap<CurveType, Vector<BezierChain>> attached_curves ) {
        CadenciiCommand ret = new CadenciiCommand();
        ret.type = CadenciiCommandType.ReplaceAttachedCurveRange;
        ret.args = new Object[2];
        ret.args[0] = track;
        ret.args[1] = attached_curves;
        return ret;
    }

    public VsqCommand preprocessSpecialCommand( VsqCommand command ) {
        if ( command.type == VsqCommandType.TrackEditCurve ) {
            // TrackEditCurve
            int track = (Integer)command.args[0];
            String curve = (String)command.args[1];
            Vector<BPPair> com = (Vector<BPPair>)command.args[2];
            VsqBPList target = tracks.get( track ).getCurve( curve );

            VsqCommand inv = null;
            Vector<BPPair> edit = new Vector<BPPair>();
            if ( com != null ) {
                if ( com.size() > 0 ) {
                    int start_clock = com.get( 0 ).clock;
                    int end_clock = com.get( 0 ).clock;
                    for ( BPPair item : com ) {
                        start_clock = Math.min( start_clock, item.clock );
                        end_clock = Math.max( end_clock, item.clock );
                    }
                    tracks.get( track ).setEditedStart( start_clock );
                    tracks.get( track ).setEditedEnd( end_clock );
                    int start_value = target.getValue( start_clock );
                    int end_value = target.getValue( end_clock );
                    for ( Iterator i = target.keyClockIterator(); i.hasNext(); ) {
                        int clock = (Integer)i.next();
                        if ( start_clock <= clock && clock <= end_clock ) {
                            edit.add( new BPPair( clock, target.getValue( clock ) ) );
                        }
                    }
                    boolean start_found = false;
                    boolean end_found = false;
                    for ( int i = 0; i < edit.size(); i++ ) {
                        if ( edit.get( i ).clock == start_clock ) {
                            start_found = true;
                            edit.get( i ).value = start_value;
                            if ( start_found && end_found ) {
                                break;
                            }
                        }
                        if ( edit.get( i ).clock == end_clock ) {
                            end_found = true;
                            edit.get( i ).value = end_value;
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
                    inv = VsqCommand.generateCommandTrackEditCurve( track, curve, edit );
                } else if ( com.size() == 0 ) {
                    inv = VsqCommand.generateCommandTrackEditCurve( track, curve, new Vector<BPPair>() );
                }
            }

            updateTotalClocks();
            if ( com.size() == 0 ) {
                return inv;
            } else if ( com.size() == 1 ) {
                boolean found = false;
                for ( Iterator itr = target.keyClockIterator(); itr.hasNext(); ) {
                    int clock = (Integer)itr.next();
                    if ( clock == com.get( 0 ).clock ) {
                        found = true;
                        target.add( clock, com.get( 0 ).value );
                        break;
                    }
                }
                if ( !found ) {
                    target.add( com.get( 0 ).clock, com.get( 0 ).value );
                }
            } else {
                int start_clock = com.get( 0 ).clock;
                int end_clock = com.get( com.size() - 1 ).clock;
                boolean removed = true;
                while ( removed ) {
                    removed = false;
                    for ( Iterator itr = target.keyClockIterator(); itr.hasNext(); ) {
                        int clock = (Integer)itr.next();
                        if ( start_clock <= clock && clock <= end_clock ) {
                            target.remove( clock );
                            removed = true;
                            break;
                        }
                    }
                }
                for ( BPPair item : com ) {
                    target.add( item.clock, item.value );
                }
            }
            return inv;
        } else if ( command.type == VsqCommandType.TrackEditCurveRange ) {
            // TrackEditCurveRange
            int track = (Integer)command.args[0];
            String[] curves = (String[])command.args[1];
            Vector<BPPair>[] coms = (Vector<BPPair>[])command.args[2];
            Vector<Vector<BPPair>> inv_coms = new Vector<Vector<BPPair>>();
            for( int i = 0; i < curves.length; i++ ){
                inv_coms.add( new Vector<BPPair>() );
            }
            VsqCommand inv = null;

            for ( int k = 0; k < curves.length; k++ ) {
                String curve = curves[k];
                VsqBPList target = tracks.get( track ).getCurve( curve );
                Vector<BPPair> com = coms[k];
                Vector<BPPair> edit = new Vector<BPPair>();
                if ( com != null ) {
                    if ( com.size() > 0 ) {
                        int start_clock = com.get( 0 ).clock;
                        int end_clock = com.get( 0 ).clock;
                        for ( BPPair item : com ) {
                            start_clock = Math.min( start_clock, item.clock );
                            end_clock = Math.max( end_clock, item.clock );
                        }
                        tracks.get( track ).setEditedStart( start_clock );
                        tracks.get( track ).setEditedEnd( end_clock );
                        int start_value = target.getValue( start_clock );
                        int end_value = target.getValue( end_clock );
                        for ( Iterator itr = target.keyClockIterator(); itr.hasNext(); ) {
                            int clock = (Integer)itr.next();
                            if ( start_clock <= clock && clock <= end_clock ) {
                                edit.add( new BPPair( clock, target.getValue( clock ) ) );
                            }
                        }
                        boolean start_found = false;
                        boolean end_found = false;
                        for ( int i = 0; i < edit.size(); i++ ) {
                            if ( edit.get( i ).clock == start_clock ) {
                                start_found = true;
                                edit.get( i ).value = start_value;
                                if ( start_found && end_found ) {
                                    break;
                                }
                            }
                            if ( edit.get( i ).clock == end_clock ) {
                                end_found = true;
                                edit.get( i ).value = end_value;
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
                        inv_coms.set( k, edit );
                    } else if ( com.size() == 0 ) {
                        inv_coms.set( k, new Vector<BPPair>() );
                    }
                }

                updateTotalClocks();
                if ( com.size() == 0 ) {
                    return inv;
                } else if ( com.size() == 1 ) {
                    boolean found = false;
                    for ( Iterator itr = target.keyClockIterator(); itr.hasNext(); ) {
                        int clock = (Integer)itr.next();
                        if ( clock == com.get( 0 ).clock ) {
                            found = true;
                            target.add( clock, com.get( 0 ).value );
                            break;
                        }
                    }
                    if ( !found ) {
                        target.add( com.get( 0 ).clock, com.get( 0 ).value );
                    }
                } else {
                    int start_clock = com.get( 0 ).clock;
                    int end_clock = com.get( com.size() - 1 ).clock;
                    boolean removed = true;
                    while ( removed ) {
                        removed = false;
                        for ( Iterator itr = target.keyClockIterator(); itr.hasNext(); ) {
                            int clock = (Integer)itr.next();
                            if ( start_clock <= clock && clock <= end_clock ) {
                                target.remove( clock );
                                removed = true;
                                break;
                            }
                        }
                    }
                    for ( BPPair item : com ) {
                        target.add( item.clock, item.value );
                    }
                }
            }
            return VsqCommand.generateCommandTrackEditCurveRange( track, curves, inv_coms );
        }
        return null;
    }

    public Command executeCommand( Command com ) {
        CadenciiCommand command = (CadenciiCommand)com;
        CadenciiCommand ret = null;
        if ( command.type == CadenciiCommandType.VsqCommand ) {
            ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.VsqCommand;
            if ( command.vsqCommand.type == VsqCommandType.TrackEditCurve || command.vsqCommand.type == VsqCommandType.TrackEditCurveRange ) {
                ret.vsqCommand = preprocessSpecialCommand( command.vsqCommand );
            } else {
                ret.vsqCommand = super.executeCommand( command.vsqCommand );
            }
        } else {
            if ( command.type == CadenciiCommandType.AddBezierChain ) {
                // AddBezierChain
                int track = (Integer)command.args[0];
                CurveType curve_type = (CurveType)command.args[1];
                BezierChain chain = (BezierChain)command.args[2];
                int clock_resolution = (Integer)command.args[3];
                int added_id = (Integer)command.args[4];
                AttachedCurves.get( track - 1 ).addBezierChain( curve_type, chain, added_id );
                ret = generateCommandDeleteBezierChain( track, curve_type, added_id, clock_resolution );
                if ( chain.getCount() > 1 ) {
                    int min = (int)chain.points.get( 0 ).getBase().X;
                    int max = min;
                    for ( int i = 1; i < chain.points.size(); i++ ) {
                        min = Math.min( min, (int)chain.points.get( i ).getBase().X );
                        max = Math.max( max, (int)chain.points.get( i ).getBase().X );
                    }
                    int max_value = curve_type.getMaximum();
                    int min_value = curve_type.getMinimum();
                    if ( min < max ) {
                        Vector<BPPair> edit = new Vector<BPPair>();
                        int last_value = Integer.MAX_VALUE;
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
                        int value2 = tracks.get( track ).getCurve( curve_type.getName() ).getValue( max );
                        edit.add( new BPPair( max, value2 ) );
                        command.vsqCommand = VsqCommand.generateCommandTrackEditCurve( track, curve_type.getName(), edit );
                    }
                }
            } else if ( command.type == CadenciiCommandType.DeleteBezierChain ) {
                // DeleteBezierChain
                int track = (Integer)command.args[0];
                CurveType curve_type = (CurveType)command.args[1];
                int chain_id = (Integer)command.args[2];
                int clock_resolution = (Integer)command.args[3];
                BezierChain chain = (BezierChain)AttachedCurves.get( track - 1 ).getBezierChain( curve_type, chain_id ).clone();
                AttachedCurves.get( track - 1 ).remove( curve_type, chain_id );
                ret = generateCommandAddBezierChain( track, curve_type, chain_id, clock_resolution, chain );
                if ( command.vsqCommand != null && ret != null ) {
                    if ( command.vsqCommand.type == VsqCommandType.TrackEditCurve || command.vsqCommand.type == VsqCommandType.TrackEditCurveRange ) {
                        ret.vsqCommand = preprocessSpecialCommand( command.vsqCommand );
                    } else {
                        ret.vsqCommand = super.executeCommand( command.vsqCommand );
                    }
                }
            } else if ( command.type == CadenciiCommandType.ReplaceBezierChain ) {
                // ReplaceBezierChain
                int track = (Integer)command.args[0];
                CurveType curve_type = (CurveType)command.args[1];
                int chain_id = (Integer)command.args[2];
                BezierChain chain = (BezierChain)command.args[3];
                int clock_resolution = (Integer)command.args[4];
                BezierChain target = (BezierChain)AttachedCurves.get( track - 1 ).getBezierChain( curve_type, chain_id ).clone();
                AttachedCurves.get( track - 1 ).setBezierChain( curve_type, chain_id, chain );
                ret = generateCommandReplaceBezierChain( track, curve_type, chain_id, target, clock_resolution );
                if ( chain.getCount() == 1 ) {
                    int ex_min = (int)chain.points.get( 0 ).getBase().X;
                    int ex_max = ex_min;
                    if ( target.points.size() > 1 ) {
                        for ( int i = 1; i < target.points.size(); i++ ) {
                            ex_min = Math.min( ex_min, (int)target.points.get( i ).getBase().X );
                            ex_max = Math.max( ex_max, (int)target.points.get( i ).getBase().X );
                        }
                        if ( ex_min < ex_max ) {
                            int default_value = curve_type.getDefault();
                            Vector<BPPair> edit = new Vector<BPPair>();
                            edit.add( new BPPair( ex_min, default_value ) );
                            edit.add( new BPPair( ex_max, default_value ) );
                            command.vsqCommand = VsqCommand.generateCommandTrackEditCurve( track, curve_type.getName(), edit );
                        }
                    }
                } else if ( chain.getCount() > 1 ) {
                    int min = (int)chain.points.get( 0 ).getBase().X;
                    int max = min;
                    for ( int i = 1; i < chain.points.size(); i++ ) {
                        min = Math.min( min, (int)chain.points.get( i ).getBase().X );
                        max = Math.max( max, (int)chain.points.get( i ).getBase().X );
                    }
                    int ex_min = min;
                    int ex_max = max;
                    if ( target.points.size() > 0 ) {
                        ex_min = (int)target.points.get( 0 ).getBase().X;
                        ex_max = ex_min;
                        for ( int i = 1; i < target.points.size(); i++ ) {
                            ex_min = Math.min( ex_min, (int)target.points.get( i ).getBase().X );
                            ex_max = Math.max( ex_max, (int)target.points.get( i ).getBase().X );
                        }
                    }
                    int max_value = curve_type.getMaximum();
                    int min_value = curve_type.getMinimum();
                    int default_value = curve_type.getDefault();
                    Vector<BPPair> edit = new Vector<BPPair>();
                    if ( ex_min < min ) {
                        edit.add( new BPPair( ex_min, default_value ) );
                    }
                    if ( min < max ) {
                        int last_value = Integer.MAX_VALUE;
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
                            value2 = tracks.get( track ).getCurve( curve_type.getName() ).getValue( max );
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
                        command.vsqCommand = VsqCommand.generateCommandTrackEditCurve( track, curve_type.getName(), edit );
                    }
                }
            } else if ( command.type == CadenciiCommandType.Replace ) {
                // Replace
                VsqFileEx vsq = (VsqFileEx)command.args[0];
                VsqFileEx inv = (VsqFileEx)this.clone();
                tracks.clear();
                for ( int i = 0; i < vsq.tracks.size(); i++ ) {
                    tracks.add( (VsqTrack)vsq.tracks.get( i ).clone() );
                }
                tempoTable.clear();
                for ( int i = 0; i < vsq.tempoTable.size(); i++ ) {
                    tempoTable.add( (TempoTableEntry)vsq.tempoTable.get( i ).clone() );
                }
                timeSigTable.clear();
                for ( int i = 0; i < vsq.timeSigTable.size(); i++ ) {
                    timeSigTable.add( (TimeSigTableEntry)vsq.timeSigTable.get( i ).clone() );
                }
                m_tpq = vsq.m_tpq;
                totalClocks = vsq.totalClocks;
                m_base_tempo = vsq.m_base_tempo;
                master = (VsqMaster)vsq.master.clone();
                mixer = (VsqMixer)vsq.mixer.clone();
                AttachedCurves = (AttachedCurve)vsq.AttachedCurves.clone();
                updateTotalClocks();
                ret = generateCommandReplace( inv );
            } else if ( command.type == CadenciiCommandType.ReplaceAttachedCurveRange ) {
                // ReplaceAttachedCurveRange
                int track = (Integer)command.args[0];
                TreeMap<CurveType, Vector<BezierChain>> curves = (TreeMap<CurveType, Vector<BezierChain>>)command.args[1];
                TreeMap<CurveType, Vector<BezierChain>> inv = new TreeMap<CurveType, Vector<BezierChain>>();
                for ( CurveType ct : curves.keySet() ) {
                    Vector<BezierChain> chains = new Vector<BezierChain>();
                    Vector<BezierChain> src = this.AttachedCurves.get( track - 1 ).get( ct );
                    for ( int i = 0; i < src.size(); i++ ){
                        chains.add( (BezierChain)src.get( i ).clone() );
                    }
                    inv.put( ct, chains );

                    this.AttachedCurves.get( track - 1 ).get( ct ).clear();
                    for ( BezierChain bc : curves.get( ct ) ) {
                        this.AttachedCurves.get( track - 1 ).get( ct ).add( bc );
                    }
                }
                ret = generateCommandReplaceAttachedCurveRange( track, inv );
            } else if ( command.type == CadenciiCommandType.AddTrack ) {
                // AddTrack
                VsqTrack track = (VsqTrack)command.args[0];
                VsqMixerEntry aMixer = (VsqMixerEntry)command.args[1];
                int position = (Integer)command.args[2];
                BezierCurves attached_curve = (BezierCurves)command.args[3];
                ret = VsqFileEx.generateCommandDeleteTrack( position );
                if ( tracks.size() <= 17 ) {
                    tracks.insertElementAt( (VsqTrack)track.clone(), position );
                    AttachedCurves.insert( position - 1, attached_curve );
                    mixer.slave.insertElementAt( (VsqMixerEntry)aMixer.clone(), position - 1 );
                }
            } else if ( command.type == CadenciiCommandType.DeleteTrack ) {
                // DeleteTrack
                int track = (Integer)command.args[0];
                ret = VsqFileEx.generateCommandAddTrack( tracks.get( track ), mixer.slave.get( track - 1 ), track, AttachedCurves.get( track - 1 ) );
                tracks.removeElementAt( track );
                AttachedCurves.removeElementAt( track - 1 );
                mixer.slave.removeElementAt( track - 1 );
                updateTotalClocks();
            } else if ( command.type == CadenciiCommandType.TrackReplace ) {
                // TrackReplace
                int track = (Integer)command.args[0];
                VsqTrack item = (VsqTrack)command.args[1];
                BezierCurves bezier_curves = (BezierCurves)command.args[2];
                ret = VsqFileEx.generateCommandTrackReplace( track, tracks.get( track ), AttachedCurves.get( track - 1 ) );
                tracks.set( track, item );
                AttachedCurves.set( track - 1, bezier_curves );
                updateTotalClocks();
            }
            if ( command.vsqCommand != null && ret != null ) {
                if ( command.vsqCommand.type == VsqCommandType.TrackEditCurve || command.vsqCommand.type == VsqCommandType.TrackEditCurveRange ) {
                    ret.vsqCommand = preprocessSpecialCommand( command.vsqCommand );
                } else {
                    ret.vsqCommand = super.executeCommand( command.vsqCommand );
                }
            }
        }
        return ret;
    }

    public VsqFileEx(){
        this( "Miku", 1, 4, 4, 500000 );
        tracks.clear();
        tempoTable.clear();
        timeSigTable.clear();
    }

    public VsqFileEx( String singer, int pre_measure, int numerator, int denominator, int tempo ){
        super( singer, pre_measure, numerator, denominator, tempo );
        AttachedCurves = new AttachedCurve();
        for ( int i = 1; i < tracks.size(); i++ ) {
            AttachedCurves.add( new BezierCurves() );
        }
    }

    public VsqFileEx( String _fpath ){
        super( _fpath );
        AttachedCurves = new AttachedCurve();

        String xml = Path.combine( Path.getDirectoryName( _fpath ), Path.getFileName( _fpath ) + ".xml" );
        if ( (new File( xml )).exists() ) {
            AttachedCurve tmp = null;
            FileInputStream fs = null;
            try {
                fs = new FileInputStream( xml );
                XmlSerializer xs = new XmlSerializer( AttachedCurve.class );
                xs.deserialize( fs );
                //tmp = (AttachedCurve)AppManager.XmlSerializerListBezierCurves.deserialize( fs );
            } catch ( Exception ex ) {
                // 1.4.xのxmlとして読み込みを試みる
                if ( fs != null ) {
                    try{
                        fs.close();
                        fs = null;
                    }catch( Exception ex2 ){
                    }
                }
                Rescue14xXml rx = new Rescue14xXml();
                tmp = rx.rescue( xml, tracks.size() - 1 );
            } finally {
                if ( fs != null ) {
                    try{
                        fs.close();
                    }catch( Exception ex2 ){
                    }
                }
            }
            if ( tmp != null ) {
                for ( BezierCurves bc : tmp.curves ) {
                    for ( CurveType ct : CurveType.CURVE_USAGE ) {
                        Vector<BezierChain> list = bc.get( ct );
                        for ( int i = 0; i < list.size(); i++ ) {
                            list.get( i ).ID = i + 1;
                            for ( int j = 0; j < list.get( i ).points.size(); j++ ) {
                                list.get( i ).points.get( j ).setIDInternal( j + 1 );
                            }
                        }
                    }
                }
                AttachedCurves = tmp;
            }
        } else {
            for ( int i = 1; i < tracks.size(); i++ ) {
                AttachedCurves.add( new BezierCurves() );
            }
        }

        // UTAUでエクスポートしたIconHandleは、IDS=UTAUとなっているので探知する
        for ( int i = 1; i < tracks.size(); i++ ) {
            for ( Iterator itr = tracks.get( i ).getSingerEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = (VsqEvent)itr.next();
                if ( ve.id.iconHandle.IDS.toLowerCase().equals( "utau" ) ){
                    tracks.get( i ).getCommon().version = "UTU000";
                    break;
                }
            }
        }
    }

    public VsqFileEx( UstFile ust ){
        this( "Miku", 1, 4, 4, ust.getBaseTempo() );
        int clock_count = 480 * 4; //pre measure = 1、4分の4拍子としたので
        VsqBPList pitch = new VsqBPList( 0, -2400, 2400 );
        for ( Iterator itr = ust.getTrack( 0 ).getNoteEventIterator(); itr.hasNext(); ) {
            UstEvent ue = (UstEvent)itr.next();
            if ( !ue.lyric.equals( "R" ) ){
                VsqID id = new VsqID( 0 );
                id.length = ue.length;
                StringBuilder psymbol = new StringBuilder();
                psymbol.append( "a" );
                if ( !SymbolTable.attatch( ue.lyric, psymbol ) ) {
                    psymbol.setLength( 0 );
                    psymbol.append( "a" );
                }
                id.lyricHandle = new LyricHandle( ue.lyric, psymbol.toString() );
                id.note = ue.note;
                id.type = VsqIDType.Anote;
                VsqEvent ve = new VsqEvent( clock_count, id );
                ve.ustEvent = (UstEvent)ue.clone();
                tracks.get( 1 ).addEvent( ve );

                if ( ue.pitches != null ) {
                    // PBTypeクロックごとにデータポイントがある
                    int clock = clock_count - ue.pbType;
                    for ( int i = 0; i < ue.pitches.length; i++ ) {
                        clock += ue.pbType;
                        pitch.add( clock, (int)ue.pitches[i] );
                    }
                }
            }
            if ( ue.tempo > 0.0f ) {
                tempoTable.add( new TempoTableEntry( clock_count, (int)(60e6 / ue.tempo), 0.0 ) );
            }
            clock_count += ue.length;
        }
        updateTempoInfo();
        updateTotalClocks();
        updateTimesigInfo();
        reflectPitch( this, 1, pitch );
    }

    /// <summary>
    /// m_pitchからPITとPBSを再構成
    /// </summary>
    public void reflectPitch( VsqFile vsq, int track, VsqBPList pitch ) {
        Vector<Integer> keyclocks = new Vector<Integer>();
        int[] tkeys = pitch.getKeys();
        for( int i : tkeys ){
            keyclocks.add( i );
        }
        VsqBPList pit = new VsqBPList( 0, -8192, 8191 );
        VsqBPList pbs = new VsqBPList( 2, 0, 24 );
        int premeasure_clock = vsq.getPreMeasureClocks();
        int lastpit = pit.defaultValue;
        int lastpbs = pbs.defaultValue;
        int vpbs = 24;
        int vpit = 0;

        Vector<Integer> parts = new Vector<Integer>();   // 連続した音符ブロックの先頭音符のクロック位置。のリスト
        parts.add( premeasure_clock );
        int lastclock = premeasure_clock;
        for ( Iterator itr = vsq.tracks.get( track ).getNoteEventIterator(); itr.hasNext(); ) {
            VsqEvent ve = (VsqEvent)itr.next();
            if ( ve.clock <= lastclock ) {
                lastclock = Math.max( lastclock, ve.clock + ve.id.length );
            } else {
                parts.add( ve.clock );
                lastclock = ve.clock + ve.id.length;
            }
        }

        for ( int i = 0; i < parts.size(); i++ ) {
            int partstart = parts.get( i );
            int partend = Integer.MAX_VALUE;
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
                max = Math.max( max, Math.abs( pitch.getValue( keyclocks.get( j ) ) ) );
            }

            // 最大ピッチベンド幅を表現できる最小のPBSを計算
            vpbs = (int)(Math.ceil( max * 8192.0 / 8191.0 ) + 0.1);
            if ( vpbs <= 0 ) {
                vpbs = 1;
            }
            double pitch2 = pitch.getValue( partstart );
            if ( lastpbs != vpbs ) {
                pbs.add( partstart, vpbs );
                lastpbs = vpbs;
            }
            vpit = (int)(pitch2 * 8192 / (double)vpbs);
            if ( lastpit != vpit ){
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
                    pitch2 = pitch.getValue( clock );
                    vpit = (int)(pitch2 * 8192 / (double)vpbs);
                    if( lastpit != vpit ){
                        pit.add( clock, vpit );
                        lastpit = vpit;
                    }
                }
            }
        }
        vsq.tracks.get( track ).setCurve( "pit", pit );
        vsq.tracks.get( track ).setCurve( "pbs", pbs );
    }

    public void writeAsXml( String file ) {
        FileOutputStream fs = null;
        try{
            fs = new FileOutputStream( file );
            s_vsq_serializer.serialize( fs, this );
        }catch( Exception ex ){
        }finally{
            if( fs != null ){
                try{
                    fs.close();
                }catch( Exception ex2 ){
                }
            }
        }
    }

    public static VsqFileEx readFromXml( String file ) {
        VsqFileEx ret = null;
        FileInputStream fs = null;
        try{
            fs = new FileInputStream( file );
            ret = (VsqFileEx)s_vsq_serializer.deserialize( fs );
        }catch( Exception ex ){
        }finally{
            if( fs != null ){
                try{
                    fs.close();
                }catch( Exception ex2 ){
                }
            }
        }

        if( ret == null ){
            return null;
        }
        // ベジエ曲線のIDを播番
        if ( ret.AttachedCurves != null ) {
            for ( BezierCurves bc : ret.AttachedCurves.curves ) {
                for ( CurveType ct : CurveType.CURVE_USAGE ) {
                    Vector<BezierChain> list = bc.get( ct );
                    for ( int i = 0; i < list.size(); i++ ) {
                        list.get( i ).ID = i + 1;
                        for ( int j = 0; j < list.get( i ).points.size(); j++ ) {
                            list.get( i ).points.get( j ).setIDInternal( j + 1 );
                        }
                    }
                }
            }
        } else {
            for ( int i = 1; i < ret.tracks.size(); i++ ) {
                ret.AttachedCurves.add( new BezierCurves() );
            }
        }
        return ret;
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

        private Vector<BezierChain> util_clone( BezierChain[] item ){
            Vector<BezierChain> ret = new Vector<BezierChain>();
            for( BezierChain bc : item ){
                ret.add( (BezierChain)bc.clone() );
            }
            return ret;
        }

        public com.boare.cadencii.AttachedCurve rescue( String file, int num_track ) {
            XmlSerializer xs = null;
            try {
                xs = new XmlSerializer( Vector.class );
            } catch ( Exception ex ) {
                System.out.println( "VsqFileEx.Rescue14xXml.rescue; ex=" + ex );
            }
            if ( xs == null ) {
                return null;
            }
            FileInputStream fs = null;
            AttachedCurve ac = null;
            try {
                fs = new FileInputStream( file );
                Vector<Rescue14xXml.BezierCurves> list = (Vector<Rescue14xXml.BezierCurves>)xs.deserialize( fs );
                if ( list.size() >= num_track ) {
                    ac = new AttachedCurve();
                    ac.curves = new Vector<com.boare.cadencii.BezierCurves>();
                    for ( int i = 0; i < num_track; i++ ) {
                        com.boare.cadencii.BezierCurves add = new com.boare.cadencii.BezierCurves();
                        add.Brethiness = util_clone( list.get( i ).Brethiness );
                        add.Brightness = util_clone( list.get( i ).Brightness );
                        add.Clearness = util_clone( list.get( i ).Clearness );
                        add.Dynamics = util_clone( list.get( i ).Dynamics );
                        add.FX2Depth = new Vector<BezierChain>();
                        add.GenderFactor = util_clone( list.get( i ).GenderFactor );
                        add.Harmonics = new Vector<BezierChain>();
                        add.Opening = util_clone( list.get( i ).Opening );
                        add.PitchBend = util_clone( list.get( i ).PitchBend );
                        add.PitchBendSensitivity = util_clone( list.get( i ).PitchBendSensitivity );
                        add.PortamentoTiming = util_clone( list.get( i ).PortamentoTiming );
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
                        add.VibratoDepth = util_clone( list.get( i ).VibratoDepth );
                        add.VibratoRate = util_clone( list.get( i ).VibratoRate );
                        ac.curves.add( add );
                    }
                }
            } catch ( Exception ex ) {
                System.out.println( "Rescue14xXml; ex=" + ex );
                ac = null;
            } finally {
                if ( fs != null ) {
                    try{
                        fs.close();
                    }catch( Exception ex2 ){
                    }
                }
            }
            return ac;
        }
    }

}
