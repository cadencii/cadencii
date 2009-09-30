/*
 * VsqFile.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed : the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

import java.util.*;
import java.io.*;
import java.text.*;
import java.lang.reflect.*;
import com.boare.corlib.*;

/**
 * VSQファイルの内容を保持するクラス
 */
public class VsqFile implements Cloneable {
    /// <summary>
    /// トラックのリスト．最初のトラックはMasterTrackであり，通常の音符が格納されるトラックはインデックス1以降となる
    /// </summary>
    public Vector<VsqTrack> tracks;
    /// <summary>
    /// テンポ情報を保持したテーブル
    /// </summary>
    public Vector<TempoTableEntry> tempoTable;
    public Vector<TimeSigTableEntry> timeSigTable;
    protected int m_tpq;
    /// <summary>
    /// 曲の長さを取得します。(クロック(4分音符は480クロック))
    /// </summary>
    public int totalClocks = 0;
    protected int m_base_tempo;
    public VsqMaster master;  // VsqMaster, VsqMixerは通常，最初の非master tracksに記述されるが，可搬性のため，
    public VsqMixer mixer;    // ここではVsqFileに直属するものとして取り扱う．
    //protected int m_premeasure_clocks;
    public Object tag;

    static final byte[] _MTRK = new byte[] { 0x4d, 0x54, 0x72, 0x6b };
    static final byte[] _MTHD = new byte[] { 0x4d, 0x54, 0x68, 0x64 };
    static final byte[] _MASTER_TRACK = new byte[] { 0x4D, 0x61, 0x73, 0x74, 0x65, 0x72, 0x20, 0x54, 0x72, 0x61, 0x63, 0x6B, };
    static final String[] _CURVES = new String[] { "VEL", "DYN", "BRE", "BRI", "CLE", "OPE", "GEN", "POR", "PIT", "PBS" };

    public static String getGenericTypeName( String name ){
        if( name.equals( "tracks" ) ){
            return "com.boare.vsq.VsqTrack";
        }else if( name.equals( "tempoTable" ) ){
            return "com.boare.vsq.TempoTableEntry";
        }else if( name.equals( "timeSigTable" ) ){
            return "com.boare.vsq.TimeSigTableEntry";
        }
        return "";
    }

    public static boolean isXmlIgnored( String name ){
        if( name.equals( "tracks" ) ){
            return false;
        }else if( name.equals( "tempoTable" ) ){
            return false;
        }else if( name.equals( "timeSigTable" ) ){
            return false;
        }else if( name.equals( "totalClocks" ) ){
            return false;
        }else if( name.equals( "master" ) ){
            return false;
        }else if( name.equals( "mixer" ) ){
            return false;
        }
        return true;
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "tracks" ) ){
            return "Track";
        }else if( name.equals( "tempoTable" ) ){
            return "TempoTable";
        }else if( name.equals( "timeSigTable" ) ){
            return "TimesigTable";
        }else if( name.equals( "totalClocks" ) ){
            return "TotalClock";
        }else if( name.equals( "master" ) ){
            return "Master";
        }else if( name.equals( "mixer" ) ){
            return "Mixer";
        }
        return name;
    }

    /// <summary>
    /// テンポ値を一律order倍します。
    /// </summary>
    /// <param name="order"></param>
    public void speedingUp( double order ) {
        synchronized( tempoTable ){
            int c = tempoTable.size();
            for ( int i = 0; i < c; i++ ) {
                tempoTable.get( i ).tempo = (int)(tempoTable.get( i ).tempo / order);
            }
        }
        updateTempoInfo();
    }

    /// <summary>
    /// このインスタンスに編集を行うコマンドを実行します
    /// </summary>
    /// <param name="command">実行するコマンド</param>
    /// <returns>編集結果を元に戻すためのコマンドを返します</returns>
    public VsqCommand executeCommand( VsqCommand command ) {
        VsqCommandType type = command.type;
        if ( type == VsqCommandType.ChangePreMeasure ) {
            VsqCommand ret = VsqCommand.generateCommandChangePreMeasure( master.preMeasure );
            int value = (Integer)command.args[0];
            master.preMeasure = value;
            updateTimesigInfo();
            return ret;
        } else if ( type == VsqCommandType.AddTrack ) {
            VsqTrack track = (VsqTrack)command.args[0];
            VsqMixerEntry item = (VsqMixerEntry)command.args[1];
            int position = (Integer)command.args[2];
            VsqCommand ret = VsqCommand.generateCommandDeleteTrack( position );
            if ( tracks.size() <= 17 ) {
                tracks.insertElementAt( (VsqTrack)track.clone(), position );
                mixer.slave.add( (VsqMixerEntry)item.clone() );
                return ret;
            } else {
                return null;
            }
        } else if ( type == VsqCommandType.DeleteTrack ) {
            int track = (Integer)command.args[0];
            VsqCommand ret = VsqCommand.generateCommandAddTrack( tracks.get( track ), mixer.slave.get( track - 1 ), track );
            tracks.removeElementAt( track );
            mixer.slave.removeElementAt( track - 1 );
            updateTotalClocks();
            return ret;
        } else if ( type == VsqCommandType.UpdateTempo ) {
            int clock = (Integer)command.args[0];
            int tempo = (Integer)command.args[1];
            int new_clock = (Integer)command.args[2];

            int index = -1;
            int c = tempoTable.size();
            for ( int i = 0; i < c; i++ ) {
                if ( tempoTable.get( i ).clock == clock ) {
                    index = i;
                    break;
                }
            }
            VsqCommand ret = null;
            if ( index >= 0 ) {
                if ( tempo <= 0 ) {
                    ret = VsqCommand.generateCommandUpdateTempo( clock, clock, tempoTable.get( index ).tempo );
                    tempoTable.removeElementAt( index );
                } else {
                    ret = VsqCommand.generateCommandUpdateTempo( new_clock, clock, tempoTable.get( index ).tempo );
                    tempoTable.get( index ).tempo= tempo ;
                    tempoTable.get( index ).clock= new_clock ;
                }
            } else {
                ret = VsqCommand.generateCommandUpdateTempo( clock, clock, -1 );
                tempoTable.add( new TempoTableEntry( new_clock, tempo, 0.0 ) );
            }
            updateTempoInfo();
            updateTotalClocks();

            // 編集領域を更新
            int affected_clock = Math.min( clock, new_clock );
            c = tracks.size();
            for ( int i = 1; i < c; i++ ) {
                if ( affected_clock < tracks.get( i ).getEditedStart() ) {
                    tracks.get( i ).setEditedStart( affected_clock );
                }
                tracks.get( i ).setEditedEnd( (int)totalClocks );
            }
            return ret;
        } else if ( type == VsqCommandType.UpdateTempoRange ) {
            int[] clocks = (int[])command.args[0];
            int[] tempos = (int[])command.args[1];
            int[] new_clocks = (int[])command.args[2];
            int[] new_tempos = new int[tempos.length];
            int affected_clock = Integer.MAX_VALUE;
            for ( int i = 0; i < clocks.length; i++ ) {
                int index = -1;
                affected_clock = Math.min( affected_clock, clocks[i] );
                affected_clock = Math.min( affected_clock, new_clocks[i] );
                int c = tempoTable.size();
                for ( int j = 0; j < c; j++ ) {
                    if ( tempoTable.get( j ).clock == clocks[i] ) {
                        index = j;
                        break;
                    }
                }
                if ( index >= 0 ) {
                    new_tempos[i] = tempoTable.get( index ).tempo;
                    if ( tempos[i] <= 0 ) {
                        tempoTable.removeElementAt( index );
                    } else {
                        tempoTable.get( index ).tempo = tempos[i];
                        tempoTable.get( index ).clock = new_clocks[i];
                    }
                } else {
                    new_tempos[i] = -1;
                    tempoTable.add( new TempoTableEntry( new_clocks[i], tempos[i], 0.0 ) );
                }
            }
            updateTempoInfo();
            updateTotalClocks();
            int tracks_size = tracks.size();
            for ( int i = 1; i < tracks_size; i++ ) {
                VsqTrack item = tracks.get( i );
                if ( affected_clock < item.getEditedStart() ) {
                    item.setEditedStart( affected_clock );
                }
                item.setEditedEnd( (int)totalClocks );
            }
            return VsqCommand.generateCommandUpdateTempoRange( new_clocks, clocks, new_tempos );
        } else if ( type == VsqCommandType.UpdateTimesig ) {
            int barcount = (Integer)command.args[0];
            int numerator = (Integer)command.args[1];
            int denominator = (Integer)command.args[2];
            int new_barcount = (Integer)command.args[3];
            int index = -1;
            int c = timeSigTable.size();
            for ( int i = 0; i < c; i++ ) {
                if ( barcount == timeSigTable.get( i ).barCount ) {
                    index = i;
                    break;
                }
            }
            VsqCommand ret = null;
            if ( index >= 0 ) {
                if ( numerator <= 0 ) {
                    ret = VsqCommand.generateCommandUpdateTimesig( barcount, barcount, timeSigTable.get( index ).numerator, timeSigTable.get( index ).denominator );
                    timeSigTable.removeElementAt( index );
                } else {
                    ret = VsqCommand.generateCommandUpdateTimesig( new_barcount, barcount, timeSigTable.get( index ).numerator, timeSigTable.get( index ).denominator );
                    timeSigTable.get( index ).barCount = new_barcount;
                    timeSigTable.get( index ).numerator = numerator;
                    timeSigTable.get( index ).denominator = denominator;
                }
            } else {
                ret = VsqCommand.generateCommandUpdateTimesig( new_barcount, new_barcount, -1, -1 );
                timeSigTable.add( new TimeSigTableEntry( 0, numerator, denominator, new_barcount ) );
            }
            updateTimesigInfo();
            updateTotalClocks();
            return ret;
        } else if ( type == VsqCommandType.UpdateTimesigRange ) {
            int[] barcounts = (int[])command.args[0];
            int[] numerators = (int[])command.args[1];
            int[] denominators = (int[])command.args[2];
            int[] new_barcounts = (int[])command.args[3];
            int[] new_numerators = new int[numerators.length];
            int[] new_denominators = new int[denominators.length];
            for ( int i = 0; i < barcounts.length; i++ ) {
                int index = -1;
                // すでに拍子が登録されているかどうかを検査
                int c = timeSigTable.size();
                for ( int j = 0; j < c; j++ ) {
                    if ( timeSigTable.get( j ).barCount == barcounts[i] ) {
                        index = j;
                        break;
                    }
                }
                if ( index >= 0 ) {
                    // 登録されている場合
                    new_numerators[i] = timeSigTable.get( index ).numerator;
                    new_denominators[i] = timeSigTable.get( index ).denominator;
                    if ( numerators[i] <= 0 ) {
                        timeSigTable.removeElementAt( index );
                    } else {
                        timeSigTable.get( index ).barCount = new_barcounts[i];
                        timeSigTable.get( index ).numerator = numerators[i];
                        timeSigTable.get( index ).denominator = denominators[i];
                    }
                } else {
                    // 登録されていない場合
                    new_numerators[i] = -1;
                    new_denominators[i] = -1;
                    timeSigTable.add( new TimeSigTableEntry( 0, numerators[i], denominators[i], new_barcounts[i] ) );
                }
            }
            updateTimesigInfo();
            updateTotalClocks();
            return VsqCommand.generateCommandUpdateTimesigRange( new_barcounts, barcounts, new_numerators, new_denominators );
        } else if ( type == VsqCommandType.Replace ) {
            VsqFile vsq = (VsqFile)command.args[0];
            VsqFile inv = (VsqFile)this.clone();
            tracks.clear();
            int c = vsq.tracks.size();
            for ( int i = 0; i < c; i++ ) {
                tracks.add( (VsqTrack)vsq.tracks.get( i ).clone() );
            }
            tempoTable.clear();
            c = vsq.tempoTable.size();
            for ( int i = 0; i < c; i++ ) {
                tempoTable.add( (TempoTableEntry)vsq.tempoTable.get( i ).clone() );
            }
            timeSigTable.clear();
            c = vsq.timeSigTable.size();
            for ( int i = 0; i < c; i++ ) {
                timeSigTable.add( (TimeSigTableEntry)vsq.timeSigTable.get( i ).clone() );
            }
            m_tpq = vsq.m_tpq;
            totalClocks = vsq.totalClocks;
            m_base_tempo = vsq.m_base_tempo;
            master = (VsqMaster)vsq.master.clone();
            mixer = (VsqMixer)vsq.mixer.clone();
            updateTotalClocks();
            return VsqCommand.generateCommandReplace( inv );
        } else if ( type == VsqCommandType.EventAdd ) {
            int track = (Integer)command.args[0];
            VsqEvent item = (VsqEvent)command.args[1];
            tracks.get( track ).addEvent( item );
            VsqCommand ret = VsqCommand.generateCommandEventDelete( track, item.internalID );
            updateTotalClocks();
            if ( item.clock < tracks.get( track ).getEditedStart() ) {
                tracks.get( track ).setEditedStart( item.clock );
            }
            if ( tracks.get( track ).getEditedEnd() < item.clock + item.id.length ) {
                tracks.get( track ).setEditedEnd( item.clock + item.id.length );
            }
            return ret;
        } else if ( type == VsqCommandType.EventAddRange ) {
            int track = (Integer)command.args[0];
            VsqEvent[] items = (VsqEvent[])command.args[1];
            int[] inv_ids = new int[items.length];
            int min_clock = (int)totalClocks;
            int max_clock = 0;
            for ( int i = 0; i < items.length; i++ ) {
                VsqEvent item = (VsqEvent)items[i].clone();
                min_clock = Math.min( min_clock, item.clock );
                max_clock = Math.max( max_clock, item.clock + item.id.length );
                tracks.get( track ).addEvent( item );
                inv_ids[i] = item.internalID;
            }
            updateTotalClocks();
            if ( min_clock < tracks.get( track ).getEditedStart() ) {
                tracks.get( track ).setEditedStart( min_clock );
            }
            if ( tracks.get( track ).getEditedEnd() < max_clock ) {
                tracks.get( track ).setEditedEnd( max_clock );
            }
            return VsqCommand.generateCommandEventDeleteRange( track, inv_ids );
        } else if ( type == VsqCommandType.EventDelete ) {
            int internal_id = (Integer)command.args[0];
            int track = (Integer)command.args[1];
            VsqEvent[] original = new VsqEvent[1];
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.internalID == internal_id ) {
                    original[0] = (VsqEvent)item.clone();
                    break;
                }
            }
            if ( original[0].clock < tracks.get( track ).getEditedStart() ) {
                tracks.get( track ).setEditedStart( original[0].clock );
            }
            if ( tracks.get( track ).getEditedEnd() < original[0].clock + original[0].id.length ) {
                tracks.get( track ).setEditedEnd( original[0].clock + original[0].id.length );
            }
            VsqCommand ret = VsqCommand.generateCommandEventAddRange( track, original );
            VsqTrack work = tracks.get( track );
            int c = work.getEventCount();
            for ( int i = 0; i < c; i++ ) {
                if ( work.getEvent( i ).internalID == internal_id ) {
                    work.removeEvent( i );
                    break;
                }
            }
            updateTotalClocks();
            return ret;
        } else if ( type == VsqCommandType.EventDeleteRange ) {
            int[] internal_ids = (int[])command.args[0];
            int track = (Integer)command.args[1];
            Vector<VsqEvent> inv = new Vector<VsqEvent>();
            int min_clock = Integer.MAX_VALUE;
            int max_clock = Integer.MIN_VALUE;
            for ( int j = 0; j < internal_ids.length; j++ ) {
                VsqTrack work = tracks.get( track );
                int c = work.getEventCount();
                for ( int i = 0; i < c; i++ ) {
                    VsqEvent item = work.getEvent( i );
                    if ( internal_ids[j] == item.internalID ) {
                        inv.add( (VsqEvent)item.clone() );
                        min_clock = Math.min( min_clock, item.clock );
                        max_clock = Math.max( max_clock, item.clock + item.id.length );
                        work.removeEvent( i );
                        break;
                    }
                }
            }
            updateTotalClocks();
            tracks.get( track ).setEditedStart( min_clock );
            tracks.get( track ).setEditedEnd( max_clock );
            return VsqCommand.generateCommandEventAddRange( track, inv.toArray( new VsqEvent[]{} ) );
        } else if ( type == VsqCommandType.EventChangeClock ) {
            int track = (Integer)command.args[0];
            int internal_id = (Integer)command.args[1];
            int value = (Integer)command.args[2];
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.internalID == internal_id ) {
                    VsqCommand ret = VsqCommand.generateCommandEventChangeClock( track, internal_id, item.clock );
                    int min = Math.min( item.clock, value );
                    int max = Math.max( item.clock + item.id.length, value + item.id.length );
                    tracks.get( track ).setEditedStart( min );
                    tracks.get( track ).setEditedEnd( max );
                    item.clock = value;
                    updateTotalClocks();
                    return ret;
                }
            }
            return null;
        } else if ( type == VsqCommandType.EventChangeLyric ) {
            int track = (Integer)command.args[0];
            int internal_id = (Integer)command.args[1];
            String phrase = (String)command.args[2];
            String phonetic_symbol = (String)command.args[3];
            boolean protect_symbol = (Boolean)command.args[4];
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.internalID == internal_id ) {
                    if ( item.id.type == VsqIDType.Anote ) {
                        VsqCommand ret = VsqCommand.generateCommandEventChangeLyric( track, 
                                                                                     internal_id,
                                                                                     item.id.lyricHandle.L0.phrase, 
                                                                                     item.id.lyricHandle.L0.getPhoneticSymbol(),
                                                                                     item.id.lyricHandle.L0.phoneticSymbolProtected );
                        item.id.lyricHandle.L0.phrase = phrase;
                        item.id.lyricHandle.L0.setPhoneticSymbol( phonetic_symbol );
                        item.id.lyricHandle.L0.phoneticSymbolProtected = protect_symbol;
                        tracks.get( track ).setEditedStart( item.clock );
                        tracks.get( track ).setEditedEnd( item.clock + item.id.length );
                        updateTotalClocks();
                        return ret;
                    }
                }
            }
            return null;
        } else if ( type == VsqCommandType.EventChangeNote ) {
            int track = (Integer)command.args[0];
            int internal_id = (Integer)command.args[1];
            int note = (Integer)command.args[2];
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.internalID == internal_id ) {
                    VsqCommand ret = VsqCommand.generateCommandEventChangeNote( track, internal_id, item.id.note );
                    item.id.note = note;
                    updateTotalClocks();
                    tracks.get( track ).setEditedStart( item.clock );
                    tracks.get( track ).setEditedEnd( item.clock + item.id.length );
                    return ret;
                }
            }
            return null;
        } else if ( type == VsqCommandType.EventChangeClockAndNote ) {
            int track = (Integer)command.args[0];
            int internal_id = (Integer)command.args[1];
            int clock = (Integer)command.args[2];
            int note = (Integer)command.args[3];
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.internalID == internal_id ) {
                    VsqCommand ret = VsqCommand.generateCommandEventChangeClockAndNote( track, internal_id, item.clock, item.id.note );
                    int min = Math.min( item.clock, clock );
                    int max = Math.max( item.clock + item.id.length, clock + item.id.length );
                    tracks.get( track ).setEditedStart( min );
                    tracks.get( track ).setEditedEnd( max );
                    item.clock = clock;
                    item.id.note = note;
                    updateTotalClocks();
                    return ret;
                }
            }
            return null;
        } else if ( type == VsqCommandType.TrackEditCurve ) {
            int track = (Integer)command.args[0];
            String curve = (String)command.args[1];
            Vector<BPPair> com = (Vector<BPPair>)command.args[2];
            VsqCommand inv = null;
            Vector<BPPair> edit = new Vector<BPPair>();
            if ( com != null ) {
                if ( com.size() > 0 ) {
                    int start_clock = com.get( 0 ).clock;
                    int end_clock = com.get( 0 ).clock;
                    for( BPPair item : com ){
                        start_clock = Math.min( start_clock, item.clock );
                        end_clock = Math.max( end_clock, item.clock );
                    }
                    tracks.get( track ).setEditedStart( start_clock );
                    tracks.get( track ).setEditedEnd( end_clock );
                    int start_value = tracks.get( track ).getCurve( curve ).getValue( start_clock );
                    int end_value = tracks.get( track ).getCurve( curve ).getValue( end_clock );
                    for ( Iterator i = tracks.get( track ).getCurve( curve ).keyClockIterator(); i.hasNext(); ){
                        int clock = (Integer)i.next();
                        if ( start_clock <= clock && clock <= end_clock ) {
                            edit.add( new BPPair( clock, tracks.get( track ).getCurve( curve ).getValue( clock ) ) );
                        }
                    }
                    boolean start_found = false;
                    boolean end_found = false;
                    int c = edit.size();
                    for ( int i = 0; i < c; i++ ) {
                        BPPair edit_element = edit.get( i );
                        if ( edit_element.clock == start_clock ) {
                            start_found = true;
                            edit_element.value = start_value;
                            if ( start_found && end_found ) {
                                break;
                            }
                        }
                        if ( edit_element.clock == end_clock ) {
                            end_found = true;
                            edit_element.value = end_value;
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
                for ( Iterator itr = tracks.get( track ).getCurve( curve ).keyClockIterator(); itr.hasNext(); ){
                    int clock = (Integer)itr.next();
                    if ( clock == com.get( 0 ).clock ) {
                        found = true;
                        tracks.get( track ).getCurve( curve ).add( clock, com.get( 0 ).value );
                        break;
                    }
                }
                if ( !found ) {
                    tracks.get( track ).getCurve( curve ).add( com.get( 0 ).clock, com.get( 0 ).value );
                }
            } else {
                int start_clock = com.get( 0 ).clock;
                int end_clock = com.get( com.size() - 1 ).clock;
                boolean removed = true;
                while ( removed ) {
                    removed = false;
                    for ( Iterator itr = tracks.get( track ).getCurve( curve ).keyClockIterator(); itr.hasNext(); ) {
                        int clock = (Integer)itr.next();
                        if ( start_clock <= clock && clock <= end_clock ) {
                            tracks.get( track ).getCurve( curve ).remove( clock );
                            removed = true;
                            break;
                        }
                    }
                }
                for( BPPair item : com ){
                    tracks.get( track ).getCurve( curve ).add( item.clock, item.value );
                }
            }
            return inv;
        }else if( type == VsqCommandType.TrackEditCurveRange ){
            int track = (Integer)command.args[0];
            String[] curves = (String[])command.args[1];
            Vector<Vector<BPPair>> coms = (Vector<Vector<BPPair>>)command.args[2];
            Vector<Vector<BPPair>> inv_coms = new Vector<Vector<BPPair>>();
            VsqCommand inv = null;

            for ( int k = 0; k < curves.length; k++ ) {
                String curve = curves[k];
                Vector<BPPair> com = coms.get( k );
                Vector<BPPair> edit = new Vector<BPPair>();
                if ( com != null ) {
                    if ( com.size() > 0 ) {
                        int start_clock = com.get( 0 ).clock;
                        int end_clock = com.get( 0 ).clock;
                        for( BPPair item : com ){
                            start_clock = Math.min( start_clock, item.clock );
                            end_clock = Math.max( end_clock, item.clock );
                        }
                        tracks.get( track ).setEditedStart( start_clock );
                        tracks.get( track ).setEditedEnd( end_clock );
                        int start_value = tracks.get( track ).getCurve( curve ).getValue( start_clock );
                        int end_value = tracks.get( track ).getCurve( curve ).getValue( end_clock );
                        for ( Iterator itr = tracks.get( track ).getCurve( curve ).keyClockIterator(); itr.hasNext(); ) {
                            int clock = (Integer)itr.next();
                            if ( start_clock <= clock && clock <= end_clock ) {
                                edit.add( new BPPair( clock, tracks.get( track ).getCurve( curve ).getValue( clock ) ) );
                            }
                        }
                        boolean start_found = false;
                        boolean end_found = false;
                        int c = edit.size();
                        for ( int i = 0; i < c; i++ ) {
                            BPPair edit_element = edit.get( i );
                            if ( edit_element.clock == start_clock ) {
                                start_found = true;
                                edit_element.value = start_value;
                                if ( start_found && end_found ) {
                                    break;
                                }
                            }
                            if ( edit_element.clock == end_clock ) {
                                end_found = true;
                                edit_element.value = end_value;
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
                        inv_coms.add( edit );
                    } else if ( com.size() == 0 ) {
                        inv_coms.add( new Vector<BPPair>() );
                    }
                }

                updateTotalClocks();
                if ( com.size() == 0 ) {
                    return inv;
                } else if ( com.size() == 1 ) {
                    boolean found = false;
                    for ( Iterator itr = tracks.get( track ).getCurve( curve ).keyClockIterator(); itr.hasNext(); ) {
                        int clock = (Integer)itr.next();
                        if ( clock == com.get( 0 ).clock ) {
                            found = true;
                            tracks.get( track ).getCurve( curve ).add( clock, com.get( 0 ).value );
                            break;
                        }
                    }
                    if ( !found ) {
                        tracks.get( track ).getCurve( curve ).add( com.get( 0 ).clock, com.get( 0 ).value );
                    }
                } else {
                    int start_clock = com.get( 0 ).clock;
                    int end_clock = com.get( com.size() - 1 ).clock;
                    boolean removed = true;
                    while ( removed ) {
                        removed = false;
                        for ( Iterator itr = tracks.get( track ).getCurve( curve ).keyClockIterator(); itr.hasNext(); ) {
                            int clock = (Integer)itr.next();
                            if ( start_clock <= clock && clock <= end_clock ) {
                                tracks.get( track ).getCurve( curve ).remove( clock );
                                removed = true;
                                break;
                            }
                        }
                    }
                    for( BPPair item : com ){
                        tracks.get( track ).getCurve( curve ).add( item.clock, item.value );
                    }
                }
            }
            return VsqCommand.generateCommandTrackEditCurveRange( track, curves, inv_coms );
        } else if ( type == VsqCommandType.EventChangeVelocity ) {
            int track = (Integer)command.args[0];
            Vector<KeyValuePair<Integer, Integer>> veloc = (Vector<KeyValuePair<Integer, Integer>>)command.args[1];
            Vector<KeyValuePair<Integer, Integer>> inv = new Vector<KeyValuePair<Integer, Integer>>();
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent ev = (VsqEvent)itr.next();
                for( KeyValuePair<Integer, Integer> add : veloc ){
                    if ( ev.internalID == add.key ) {
                        inv.add( new KeyValuePair<Integer, Integer>( ev.internalID, ev.id.dynamics ) );
                        ev.id.dynamics = add.value;
                        tracks.get( track ).setEditedStart( ev.clock );
                        tracks.get( track ).setEditedEnd( ev.clock + ev.id.length );
                        break;
                    }
                }
            }
            return VsqCommand.generateCommandEventChangeVelocity( track, inv );
        } else if ( type == VsqCommandType.EventChangeAccent ) {
            int track = (Integer)command.args[0];
            Vector<KeyValuePair<Integer, Integer>> veloc = (Vector<KeyValuePair<Integer, Integer>>)command.args[1];
            Vector<KeyValuePair<Integer, Integer>> inv = new Vector<KeyValuePair<Integer, Integer>>();
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent ev = (VsqEvent)itr.next();
                for ( KeyValuePair<Integer, Integer> add : veloc ) {
                    if ( ev.internalID == add.key ) {
                        inv.add( new KeyValuePair<Integer, Integer>( ev.internalID, ev.id.demAccent ) );
                        ev.id.demAccent = add.value;
                        tracks.get( track ).setEditedStart( ev.clock );
                        tracks.get( track ).setEditedEnd( ev.clock + ev.id.length );
                        break;
                    }
                }
            }
            return VsqCommand.generateCommandEventChangeAccent( track, inv );
        } else if ( type == VsqCommandType.EventChangeDecay ) {
            int track = (Integer)command.args[0];
            Vector<KeyValuePair<Integer, Integer>> veloc = (Vector<KeyValuePair<Integer, Integer>>)command.args[1];
            Vector<KeyValuePair<Integer, Integer>> inv = new Vector<KeyValuePair<Integer, Integer>>();
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent ev = (VsqEvent)itr.next();
                for ( KeyValuePair<Integer, Integer> add : veloc ) {
                    if ( ev.internalID == add.key ) {
                        inv.add( new KeyValuePair<Integer, Integer>( ev.internalID, ev.id.demDecGainRate ) );
                        ev.id.demDecGainRate = add.value;
                        tracks.get( track ).setEditedStart( ev.clock );
                        tracks.get( track ).setEditedEnd( ev.clock + ev.id.length );
                        break;
                    }
                }
            }
            return VsqCommand.generateCommandEventChangeDecay( track, inv );
        } else if ( type == VsqCommandType.EventChangeLength ) {
            int track = (Integer)command.args[0];
            int internal_id = (Integer)command.args[1];
            int new_length = (Integer)command.args[2];
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.internalID == internal_id ) {
                    VsqCommand ret = VsqCommand.generateCommandEventChangeLength( track, internal_id, item.id.length );
                    tracks.get( track ).setEditedStart( item.clock );
                    int max = Math.max( item.clock + item.id.length, item.clock + new_length );
                    tracks.get( track ).setEditedEnd( max );
                    item.id.length = new_length;
                    updateTotalClocks();
                    return ret;
                }
            }
            return null;
        } else if ( type == VsqCommandType.EventChangeClockAndLength ) {
            int track = (Integer)command.args[0];
            int internal_id = (Integer)command.args[1];
            int new_clock = (Integer)command.args[2];
            int new_length = (Integer)command.args[3];
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.internalID == internal_id ) {
                    VsqCommand ret = VsqCommand.generateCommandEventChangeClockAndLength( track, internal_id, item.clock, item.id.length );
                    int min = Math.min( item.clock, new_clock );
                    int max_length = Math.max( item.id.length, new_length );
                    int max = Math.max( item.clock + max_length, new_clock + max_length );
                    tracks.get( track ).setEditedStart( min );
                    tracks.get( track ).setEditedEnd( max );
                    item.id.length = new_length;
                    item.clock = new_clock;
                    updateTotalClocks();
                    return ret;
                }
            }
            return null;
        } else if ( type == VsqCommandType.EventChangeIDContaints ) {
            int track = (Integer)command.args[0];
            int internal_id = (Integer)command.args[1];
            VsqID value = (VsqID)command.args[2];
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.internalID == internal_id ) {
                    VsqCommand ret = VsqCommand.generateCommandEventChangeIDContaints( track, internal_id, item.id );
                    int max_length = Math.max( item.id.length, value.length );
                    tracks.get( track ).setEditedStart( item.clock );
                    tracks.get( track ).setEditedEnd( item.clock + max_length );
                    item.id = (VsqID)value.clone();
                    if ( item.id.type == VsqIDType.Singer ) {
                        // 歌手変更の場合、次に現れる歌手変更の位置まで編集の影響が及ぶ
                        boolean found = false;
                        for ( Iterator itr2 = tracks.get( track ).getSingerEventIterator(); itr2.hasNext(); ) {
                            VsqEvent item2 = (VsqEvent)itr2.next();
                            if ( item.clock < item2.clock ) {
                                tracks.get( track ).setEditedEnd( item2.clock );
                                found = true;
                                break;
                            }
                        }
                        if ( !found ) {
                            // 変更対象が、該当トラック最後の歌手変更イベントだった場合
                            if ( tracks.get( track ).getEventCount() >= 1 ) {
                                VsqEvent last_event = tracks.get( track ).getEvent( tracks.get( track ).getEventCount() - 1 );
                                tracks.get( track ).setEditedEnd( last_event.clock + last_event.id.length );
                            }
                        }
                    }
                    updateTotalClocks();
                    return ret;
                }
            }
            return null;
        } else if ( type == VsqCommandType.EventChangeIDContaintsRange ) {
            int track = (Integer)command.args[0];
            int[] internal_ids = (int[])command.args[1];
            VsqID[] values = (VsqID[])command.args[2];
            VsqID[] inv_values = new VsqID[values.length];
            for ( int i = 0; i < internal_ids.length; i++ ) {
                for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr.next();
                    if ( item.internalID == internal_ids[i] ) {
                        inv_values[i] = (VsqID)item.id.clone();
                        int max_length = Math.max( item.id.length, values[i].length );
                        tracks.get( track ).setEditedStart( item.clock );
                        tracks.get( track ).setEditedEnd( item.clock + max_length );
                        item.id = (VsqID)values[i].clone();
                        if ( item.id.type == VsqIDType.Singer ) {
                            // 歌手変更の場合、次に現れる歌手変更の位置まで編集の影響が及ぶ
                            boolean found = false;
                            for ( Iterator itr2 = tracks.get( track ).getSingerEventIterator(); itr2.hasNext(); ) {
                                VsqEvent item2 = (VsqEvent)itr2.next();
                                if ( item.clock < item2.clock ) {
                                    tracks.get( track ).setEditedEnd( item2.clock );
                                    found = true;
                                    break;
                                }
                            }
                            if ( !found ) {
                                // 変更対象が、該当トラック最後の歌手変更イベントだった場合
                                if ( tracks.get( track ).getEventCount() >= 1 ) {
                                    VsqEvent last_event = tracks.get( track ).getEvent( tracks.get( track ).getEventCount() - 1 );
                                    tracks.get( track ).setEditedEnd( last_event.clock + last_event.id.length );
                                }
                            }
                        }
                        break;
                    }
                }
            }
            updateTotalClocks();
            return VsqCommand.generateCommandEventChangeIDContaintsRange( track, internal_ids, inv_values );
        } else if ( type == VsqCommandType.EventChangeClockAndIDContaints ) {
            int track = (Integer)command.args[0];
            int internal_id = (Integer)command.args[1];
            int new_clock = (Integer)command.args[2];
            VsqID value = (VsqID)command.args[3];
            for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.internalID == internal_id ) {
                    VsqCommand ret = VsqCommand.generateCommandEventChangeClockAndIDContaints( track, internal_id, item.clock, item.id );
                    int max_length = Math.max( item.id.length, value.length );
                    int min = Math.min( item.clock, new_clock );
                    int max = Math.max( item.clock + max_length, new_clock + max_length );
                    item.id = (VsqID)value.clone();
                    item.clock = new_clock;
                    tracks.get( track ).setEditedStart( min );
                    tracks.get( track ).setEditedEnd( max );
                    updateTotalClocks();
                    return ret;
                }
            }
            return null;
        } else if ( type == VsqCommandType.EventChangeClockAndIDContaintsRange ) {
            int track = (Integer)command.args[0];
            int[] internal_ids = (int[])command.args[1];
            int[] clocks = (int[])command.args[2];
            VsqID[] values = (VsqID[])command.args[3];
            VsqID[] inv_id = new VsqID[values.length];
            int[] inv_clock = new int[values.length];
            for ( int i = 0; i < internal_ids.length; i++ ) {
                for ( Iterator itr = tracks.get( track ).getEventIterator(); itr.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr.next();
                    if ( item.internalID == internal_ids[i] ) {
                        inv_id[i] = (VsqID)item.id.clone();
                        inv_clock[i] = item.clock;
                        int max_length = Math.max( item.id.length, values[i].length );
                        int min = Math.min( item.clock, clocks[i] );
                        int max = Math.max( item.clock + max_length, clocks[i] + max_length );
                        tracks.get( track ).setEditedStart( min );
                        tracks.get( track ).setEditedEnd( max );
                        item.id = (VsqID)values[i].clone();
                        item.clock = clocks[i];
                        break;
                    }
                }
            }
            updateTotalClocks();
            return VsqCommand.generateCommandEventChangeClockAndIDContaintsRange(
                track,
                internal_ids,
                inv_clock,
                inv_id );
        } else if ( type == VsqCommandType.TrackChangeName ) {
            int track = (Integer)command.args[0];
            String new_name = (String)command.args[1];
            VsqCommand ret = VsqCommand.generateCommandTrackChangeName( track, tracks.get( track ).name );
            tracks.get( track ).name = new_name;
            return ret;
        } else if ( type == VsqCommandType.TrackReplace ) {
            int track = (Integer)command.args[0];
            VsqTrack item = (VsqTrack)command.args[1];
            VsqCommand ret = VsqCommand.generateCommandTrackReplace( track, tracks.get( track ) );
            tracks.set( track, item );
            updateTotalClocks();
            return ret;
        } else if ( type == VsqCommandType.TrackChangePlayMode ) {
            int track = (Integer)command.args[0];
            int play_mode = (Integer)command.args[1];
            VsqCommand ret = VsqCommand.generateCommandTrackChangePlayMode( track, tracks.get( track ).getCommon().playMode );
            tracks.get( track ).getCommon().playMode = play_mode;
            return ret;
        } else if ( type == VsqCommandType.EventReplace ) {
            int track = (Integer)command.args[0];
            VsqEvent item = (VsqEvent)command.args[1];
            VsqCommand ret = null;
            int c = tracks.get( track ).getEventCount();
            for ( int i = 0; i < c; i++ ) {
                VsqEvent ve = tracks.get( track ).getEvent( i );
                if ( ve.internalID == item.internalID ) {
                    ret = VsqCommand.generateCommandEventReplace( track, ve );
                    tracks.get( track ).setEvent( i, item );
                    break;
                }
            }
            return ret;
        } else if ( type == VsqCommandType.EventReplaceRange ) {
            int track = (Integer)command.args[0];
            Object[] items = (Object[])command.args[1];
            VsqCommand ret = null;
            VsqEvent[] reverse = new VsqEvent[items.length];
            for ( int i = 0; i < items.length; i++ ) {
                VsqEvent ve = (VsqEvent)items[i];
                int c = tracks.get( track ).getEventCount();
                for ( int j = 0; j < c; j++ ) {
                    VsqEvent ve2 = (VsqEvent)tracks.get( track ).getEvent( j );
                    if ( ve2.internalID == ve.internalID ) {
                        reverse[i] = (VsqEvent)ve2.clone();
                        tracks.get( track ).setEvent( j, (VsqEvent)items[i] );
                        break;
                    }
                }
            }
            ret = VsqCommand.generateCommandEventReplaceRange( track, reverse );
            return ret;
        }
        return null;
    }

    /// <summary>
    /// VSQファイルの指定されたクロック範囲のイベント等を削除します
    /// </summary>
    /// <param name="vsq">編集対象のVsqFileインスタンス</param>
    /// <param name="clock_start">削除を行う範囲の開始クロック</param>
    /// <param name="clock_end">削除を行う範囲の終了クロック</param>
    public void removePart( int clock_start, int clock_end ) {
        int dclock = clock_end - clock_start;

        // テンポ情報の削除、シフト
        boolean changed = true;
        Vector<TempoTableEntry> buf = new Vector<TempoTableEntry>( tempoTable );
        int tempo_at_clock_end = getTempoAt( clock_end );
        tempoTable.clear();
        boolean just_on_clock_end_added = false;
        int c = buf.size();
        for ( int i = 0; i < c; i++ ) {
            TempoTableEntry buf_element = buf.get( i );
            if ( buf_element.clock < clock_start ) {
                tempoTable.add( (TempoTableEntry)buf_element.clone() );
            } else if ( clock_end <= buf_element.clock ) {
                TempoTableEntry tte = (TempoTableEntry)buf_element.clone();
                tte.clock = tte.clock - dclock;
                if ( clock_end == buf_element.clock ) {
                    tempoTable.add( tte );
                    just_on_clock_end_added = true;
                } else {
                    if ( !just_on_clock_end_added ) {
                        tempoTable.add( new TempoTableEntry( clock_start, tempo_at_clock_end, 0.0 ) );
                        just_on_clock_end_added = true;
                    }
                    tempoTable.add( tte );
                }
            }
        }
        if ( !just_on_clock_end_added ) {
            tempoTable.add( new TempoTableEntry( clock_start, tempo_at_clock_end, 0.0 ) );
        }
        updateTempoInfo();

        c = tracks.size();
        for ( int track = 1; track < c; track++ ) {
            // 削除する範囲に歌手変更イベントが存在するかどうかを検査。
            VsqEvent t_last_singer = null;
            for ( Iterator itr = tracks.get( track ).getSingerEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = (VsqEvent)itr.next();
                if ( clock_start <= ve.clock && ve.clock < clock_end ) {
                    t_last_singer = ve;
                }
                if ( ve.clock == clock_end ) {
                    t_last_singer = null; // 後でclock_endの位置に補うが、そこにに既に歌手変更イベントがあるとまずいので。
                }
            }
            VsqEvent last_singer = null;
            if ( t_last_singer != null ) {
                last_singer = (VsqEvent)t_last_singer.clone();
                last_singer.clock = clock_end;
            }

            changed = true;
            // イベントの削除
            while ( changed ) {
                changed = false;
                int event_count = tracks.get( track ).getEventCount();
                for ( int i = 0; i < event_count; i++ ) {
                    if ( clock_start <= tracks.get( track ).getEvent( i ).clock && tracks.get( track ).getEvent( i ).clock < clock_end ) {
                        tracks.get( track ).removeEvent( i );
                        changed = true;
                        break;
                    }
                }
            }

            // クロックのシフト
            if ( last_singer != null ) {
                tracks.get( track ).addEvent( last_singer ); //歌手変更イベントを補う
            }
            int event_count = tracks.get( track ).getEventCount();
            for ( int i = 0; i < event_count; i++ ) {
                if ( clock_end <= tracks.get( track ).getEvent( i ).clock ) {
                    tracks.get( track ).getEvent( i ).clock -= dclock;
                }
            }

            for ( String curve : _CURVES ) {
                if ( curve.equals( "VEL" ) ) {
                    continue;
                }
                VsqBPList buf_bplist = (VsqBPList)tracks.get( track ).getCurve( curve ).clone();
                tracks.get( track ).getCurve( curve ).clear();
                int value_at_end = buf_bplist.getValue( clock_end );
                boolean at_end_added = false;
                for ( Iterator itr = buf_bplist.keyClockIterator(); itr.hasNext(); ) {
                    int key = (Integer)itr.next();
                    if ( key < clock_start ) {
                        tracks.get( track ).getCurve( curve ).add( key, buf_bplist.getValue( key ) );
                    } else if ( clock_end <= key ) {
                        if ( key == clock_end ) {
                            at_end_added = true;
                        }
                        tracks.get( track ).getCurve( curve ).add( key - dclock, buf_bplist.getValue( key ) );
                    }
                }
                if ( !at_end_added ) {
                    tracks.get( track ).getCurve( curve ).add( clock_end - dclock, value_at_end );
                }
            }
        }
    }

    /// <summary>
    /// vsqファイル全体のイベントを，指定したクロックだけ遅らせます．
    /// ただし，曲頭のテンポ変更イベントと歌手変更イベントはクロック0から移動しません．
    /// この操作を行うことで，timeSigTableの情報は破綻します（仕様です）．
    /// </summary>
    /// <param name="delta_clock"></param>
    public static void shift( VsqFile vsq, int delta_clock ) {
        if ( delta_clock == 0 ) {
            return;
        }
        int dclock = (int)delta_clock;
        int c = vsq.tempoTable.size();
        for ( int i = 0; i < c; i++ ) {
            TempoTableEntry item = vsq.tempoTable.get( i );
            if ( item.clock > 0 ) {
                item.clock = item.clock + dclock;
            }
        }
        vsq.updateTempoInfo();
        c = vsq.tracks.size();
        for ( int track = 1; track < c; track++ ) {
            VsqTrack work = vsq.tracks.get( track );
            int event_count = work.getEventCount();
            for ( int i = 0; i < event_count; i++ ) {
                if ( work.getEvent( i ).clock > 0 ) {
                    work.getEvent( i ).clock += dclock;
                }
            }
            for ( String curve : _CURVES ) {
                if ( curve.equals( "VEL" ) ) {
                    continue;
                
                }
                // 順番に+=dclockしていくとVsqBPList内部のSortedListの値がかぶる可能性がある．
                VsqBPList edit = work.getCurve( curve );
                VsqBPList new_one = new VsqBPList( edit.defaultValue, edit.minimum, edit.maximum );
                for( int key : edit.getKeys() ){
                    new_one.add( key + dclock, edit.getValue( key ) );
                }
                work.setCurve( curve, new_one );
            }
        }
        vsq.updateTotalClocks();
    }

    /// <summary>
    /// このインスタンスのコピーを作成します
    /// </summary>
    /// <returns>このインスタンスのコピー</returns>
    public Object clone() {
        VsqFile ret = new VsqFile();
        ret.tracks = new Vector<VsqTrack>();
        int c = tracks.size();
        for ( int i = 0; i < c; i++ ) {
            ret.tracks.add( (VsqTrack)tracks.get( i ).clone() );
        }
        ret.tempoTable = new Vector<TempoTableEntry>();
        c = tempoTable.size();
        for ( int i = 0; i < c; i++ ) {
            ret.tempoTable.add( (TempoTableEntry)tempoTable.get( i ).clone() );
        }
        ret.timeSigTable = new Vector<TimeSigTableEntry>();
        c = timeSigTable.size();
        for ( int i = 0; i < c; i++ ) {
            ret.timeSigTable.add( (TimeSigTableEntry)timeSigTable.get( i ).clone() );
        }
        ret.m_tpq = m_tpq;
        ret.totalClocks = totalClocks;
        ret.m_base_tempo = m_base_tempo;
        ret.master = (VsqMaster)master.clone();
        ret.mixer = (VsqMixer)mixer.clone();
        return ret;
    }

    private VsqFile() {
    }

    private class BarLineIterator implements Iterator {
        private Vector<TimeSigTableEntry> m_list;
        private int m_end_clock;
        private int i;
        private int clock;
        int local_denominator;
        int local_numerator;
        int clock_step;
        int t_end;
        int local_clock;
        int bar_counter;

        public BarLineIterator( Vector<TimeSigTableEntry> list, int end_clock ) {
            m_list = list;
            m_end_clock = end_clock;
            i = 0;
            t_end = -1;
            clock = 0;
        }

        public Object next() {
            int mod = clock_step * local_numerator;
            if ( clock < t_end ) {
                if ( (clock - local_clock) % mod == 0 ) {
                    bar_counter++;
                    VsqBarLineType ret = new VsqBarLineType( clock, true, local_denominator, local_numerator, bar_counter );
                    clock += clock_step;
                    return ret;
                } else {
                    VsqBarLineType ret = new VsqBarLineType( clock, false, local_denominator, local_numerator, bar_counter );
                    clock += clock_step;
                    return ret;
                }
            }

            if( i < m_list.size() ) {
                local_denominator = m_list.get( i ).denominator;
                local_numerator = m_list.get( i ).numerator;
                local_clock = m_list.get( i ).clock;
                int local_bar_count = m_list.get( i ).barCount;
                clock_step = 480 * 4 / local_denominator;
                mod = clock_step * local_numerator;
                bar_counter = local_bar_count - 1;
                t_end = m_end_clock;
                if ( i + 1 < m_list.size() ) {
                    t_end = m_list.get( i + 1 ).clock;
                }
                i++;
                clock = local_clock;
                if( clock < t_end ){
                    if ( (clock - local_clock) % mod == 0 ) {
                        bar_counter++;
                        VsqBarLineType ret = new VsqBarLineType( clock, true, local_denominator, local_numerator, bar_counter );
                        clock += clock_step;
                        return ret;
                    } else {
                        VsqBarLineType ret = new VsqBarLineType( clock, false, local_denominator, local_numerator, bar_counter );
                        clock += clock_step;
                        return ret;
                    }
                }
            }
            return null;
        }

        public void remove() {
            //throw new NotImplementedException();
        }

        public boolean hasNext() {
            if ( clock < m_end_clock ) {
                return true;
            } else {
                return false;
            }
        }
    }

    /// <summary>
    /// 小節の区切りを順次返すIterator。
    /// </summary>
    /// <returns></returns>
    public Iterator getBarLineIterator( int end_clock ) {
        return new BarLineIterator( timeSigTable, end_clock ); 
    }

    /// <summary>
    /// 基本テンポ値を取得します
    /// </summary>
    public int getBaseTempo() {
        return m_base_tempo;
    }

    /// <summary>
    /// プリメジャー値を取得します
    /// </summary>
    public int getPreMeasure() {
        return master.preMeasure;
    }

    /// <summary>
    /// プリメジャー部分の長さをクロックに変換した値を取得します．
    /// </summary>
    public int getPreMeasureClocks() {
        return calculatePreMeasureInClock();
    }

    /// <summary>
    /// プリメジャーの長さ(クロック)を計算します。
    /// </summary>
    private int calculatePreMeasureInClock() {
        TimeSigTableEntry item = timeSigTable.get( 0 );
        int pre_measure = master.preMeasure;
        int last_bar_count = item.barCount;
        int last_clock = item.clock;
        int last_denominator = item.denominator;
        int last_numerator = item.numerator;
        int c = timeSigTable.size();
        for ( int i = 1; i < c; i++ ) {
            item = timeSigTable.get( i );
            if ( item.barCount >= pre_measure ) {
                break;
            } else {
                last_bar_count   = item.barCount;
                last_clock       = item.clock;
                last_denominator = item.denominator;
                last_numerator   = item.numerator;
            }
        }

        int remained = pre_measure - last_bar_count;//プリメジャーの終わりまでの残り小節数
        return last_clock + remained * last_numerator * 480 * 4 / last_denominator;
    }

    /// <summary>
    /// 指定したクロックにおける、clock=0からの演奏経過時間(sec)を取得します
    /// </summary>
    /// <param name="clock"></param>
    /// <returns></returns>
    public double getSecFromClock( double clock ) {
        for ( int i = tempoTable.size() - 1; i >= 0; i-- ) {
            TempoTableEntry item = tempoTable.get( i );
            if ( item.clock < clock ) {
                double init = item.time;
                double dclock = clock - item.clock;
                double sec_per_clock1 = item.tempo * 1e-6 / 480.0;
                return init + dclock * sec_per_clock1;
            }
        }

        double sec_per_clock = m_base_tempo * 1e-6 / 480.0;
        return clock * sec_per_clock;
    }

    /// <summary>
    /// 指定した時刻における、クロックを取得します
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public double getClockFromSec( double time ) {
        // timeにおけるテンポを取得
        int tempo = m_base_tempo;
        double base_clock = 0;
        double base_time = 0f;
        if ( tempoTable.size() == 0 ) {
            tempo = m_base_tempo;
            base_clock = 0;
            base_time = 0f;
        } else if ( tempoTable.size() == 1 ) {
            tempo = tempoTable.get( 0 ).tempo;
            base_clock = tempoTable.get( 0 ).clock;
            base_time = tempoTable.get( 0 ).time;
        } else {
            for ( int i = tempoTable.size() - 1; i >= 0; i-- ) {
                if ( tempoTable.get( i ).time < time ) {
                    return tempoTable.get( i ).clock + (time - tempoTable.get( i ).time) * m_tpq * 1000000.0 / tempoTable.get( i ).tempo;
                }
            }
        }
        double dt = time - base_time;
        return base_clock + dt * m_tpq * 1000000.0 / (double)tempo;
    }

    /// <summary>
    /// 指定したクロックにおける拍子を取得します
    /// </summary>
    /// <param name="clock"></param>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    public TimeSigTableEntry getTimesigAt( int clock ) {
        TimeSigTableEntry ret = new TimeSigTableEntry();
        int index = 0;
        for ( int i = timeSigTable.size() - 1; i >= 0; i-- ) {
            index = i;
            if ( timeSigTable.get( i ).clock <= clock ) {
                break;
            }
        }
        ret.numerator = timeSigTable.get( index ).numerator;
        ret.denominator = timeSigTable.get( index ).denominator;
        int diff = clock - timeSigTable.get( index ).clock;
        int clock_per_bar = 480 * 4 / ret.denominator * ret.numerator;
        ret.barCount = timeSigTable.get( index ).barCount + diff / clock_per_bar;
        return ret;
    }

    /// <summary>
    /// 指定したクロックにおけるテンポを取得します。
    /// </summary>
    /// <param name="clock"></param>
    /// <returns></returns>
    public int getTempoAt( int clock ) {
        int index = 0;
        for ( int i = tempoTable.size() - 1; i >= 0; i-- ) {
            index = i;
            if ( tempoTable.get( i ).clock <= clock ) {
                break;
            }
        }
        return tempoTable.get( index ).tempo;
    }

    /// <summary>
    /// 指定した小節の開始クロックを調べます。ここで使用する小節数は、プリメジャーを考慮しない。即ち、曲頭の小節が0である。
    /// </summary>
    /// <param name="bar_count"></param>
    /// <returns></returns>
    public int getClockFromBarCount( int bar_count ) {
        int index = 0;
        for ( int i = timeSigTable.size() - 1; i >= 0; i-- ) {
            index = i;
            if ( timeSigTable.get( i ).barCount <= bar_count ) {
                break;
            }
        }
        TimeSigTableEntry item = timeSigTable.get( index );
        int numerator = item.numerator;
        int denominator = item.denominator;
        int init_clock = item.clock;
        int init_bar_count = item.barCount;
        int clock_per_bar = numerator * 480 * 4 / denominator;
        return init_clock + (bar_count - init_bar_count) * clock_per_bar;
    }

    /// <summary>
    /// 指定したクロックが、曲頭から何小節目に属しているかを調べます。ここで使用する小節数は、プリメジャーを考慮しない。即ち、曲頭の小節が0である。
    /// </summary>
    /// <param name="clock"></param>
    /// <returns></returns>
    public int getBarCountFromClock( int clock ) {
        int index = 0;
        for ( int i = timeSigTable.size() - 1; i >= 0; i-- ) {
            index = i;
            if ( timeSigTable.get( i ).clock <= clock ) {
                break;
            }
        }
        int bar_count = 0;
        if ( index >= 0 ) {
            int last_clock = timeSigTable.get( index ).clock;
            int t_bar_count = timeSigTable.get( index ).barCount;
            int numerator = timeSigTable.get( index ).numerator;
            int denominator = timeSigTable.get( index ).denominator;
            int clock_per_bar = numerator * 480 * 4 / denominator;
            bar_count = t_bar_count + (clock - last_clock) / clock_per_bar;
        }
        return bar_count;
    }

    /// <summary>
    /// 4分の1拍子1音あたりのクロック数を取得します
    /// </summary>
    public int getTickPerQuarter() {
        return m_tpq;
    }

    /// <summary>
    /// 空のvsqファイルを構築します
    /// </summary>
    /// <param name="pre_measure"></param>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    /// <param name="tempo"></param>
    public VsqFile( String singer, int pre_measure, int numerator, int denominator, int tempo ) {
        totalClocks = pre_measure * 480 * 4 / denominator * numerator;
        m_tpq = 480;

        tracks = new Vector<VsqTrack>();
        tracks.add( new VsqTrack( tempo, numerator, denominator ) );
        tracks.add( new VsqTrack( "Voice1", singer ) );
        master = new VsqMaster( pre_measure );
        mixer = new VsqMixer( 0, 0, 0, 0 );
        mixer.slave.add( new VsqMixerEntry( 0, 0, 0, 0 ) );
        timeSigTable = new Vector<TimeSigTableEntry>();
        timeSigTable.add( new TimeSigTableEntry( 0, numerator, denominator, 0 ) );
        tempoTable = new Vector<TempoTableEntry>();
        tempoTable.add( new TempoTableEntry( 0, tempo, 0.0 ) );
        m_base_tempo = tempo;
    }

    /// <summary>
    /// vsqファイルからのコンストラクタ
    /// </summary>
    /// <param name="_fpath"></param>
    public VsqFile( String _fpath ) {
        tempoTable = new Vector<TempoTableEntry>();
        timeSigTable = new Vector<TimeSigTableEntry>();
        m_tpq = 480;

        // SMFをコンバートしたテキストファイルを作成
        try{
            MidiFile mf = new MidiFile( _fpath );
            tracks = new Vector<VsqTrack>();
            int num_track = mf.getTrackCount();
            for ( int i = 0; i < num_track; i++ ) {
                tracks.add( new VsqTrack( mf.getMidiEventList( i ) ) );
            }

            master = (VsqMaster)tracks.get( 1 ).getMaster().clone();
            mixer = (VsqMixer)tracks.get( 1 ).getMixer().clone();
            tracks.get( 1 ).setMaster( null );
            tracks.get( 1 ).setMixer( null );

            int master_track = -1;
            int c = tracks.size();
            for ( int i = 0; i < c; i++ ) {
                if ( tracks.get( i ).name.equals( "Master Track" ) ) {
                    master_track = i;
System.out.println( "master_track=" + master_track );
                    break;
                }
            }
System.out.println( "master_track=" + master_track );

            int prev_tempo;
            int prev_index;
            double prev_time;
            if ( master_track >= 0 ) {
                // TempoListの作成
                // MIDI event リストの取得
                Vector<MidiEventEx> midi_event = mf.getMidiEventList( master_track );//.tempoTable;
                // とりあえずtempo_tableに格納
                m_base_tempo = 500000;
                prev_tempo = 500000;
                prev_index = 0;
                double thistime;
                prev_time = 0.0;
                int count = -1;
                int midi_event_count = midi_event.size();
                for ( int j = 0; j < midi_event_count; j++ ) {
                    MidiEventEx me = midi_event.get( j );
                    if ( me.firstByte == 0xff && me.data.length >= 4 && me.data[0] == 0x51 ) {
                        count++;
                        if ( count == 0 && me.clock != 0 ) {
                            tempoTable.add( new TempoTableEntry( 0, 500000, 0.0 ) );
                            m_base_tempo = 500000;
                            prev_tempo = 500000;
                        }
                        int current_tempo = me.data[1] << 16 | me.data[2] << 8 | me.data[3];
                        int current_index = (int)me.clock;
                        thistime = prev_time + (double)(prev_tempo) * (double)(current_index - prev_index) / (m_tpq * 1000000.0);
                        tempoTable.add( new TempoTableEntry( current_index, current_tempo, thistime ) );
                        prev_tempo = current_tempo;
                        prev_index = current_index;
                        prev_time = thistime;
                    }
                }
                Collections.sort( tempoTable );

                // TimeSigTableの作成
                int dnomi = 4;
                int numer = 4;
                count = -1;
                for ( int j = 0; j < midi_event_count; j++ ) {
                    MidiEventEx me = midi_event.get( j );
                    if ( me.firstByte == 0xff && me.data.length >= 5 && me.data[0] == 0x58 ) {
                        count++;
                        numer = me.data[1];
                        dnomi = 1;
                        for ( int i = 0; i < me.data[2]; i++ ) {
                            dnomi = dnomi * 2;
                        }
                        if ( count == 0 ){
                            int numerator = 4;
                            int denominator = 4;
                            int clock = 0;
                            int bar_count = 0;
                            if ( me.clock == 0 ) {
                                timeSigTable.add( new TimeSigTableEntry( 0, numer, dnomi, 0 ) );
                            } else {
                                timeSigTable.add( new TimeSigTableEntry( 0, 4, 4, 0 ) );
                                timeSigTable.add( new TimeSigTableEntry( 0, numer, dnomi, (int)me.clock / (480 * 4) ) );
                                count++;
                            }
                        } else {
                            int numerator = timeSigTable.get( count - 1 ).numerator;
                            int denominator = timeSigTable.get( count - 1 ).denominator;
                            int clock = timeSigTable.get( count - 1 ).clock;
                            int bar_count = timeSigTable.get( count - 1 ).barCount;
                            int dif = 480 * 4 / denominator * numerator;//1小節が何クロックか？
                            bar_count += ((int)me.clock - clock) / dif;
                            timeSigTable.add( new TimeSigTableEntry( (int)me.clock, numer, dnomi, bar_count ) );
                        }
                    }
                }
            }
        }catch( Exception ex ){
        }

        // 曲の長さを計算
        updateTempoInfo();
        updateTimesigInfo();
        updateTotalClocks();
    }

    /// <summary>
    /// TimeSigTableの[*].clockの部分を更新します
    /// </summary>
    public void updateTimesigInfo() {
        if ( timeSigTable.get( 0 ).clock != 0 ) {
            //throw new ApplicationException( "initial timesig does not found" );
        }
        timeSigTable.get( 0 ).clock = 0;
        Collections.sort( timeSigTable );
        int c = timeSigTable.size();
        for ( int j = 1; j < c; j++ ) {
            int numerator = timeSigTable.get( j - 1 ).numerator;
            int denominator = timeSigTable.get( j - 1 ).denominator;
            int clock = timeSigTable.get( j - 1 ).clock;
            int bar_count = timeSigTable.get( j - 1 ).barCount;
            int dif = 480 * 4 / denominator * numerator;//1小節が何クロックか？
            clock += (timeSigTable.get( j ).barCount - bar_count) * dif;
            timeSigTable.get( j ).clock = clock;
        }
    }

    /// <summary>
    /// tempoTableの[*].Timeの部分を更新します
    /// </summary>
    public void updateTempoInfo() {
        if ( tempoTable.size() == 0 ) {
            tempoTable.add( new TempoTableEntry( 0, getBaseTempo(), 0.0 ) );
        }
        Collections.sort( tempoTable );
        if ( tempoTable.get( 0 ).clock != 0 ) {
            tempoTable.get( 0 ).time = (double)getBaseTempo() * (double)tempoTable.get( 0 ).clock / (getTickPerQuarter() * 1000000.0);
        } else {
            tempoTable.get( 0 ).time = 0.0;
        }
        double prev_time = tempoTable.get( 0 ).time;
        int prev_clock = tempoTable.get( 0 ).clock;
        int prev_tempo = tempoTable.get( 0 ).tempo;
        double inv_tpq_sec = 1.0 / (getTickPerQuarter() * 1000000.0);
        int c = tempoTable.size();
        for ( int i = 1; i < c; i++ ) {
            TempoTableEntry item = tempoTable.get( i );
            item.time = prev_time + (double)prev_tempo * (double)(item.clock - prev_clock) * inv_tpq_sec;
            prev_time = item.time;
            prev_tempo = item.tempo;
            prev_clock = item.clock;
        }
    }

    /// <summary>
    /// VsqFile.Executeの実行直後などに、m_total_clocksの値を更新する
    /// </summary>
    public void updateTotalClocks() {
        int max = getPreMeasureClocks();
        int c = tracks.size();
        for( int i = 1; i < c; i++ ){
            VsqTrack track = tracks.get( i );
            for ( Iterator itr = track.getEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = (VsqEvent)itr.next();
                max = Math.max( max, ve.clock + ve.id.length );
            }
            for ( String vct : _CURVES ) {
                if ( vct.equals( "VEL" ) ) {
                    continue;
                }
                if ( track.getCurve( vct ).getCount() > 0 ) {
                    int keys = track.getCurve( vct ).getCount();
                    int last_key = track.getCurve( vct ).getKeys()[keys - 1];
                    max = Math.max( max, last_key );
                }
            }
        }
        totalClocks = max;
    }

    /// <summary>
    /// 曲の長さを取得する。(sec)
    /// </summary>
    public double getTotalSec() {
        return getSecFromClock( (int)totalClocks );
    }

    /// <summary>
    /// 指定された番号のトラックに含まれる歌詞を指定されたファイルに出力します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="fpath"></param>
    public void printLyricTable( int track, String fpath ) {
        try{
            StreamWriter sw = new StreamWriter( fpath );
            VsqTrack work = tracks.get( track );
            int event_count = work.getEventCount();
            for ( int i = 0; i < event_count; i++ ) {
                VsqEvent item = work.getEvent( i );
                int Length;
                // timesignal
                int time_signal = item.clock;
                // イベントで指定されたIDがLyricであった場合
                if ( work.getEvent( i ).id.type == VsqIDType.Anote ) {
                    // 発音長を取得
                    Length = item.id.length;

                    // tempo_tableから、発音開始時のtempoを取得
                    int last = tempoTable.size() - 1;
                    int tempo = tempoTable.get( last ).tempo;
                    int prev_index = tempoTable.get( last ).clock;
                    double prev_time = tempoTable.get( last ).time;
                    int tempo_count = tempoTable.size();
                    for ( int j = 1; j < tempo_count; j++ ) {
                        if ( tempoTable.get( j ).clock > time_signal ) {
                            tempo = tempoTable.get( j - 1 ).tempo;
                            prev_index = tempoTable.get( j - 1 ).clock;
                            prev_time = tempoTable.get( j - 1 ).time;
                            break;
                        }
                    }
                    int current_index = work.getEvent( i ).clock;
                    double start_time = prev_time + (double)(current_index - prev_index) * (double)tempo / (m_tpq * 1000000.0);
                    // TODO: 単純に + Lengthしただけではまずいはず。要検討
                    double end_time = start_time + ((double)Length) * ((double)tempo) / (m_tpq * 1000000.0);
                    sw.writeLine( item.clock + "," +
                                  (new DecimalFormat( "0.000000" )).format( start_time ) + "," +
                                  (new DecimalFormat( "0.000000" )).format( end_time ) + "," +
                                  item.id.lyricHandle.L0.phrase + "," +
                                  item.id.lyricHandle.L0.getPhoneticSymbol() );
                }

            }
        }catch( Exception ex ){
            System.out.println( "VsqFile.printLyricTable; ex=" + ex );
        }
    }

    public Vector<MidiEventEx> generateMetaTextEvent( int track ) {
        String _NL = "" + (char)0x0a;
        Vector<MidiEventEx> ret = new Vector<MidiEventEx>();
        try{
            TextMemoryStream sr = new TextMemoryStream();
            tracks.get( track ).printMetaText( sr, totalClocks + 120, calculatePreMeasureInClock() );
            sr.rewind();
            int line_count = -1;
            String tmp = "";
            if ( sr.peek() >= 0 ) {
                tmp = sr.readLine();
                char[] line_char;
                String line = "";
                while ( sr.peek() >= 0 ) {
                    line = sr.readLine();
                    tmp += _NL + line;
                    while ( (tmp + getLinePrefix( line_count + 1 )).length() >= 127 ) {
                        line_count++;
                        tmp = getLinePrefix( line_count ) + tmp;
                        String work = tmp.substring( 0, 127 );
                        tmp = tmp.substring( 127 );
                        line_char = work.toCharArray();
                        MidiEventEx add = new MidiEventEx();
                        add.clock = 0;
                        add.firstByte = 0xff; //ステータス メタ＊
                        add.data = new byte[line_char.length + 1];
                        add.data[0] = 0x01; //メタテキスト
                        for ( int i = 0; i < line_char.length; i++ ) {
                            add.data[i + 1] = (byte)line_char[i];
                        }
                        ret.add( add );
                    }
                }
                // 残りを出力
                line_count++;
                tmp = getLinePrefix( line_count ) + tmp + _NL;
                while ( tmp.length() > 127 ) {
                    String work = tmp.substring( 0, 127 );
                    tmp = tmp.substring( 127 );
                    line_char = work.toCharArray();
                    MidiEventEx add = new MidiEventEx();
                    add.clock = 0;
                    add.firstByte = 0xff;
                    add.data = new byte[line_char.length + 1];
                    add.data[0] = 0x01;
                    for ( int i = 0; i < line_char.length; i++ ) {
                        add.data[i + 1] = (byte)line_char[i];
                    }
                    ret.add( add );
                    line_count++;
                    tmp = getLinePrefix( line_count );
                }
                line_char = tmp.toCharArray();
                MidiEventEx add0 = new MidiEventEx();
                add0.firstByte = 0xff;
                add0.data = new byte[line_char.length + 1];
                add0.data[0] = 0x01;
                for ( int i = 0; i < line_char.length; i++ ) {
                    add0.data[i + 1] = (byte)line_char[i];
                }
                ret.add( add0 );
            }
        }catch( Exception ex ){
            System.out.println( "VsqFile.generateMetaTextEvent; ex=" + ex );
        }
        return ret;
    }

    private static void printTrack( VsqFile vsq, int track, RandomAccessFile fs, int msPreSend ) throws IOException, Exception{
        //VsqTrack item = Tracks[track];
        String _NL = "" + (char)0x0a;
        //ヘッダ
        fs.write( _MTRK, 0, 4 );
        //データ長。とりあえず0
        fs.write( new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0, 4 );
        long first_position = fs.getFilePointer();
        //トラック名
        writeFlexibleLengthUnsignedLong( fs, 0x00 );//デルタタイム
        fs.writeByte( 0xff );//ステータスタイプ
        fs.writeByte( 0x03 );//イベントタイプSequence/tracks Name
        byte[] seq_name = vsq.tracks.get( track ).name.getBytes( "SJIS" );
        writeFlexibleLengthUnsignedLong( fs, (long)seq_name.length );//seq_nameの文字数
        fs.write( seq_name, 0, seq_name.length );
        
        //Meta Textを準備
        Vector<MidiEventEx> meta = vsq.generateMetaTextEvent( track );
        long lastclock = 0;
        for ( int i = 0; i < meta.size(); i++ ) {
            writeFlexibleLengthUnsignedLong( fs, (long)(meta.get( i ).clock - lastclock) );
            meta.get( i ).writeData( fs );
            lastclock = meta.get( i ).clock;
        }

        int last = 0;
        VsqNrpn[] data = generateNRPN( vsq, track, msPreSend );
        NrpnData[] nrpns = VsqNrpn.convert( data );
        for ( int i = 0; i < nrpns.length; i++ ) {
            writeFlexibleLengthUnsignedLong( fs, (long)(nrpns[i].getClock() - last) );
            fs.writeByte( 0xb0 );
            fs.writeByte( nrpns[i].getParameter() );
            fs.writeByte( nrpns[i].value );
            last = nrpns[i].getClock();
        }

        //トラックエンド
        VsqEvent last_event = vsq.tracks.get( track ).getEvent( vsq.tracks.get( track ).getEventCount() - 1 );
        int last_clock = last_event.clock + last_event.id.length;
        writeFlexibleLengthUnsignedLong( fs, (long)last_clock );
        fs.writeByte( 0xff );
        fs.writeByte( 0x2f );
        fs.writeByte( 0x00 );
        long pos = fs.getFilePointer();
        fs.seek( first_position - 4 );
        writeUnsignedInt( fs, (int)(pos - first_position) );
        fs.seek( pos );
    }

    /// <summary>
    /// 指定したクロックにおけるプリセンド・クロックを取得します
    /// </summary>
    /// <param name="clock"></param>
    /// <param name="msPreSend"></param>
    /// <returns></returns>
    public int getPresendClockAt( int clock, int msPreSend ) {
        double clock_msec = getSecFromClock( clock ) * 1000.0;
        float draft_clock_sec = (float)(clock_msec - msPreSend) / 1000.0f;
        int draft_clock = (int)Math.floor( getClockFromSec( draft_clock_sec ) );
        return clock - draft_clock;
    }

    /// <summary>
    /// 指定したトラックから、Expression(DYN)のNRPNリストを作成します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="msPreSend"></param>
    /// <returns></returns>
    public static VsqNrpn[] generateExpressionNRPN( VsqFile vsq, int track, int msPreSend ) {
        Vector<VsqNrpn> ret = new Vector<VsqNrpn>();
        VsqBPList dyn = vsq.tracks.get( track ).getCurve( "DYN" );
        int count = dyn.getCount();
        for ( int i = 0; i < count; i++ ) {
            int clock = dyn.getKeyClock( i );
            int c = clock - vsq.getPresendClockAt( clock, msPreSend );
            if ( c >= 0 ){
                VsqNrpn add = new VsqNrpn( c, 
                                           NRPN.CC_E_EXPRESSION,
                                           (byte)dyn.getElement( i ) );
                ret.add( add );
            }
        }
        return ret.toArray( new VsqNrpn[]{} );
    }

    public static VsqNrpn[] generateFx2DepthNRPN( VsqFile vsq, int track, int msPreSend ) {
        Vector<VsqNrpn> ret = new Vector<VsqNrpn>();
        VsqBPList fx2depth = vsq.tracks.get( track ).getCurve( "fx2depth" );
        int count = fx2depth.getCount();
        for ( int i = 0; i < count; i++ ) {
            int clock = fx2depth.getKeyClock( i );
            int c = clock - vsq.getPresendClockAt( clock, msPreSend );
            if ( c >= 0 ) {
                VsqNrpn add = new VsqNrpn( c,
                                           NRPN.CC_FX2_EFFECT2_DEPTH,
                                           (byte)fx2depth.getElement( i ) );
                ret.add( add );
            }
        }
        return ret.toArray( new VsqNrpn[]{} );
    }

    /// <summary>
    /// 先頭に記録されるNRPNを作成します
    /// </summary>
    /// <returns></returns>
    public static VsqNrpn generateHeaderNRPN() {
        VsqNrpn ret = new VsqNrpn( 0, NRPN.CC_BS_VERSION_AND_DEVICE, 0x00, 0x00 );
        ret.append( NRPN.CC_BS_DELAY, 0x00, 0x00 );
        ret.append( NRPN.CC_BS_LANGUAGE_TYPE, 0x00 );
        return ret;
    }

    /// <summary>
    /// 歌手変更イベントから，NRPNを作成します
    /// </summary>
    /// <param name="vsq"></param>
    /// <param name="ve"></param>
    /// <param name="msPreSend"></param>
    /// <returns></returns>
    public static VsqNrpn[] generateSingerNRPN( VsqFile vsq, VsqEvent ve, int msPreSend ) {
        int clock = ve.clock;

        double clock_msec = vsq.getSecFromClock( clock ) * 1000.0;

        int ttempo = vsq.getTempoAt( clock );
        double tempo = 6e7 / ttempo;
        double msEnd = vsq.getSecFromClock( ve.clock + ve.id.length ) * 1000.0;
        int duration = (int)Math.ceil( msEnd - clock_msec );
        int duration0 = getMsb( duration );
        int duration1 = getLsb( duration );
        int delay0 = getMsb( msPreSend );
        int delay1 = getLsb( msPreSend );
        Vector<VsqNrpn> ret = new Vector<VsqNrpn>();

        int i = clock - vsq.getPresendClockAt( clock, msPreSend );
        VsqNrpn add = new VsqNrpn( i, NRPN.CC_BS_VERSION_AND_DEVICE, 0x00, 0x00 );
        add.append( NRPN.CC_BS_DELAY, delay0, delay1, true );
        add.append( NRPN.CC_BS_LANGUAGE_TYPE, ve.id.iconHandle.language, true );
        add.append( NRPN.PC_VOICE_TYPE, ve.id.iconHandle.program );
        return new VsqNrpn[] { add };
    }

    /// <summary>
    /// 音符イベントから，NRPNを作成します
    /// </summary>
    /// <param name="ve"></param>
    /// <param name="msPreSend"></param>
    /// <param name="note_loc"></param>
    /// <returns></returns>
    public static VsqNrpn generateNoteNRPN( VsqFile vsq, int track, VsqEvent ve, int msPreSend, int note_loc, boolean add_delay_sign ) {
        int clock = ve.clock;
        String renderer = vsq.tracks.get( track ).getCommon().version;

        double clock_msec = vsq.getSecFromClock( clock ) * 1000.0;

        int ttempo = vsq.getTempoAt( clock );
        double tempo = 6e7 / ttempo;
        double msEnd = vsq.getSecFromClock( ve.clock + ve.id.length ) * 1000.0;
        int duration = (int)Math.ceil( msEnd - clock_msec );
        int duration0 = getMsb( duration );
        int duration1 = getLsb( duration );

        VsqNrpn add;
        if ( add_delay_sign ) {
            int delay0 = getMsb( msPreSend );
            int delay1 = getLsb( msPreSend );
            add = new VsqNrpn( clock - vsq.getPresendClockAt( clock, msPreSend ), NRPN.CVM_NM_VERSION_AND_DEVICE, 0x00, 0x00 );
            add.append( NRPN.CVM_NM_DELAY, delay0, delay1, true );
            add.append( NRPN.CVM_NM_NOTE_NUMBER, ve.id.note, true ); // Note number
        } else {
            add = new VsqNrpn( clock - vsq.getPresendClockAt( clock, msPreSend ), NRPN.CVM_NM_NOTE_NUMBER, ve.id.note ); // Note number
        }
        add.append( NRPN.CVM_NM_VELOCITY, ve.id.dynamics, true ); // Velocity
        add.append( NRPN.CVM_NM_NOTE_DURATION, duration0, duration1, true ); // Note duration
        add.append( NRPN.CVM_NM_NOTE_LOCATION, note_loc, true ); // Note Location

        if ( ve.id.vibratoHandle != null ) {
            add.append( NRPN.CVM_NM_INDEX_OF_VIBRATO_DB, 0x00, 0x00, true );
            int vibrato_type = VibratoTypeUtil.getVibratoTypeFromIconID( ve.id.vibratoHandle.iconID ).ordinal();
            int note_length = ve.id.length;
            int vibrato_delay = ve.id.vibratoDelay;
            int bVibratoDuration = (int)((float)(note_length - vibrato_delay) / (float)note_length * 127);
            int bVibratoDelay = (0x7f - bVibratoDuration);
            add.append( NRPN.CVM_NM_VIBRATO_CONFIG, vibrato_type, bVibratoDuration, true );
            add.append( NRPN.CVM_NM_VIBRATO_DELAY, bVibratoDelay, true );
        }

        String[] spl = ve.id.lyricHandle.L0.getPhoneticSymbolList();
        String s = "";
        for ( int j = 0; j < spl.length; j++ ) {
            s += spl[j];
        }
        char[] symbols = s.toCharArray();
        if ( renderer.startsWith( "DSB2" ) ) {
            add.append( 0x5011, 0x01, true );//TODO: 0x5011の意味は解析中
        }
        add.append( NRPN.CVM_NM_PHONETIC_SYMBOL_BYTES, symbols.length, true );// 0x12(Number of phonetic symbols : bytes)
        int count = -1;
        for ( int j = 0; j < spl.length; j++ ) {
            char[] chars = spl[j].toCharArray();
            for ( int k = 0; k < chars.length; k++ ) {
                count++;
                if ( k == 0 ) {
                    add.append( ((0x50 << 8) | (byte)(0x13 + count)), (byte)chars[k], (byte)ve.id.lyricHandle.L0.getConsonantAdjustment()[j], true ); // Phonetic symbol j
                } else {
                    add.append( ((0x50 << 8) | (byte)(0x13 + count)), (byte)chars[k], true ); // Phonetic symbol j
                }
            }
        }
        if ( !renderer.startsWith( "DSB2" ) ) {
            add.append( NRPN.CVM_NM_PHONETIC_SYMBOL_CONTINUATION, 0x7f, true ); // End of phonetic symbols
        }
        if ( renderer.startsWith( "DSB3" ) ) {
            int v1mean = ve.id.pmBendDepth * 60 / 100;
            if ( v1mean < 0 ) {
                v1mean = 0;
            }
            if ( 60 < v1mean ) {
                v1mean = 60;
            }
            int d1mean = (int)(0.3196 * ve.id.pmBendLength + 8.0);
            int d2mean = (int)(0.92 * ve.id.pmBendLength + 28.0);
            add.append( NRPN.CVM_NM_V1MEAN, v1mean, true );// 0x50(v1mean)
            add.append( NRPN.CVM_NM_D1MEAN, d1mean, true );// 0x51(d1mean)
            add.append( NRPN.CVM_NM_D1MEAN_FIRST_NOTE, 0x14, true );// 0x52(d1meanFirstNote)
            add.append( NRPN.CVM_NM_D2MEAN, d2mean, true );// 0x53(d2mean)
            add.append( NRPN.CVM_NM_D4MEAN, ve.id.d4mean, true );// 0x54(d4mean)
            add.append( NRPN.CVM_NM_PMEAN_ONSET_FIRST_NOTE, ve.id.pMeanOnsetFirstNote, true ); // 055(pMeanOnsetFirstNote)
            add.append( NRPN.CVM_NM_VMEAN_NOTE_TRNSITION, ve.id.vMeanNoteTransition, true ); // 0x56(vMeanNoteTransition)
            add.append( NRPN.CVM_NM_PMEAN_ENDING_NOTE, ve.id.pMeanEndingNote, true );// 0x57(pMeanEndingNote)
            add.append( NRPN.CVM_NM_ADD_PORTAMENTO, ve.id.pmbPortamentoUse, true );// 0x58(AddScoopToUpInternals&AddPortamentoToDownIntervals)
            add.append( NRPN.CVM_NM_CHANGE_AFTER_PEAK, 0x32, true );// 0x59(changeAfterPeak)
            int accent = (int)(0x64 * ve.id.demAccent / 100.0);
            add.append( NRPN.CVM_NM_ACCENT, accent, true );// 0x5a(Accent)
        }
        if ( renderer.startsWith( "UTU0" ) ) {
            // エンベロープ
            if ( ve.ustEvent != null ) {
                UstEnvelope env = null;
                if ( ve.ustEvent.envelope != null ) {
                    env = ve.ustEvent.envelope;
                } else {
                    env = new UstEnvelope();
                }
                int[] vals = null;
                if ( env.separator == "" ) {
                    vals = new int[7];
                } else {
                    vals = new int[10];
                }
                vals[0] = env.p1;
                vals[1] = env.p2;
                vals[2] = env.p3;
                vals[3] = env.v1;
                vals[4] = env.v2;
                vals[5] = env.v3;
                vals[6] = env.v4;
                if ( env.separator != "" ) {
                    vals[7] = env.p4;
                    vals[8] = env.p5;
                    vals[9] = env.v5;
                }
                for ( int i = 0; i < vals.length; i++ ) {
                    //(value3.msb & 0xf) << 28 | (value2.msb & 0x7f) << 21 | (value2.lsb & 0x7f) << 14 | (value1.msb & 0x7f) << 7 | (value1.lsb & 0x7f)
                    int msb, lsb;
                    int v = vals[i];
                    lsb = v & 0x7f;
                    v = v >>> 7;
                    msb = v & 0x7f;
                    v = v >>> 7;
                    add.append( NRPN.CVM_EXNM_ENV_DATA1, msb, lsb );
                    lsb = v & 0x7f;
                    v = v >>> 7;
                    msb = v & 0x7f;
                    v = v >>> 7;
                    add.append( NRPN.CVM_EXNM_ENV_DATA2, msb, lsb );
                    msb = v & 0xf;
                    add.append( NRPN.CVM_EXNM_ENV_DATA3, msb );
                    add.append( NRPN.CVM_EXNM_ENV_DATA_CONTINUATION, 0x00 );
                }
                add.append( NRPN.CVM_EXNM_ENV_DATA_CONTINUATION, 0x7f );

                // モジュレーション
                int m, l;
                if ( -100 <= ve.ustEvent.moduration && ve.ustEvent.moduration <= 100 ) {
                    int v = ve.ustEvent.moduration + 100;
                    m = getMsb( v );
                    l = getLsb( v );
                    add.append( NRPN.CVM_EXNM_MODURATION, m, l );
                }

                // 先行発声
                if ( ve.ustEvent.preUtterance != 0 ) {
                    int v = ve.ustEvent.preUtterance + 8192;
                    m = getMsb( v );
                    l = getLsb( v );
                    add.append( NRPN.CVM_EXNM_PRE_UTTERANCE, m, l );
                }

                // Flags
                if ( !ve.ustEvent.flags.equals( "" ) ) {
                    char[] arr = ve.ustEvent.flags.toCharArray();
                    add.append( NRPN.CVM_EXNM_FLAGS_BYTES, arr.length );
                    for ( int i = 0; i < arr.length; i++ ) {
                        int b = (int)arr[i];
                        add.append( NRPN.CVM_EXNM_FLAGS, b );
                    }
                    add.append( NRPN.CVM_EXNM_FLAGS_CONINUATION, 0x7f );
                }

                // オーバーラップ
                if ( ve.ustEvent.voiceOverlap != 0 ) {
                    int v = ve.ustEvent.voiceOverlap + 8192;
                    m = getMsb( v );
                    l = getLsb( v );
                    add.append( NRPN.CVM_EXNM_VOICE_OVERLAP, m, l );
                }
            }
        }
        add.append( NRPN.CVM_NM_NOTE_MESSAGE_CONTINUATION, 0x7f, true );// 0x7f(Note message continuation)
        return add;
    }

    /// <summary>
    /// 指定したトラックのデータから，NRPNを作成します
    /// </summary>
    /// <param name="vsq"></param>
    /// <param name="track"></param>
    /// <param name="msPreSend"></param>
    /// <param name="clock_start"></param>
    /// <param name="clock_end"></param>
    /// <returns></returns>
    public static VsqNrpn[] generateNRPN( VsqFile vsq, int track, int msPreSend, int clock_start, int clock_end ) throws Exception{
        VsqFile temp = (VsqFile)vsq.clone();
        temp.removePart( clock_end, vsq.totalClocks );
        if ( 0 < clock_start ) {
            temp.removePart( 0, clock_start );
        }
        temp.master.preMeasure = 1;
        VsqNrpn[] ret = generateNRPN( temp, track, msPreSend );
        temp = null;
        return ret;
    }

    /// <summary>
    /// 指定したトラックのデータから，NRPNを作成します
    /// </summary>
    /// <param name="vsq"></param>
    /// <param name="track"></param>
    /// <param name="msPreSend"></param>
    /// <returns></returns>
    public static VsqNrpn[] generateNRPN( VsqFile vsq, int track, int msPreSend ) throws Exception{
        Vector<VsqNrpn> list = new Vector<VsqNrpn>();

        VsqTrack target = vsq.tracks.get( track );
        String version = target.getCommon().version;

        int count = target.getEventCount();
        int note_start = 0;
        int note_end = target.getEventCount() - 1;
        for ( int i = 0; i < target.getEventCount(); i++ ) {
            if ( 0 <= target.getEvent( i ).clock ) {
                note_start = i;
                break;
            }
            note_start = i;
        }
        for ( int i = target.getEventCount() - 1; i >= 0; i-- ) {
            if ( target.getEvent( i ).clock <= vsq.totalClocks ) {
                note_end = i;
                break;
            }
        }

        // 最初の歌手を決める
        int singer_event = -1;
        for ( int i = note_start; i >= 0; i-- ) {
            if ( target.getEvent( i ).id.type == VsqIDType.Singer ) {
                singer_event = i;
                break;
            }
        }
        if ( singer_event >= 0 ) { //見つかった場合
            list.addAll( Arrays.asList( generateSingerNRPN( vsq, target.getEvent( singer_event ), 0 ) ) );
        } else {                   //多分ありえないと思うが、歌手が不明の場合。
            throw new Exception( "first singer was not specified" );
            //list.add( new VsqNrpn( 0, NRPN.CC_BS_LANGUAGE_TYPE, 0 ) );
            //list.add( new VsqNrpn( 0, NRPN.PC_VOICE_TYPE, 0 ) );
        }

        list.addAll( Arrays.asList( generateVoiceChangeParameterNRPN( vsq, track, msPreSend ) ) );
        if ( version.startsWith( "DSB2" ) ) {
            list.addAll( Arrays.asList( generateFx2DepthNRPN( vsq, track, msPreSend ) ) );
        }

        int ms_presend = msPreSend;
        if ( version.startsWith( "UTU0" ) ) {
            double sec_maxlen = 0.0;
            for ( Iterator itr = target.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = (VsqEvent)itr.next();
                double len = vsq.getSecFromClock( ve.clock + ve.id.length ) - vsq.getSecFromClock( ve.clock );
                sec_maxlen = Math.max( sec_maxlen, len );
            }
            ms_presend += (int)(sec_maxlen * 1000.0);
        }
        VsqBPList dyn = target.getCurve( "dyn" );
        if ( dyn.getCount() > 0 ) {
            Vector<VsqNrpn> listdyn = new Vector<VsqNrpn>( Arrays.asList( generateExpressionNRPN( vsq, track, ms_presend ) ) );
            if ( listdyn.size() > 0 ) {
                list.addAll( listdyn );
            }
        }
        VsqBPList pbs = target.getCurve( "pbs" );
        if ( pbs.getCount() > 0 ) {
            Vector<VsqNrpn> listpbs = new Vector<VsqNrpn>( Arrays.asList( generatePitchBendSensitivityNRPN( vsq, track, ms_presend ) ) );
            if ( listpbs.size() > 0 ) {
                list.addAll( listpbs );
            }
        }
        VsqBPList pit = target.getCurve( "pit" );
        if ( pit.getCount() > 0 ) {
            Vector<VsqNrpn> listpit = new Vector<VsqNrpn>( Arrays.asList( generatePitchBendNRPN( vsq, track, ms_presend ) ) );
            if ( listpit.size() > 0 ) {
                list.addAll( listpit );
            }
        }

        boolean first = true;
        int last_note_end = 0;
        for ( int i = note_start; i <= note_end; i++ ) {
            VsqEvent item = target.getEvent( i );
            if ( item.id.type == VsqIDType.Anote ) {
                byte note_loc = 0x03;
                if ( item.clock == last_note_end ) {
                    note_loc -= 0x02;
                }

                // 次に現れる音符イベントを探す
                int nextclock = item.clock + item.id.length + 1;
                for ( int j = i + 1; j < target.getEventCount(); j++ ) {
                    if ( target.getEvent( j ).id.type == VsqIDType.Anote ) {
                        nextclock = target.getEvent( j ).clock;
                        break;
                    }
                }
                if ( item.clock + item.id.length == nextclock ) {
                    note_loc -= 0x01;
                }

                list.addAll( Arrays.asList( generateNoteNRPN( vsq,
                                                             track,
                                                             item,
                                                             msPreSend,
                                                             note_loc,
                                                             first ) ) );
                first = false;
                list.addAll( Arrays.asList( generateVibratoNRPN( vsq,
                                                                 item,
                                                                 msPreSend ) ) );
                last_note_end = item.clock + item.id.length;
            } else if ( item.id.type == VsqIDType.Singer ) {
                if ( i > note_start && i != singer_event ) {
                    list.addAll( Arrays.asList( generateSingerNRPN( vsq, item, msPreSend ) ) );
                }
            }
        }

        list = VsqNrpn.sort( list );
        Vector<VsqNrpn> merged = new Vector<VsqNrpn>();
        for ( int i = 0; i < list.size(); i++ ) {
            merged.addAll( Arrays.asList( list.get( i ).expand() ) );
        }
        return merged.toArray( new VsqNrpn[]{} );
    }

    /// <summary>
    /// 指定したトラックから、PitchBendのNRPNを作成します
    /// </summary>
    /// <param name="vsq"></param>
    /// <param name="track"></param>
    /// <param name="msPreSend"></param>
    /// <returns></returns>
    public static VsqNrpn[] generatePitchBendNRPN( VsqFile vsq, int track, int msPreSend ) {
        Vector<VsqNrpn> ret = new Vector<VsqNrpn>();
        VsqBPList pit = vsq.tracks.get( track ).getCurve( "PIT" );
        int count = pit.getCount();
        for ( int i = 0; i < count; i++ ) {
            int clock = pit.getKeyClock( i );
            int value = pit.getElement( i ) + 0x2000;
            int value0 = getMsb( value );
            int value1 = getLsb( value );
            int c = clock - vsq.getPresendClockAt( clock, msPreSend );
            if ( c >= 0 ) {
                VsqNrpn add = new VsqNrpn( c,
                                           NRPN.PB_PITCH_BEND,
                                           value0,
                                           value1 );
                ret.add( add );
            }
        }
        return ret.toArray( new VsqNrpn[]{} );
    }

    /// <summary>
    /// 指定したトラックからPitchBendSensitivityのNRPNを作成します
    /// </summary>
    /// <param name="vsq"></param>
    /// <param name="track"></param>
    /// <param name="msPreSend"></param>
    /// <returns></returns>
    public static VsqNrpn[] generatePitchBendSensitivityNRPN( VsqFile vsq, int track, int msPreSend ) {
        Vector<VsqNrpn> ret = new Vector<VsqNrpn>();
        VsqBPList pbs = vsq.tracks.get( track ).getCurve( "PBS" );
        int count = pbs.getCount();
        for ( int i = 0; i < count; i++ ) {
            int clock = pbs.getKeyClock( i );
            int c = clock - vsq.getPresendClockAt( clock, msPreSend );
            if ( c >= 0 ) {
                VsqNrpn add = new VsqNrpn( c,
                                           NRPN.CC_PBS_PITCH_BEND_SENSITIVITY,
                                           pbs.getElement( i ),
                                           0x00 );
                ret.add( add );
            }
        }
        return ret.toArray( new VsqNrpn[]{} );
    }

    /// <summary>
    /// 指定した音符イベントから，ビブラート出力用のNRPNを作成します
    /// </summary>
    /// <param name="vsq"></param>
    /// <param name="ve"></param>
    /// <param name="msPreSend"></param>
    /// <returns></returns>
    public static VsqNrpn[] generateVibratoNRPN( VsqFile vsq, VsqEvent ve, int msPreSend ) {
        Vector<VsqNrpn> ret = new Vector<VsqNrpn>();
        if ( ve.id.vibratoHandle != null ){
            int vclock = ve.clock + ve.id.vibratoDelay;
            int delay0 = getMsb( vclock );
            int delay1 = getLsb( vclock );
            VsqNrpn add2 = new VsqNrpn( vclock - vsq.getPresendClockAt( vclock, msPreSend ),
                                        NRPN.CC_VD_VERSION_AND_DEVICE,
                                        0x00,
                                        0x00 );
            add2.append( NRPN.CC_VD_DELAY, delay0, delay1 );
            add2.append( NRPN.CC_VD_VIBRATO_DEPTH, ve.id.vibratoHandle.startDepth );
            add2.append( NRPN.CC_VR_VIBRATO_RATE, ve.id.vibratoHandle.startRate );
            ret.add( add2 );
            int vlength = ve.id.length - ve.id.vibratoDelay;
            if ( ve.id.vibratoHandle.rateBP.getCount() > 0 ) {
                for ( int i = 0; i < ve.id.vibratoHandle.rateBP.getCount(); i++ ) {
                    float percent = ve.id.vibratoHandle.rateBP.getElement( i ).x;
                    int cl = vclock + (int)(percent * vlength);
                    ret.add( new VsqNrpn( cl - vsq.getPresendClockAt( cl, msPreSend ), NRPN.CC_VR_VIBRATO_RATE, (byte)ve.id.vibratoHandle.rateBP.getElement( i ).y ) );
                }
            }
            if ( ve.id.vibratoHandle.depthBP.getCount() > 0 ) {
                for ( int i = 0; i < ve.id.vibratoHandle.depthBP.getCount(); i++ ) {
                    float percent = ve.id.vibratoHandle.depthBP.getElement( i ).x;
                    int cl = vclock + (int)(percent * vlength);
                    ret.add( new VsqNrpn( cl - vsq.getPresendClockAt( cl, msPreSend ), NRPN.CC_VD_VIBRATO_DEPTH, (byte)ve.id.vibratoHandle.depthBP.getElement( i ).y ) );
                }
            }
        }
        Collections.sort( ret );
        return ret.toArray( new VsqNrpn[]{} );
    }

    /// <summary>
    /// 指定したトラックから、VoiceChangeParameterのNRPNのリストを作成します
    /// </summary>
    /// <param name="vsq"></param>
    /// <param name="track"></param>
    /// <param name="msPreSend"></param>
    /// <returns></returns>
    public static VsqNrpn[] generateVoiceChangeParameterNRPN( VsqFile vsq, int track, int msPreSend ) {
        int premeasure_clock = vsq.getPreMeasureClocks();
        String renderer = vsq.tracks.get( track ).getCommon().version;
        Vector<VsqNrpn> res = new Vector<VsqNrpn>();

        String[] curves;
        if ( renderer.startsWith( "DSB3" ) ) {
            curves = new String[] { "BRE", "BRI", "CLE", "POR", "OPE", "GEN" };
        } else if ( renderer.startsWith( "DSB2" ) ) {
            curves = new String[] { "BRE", "BRI", "CLE", "POR", "GEN", "harmonics",
                                    "reso1amp", "reso1bw", "reso1freq", 
                                    "reso2amp", "reso2bw", "reso2freq",
                                    "reso3amp", "reso3bw", "reso3freq",
                                    "reso4amp", "reso4bw", "reso4freq" };
        } else {
            curves = new String[] { "BRE", "BRI", "CLE", "POR", "GEN" };
        }

        for ( int i = 0; i < curves.length; i++ ) {
            VsqBPList vbpl = vsq.tracks.get( track ).getCurve( curves[i] );
            if ( vbpl.getCount() > 0 ) {
                byte lsb = NRPN.getVoiceChangeParameterID( curves[i] );
                int count = vbpl.getCount();
                for ( int j = 0; j < count; j++ ) {
                    int clock = vbpl.getKeyClock( j );
                    int c = clock - vsq.getPresendClockAt( clock, msPreSend );
                    if ( c >= 0 ){
                        VsqNrpn add = new VsqNrpn( c,
                                                   NRPN.VCP_VOICE_CHANGE_PARAMETER_ID,
                                                   lsb );
                        add.append( NRPN.VCP_VOICE_CHANGE_PARAMETER, vbpl.getElement( j ), true );
                        res.add( add );
                    }
                }
            }
        }
        Collections.sort( res );
        return res.toArray( new VsqNrpn[]{} );
    }

    private static int getMsb( int value ){
        return 0x7f & (value >>> 7);
    }

    private static int getLsb( int value ){
        return 0x7f & value;
    }

    public Vector<MidiEventEx> generateTimeSig() {
        Vector<MidiEventEx> events = new Vector<MidiEventEx>();
        for ( TimeSigTableEntry entry : timeSigTable ) {
            events.add( MidiEventEx.generateTimeSigEvent( entry.clock, entry.numerator, entry.denominator ) );
        }
        return events;
    }

    public Vector<MidiEventEx> generateTempoChange() {
        Vector<MidiEventEx> events = new Vector<MidiEventEx>();
        for ( TempoTableEntry entry : tempoTable ) {
            events.add( MidiEventEx.generateTempoChangeEvent( entry.clock, entry.tempo ) );
        }
        return events;
    }

    /// <summary>
    /// このインスタンスをファイルに出力します
    /// </summary>
    /// <param name="file"></param>
    public void write( String file ) {
        write( file, 500 );
    }

    /// <summary>
    /// このインスタンスをファイルに出力します
    /// </summary>
    /// <param name="file"></param>
    /// <param name="msPreSend">プリセンドタイム(msec)</param>
    public void write( String file, int msPreSend ) {
        int last_clock = 0;
        for ( int track = 1; track < tracks.size(); track++ ) {
            if ( tracks.get( track ).getEventCount() > 0 ) {
                int index = tracks.get( track ).getEventCount() - 1;
                VsqEvent last = tracks.get( track ).getEvent( index );
                last_clock = Math.max( last_clock, last.clock + last.id.length );
            }
        }

        RandomAccessFile fs = null;
        try{
            fs = new RandomAccessFile( file, "rw" );
            long first_position;//チャンクの先頭のファイル位置

            // ヘッダ
            //チャンクタイプ
            fs.write( _MTHD, 0, 4 );
            //データ長
            fs.writeByte( 0x00 );
            fs.writeByte( 0x00 );
            fs.writeByte( 0x00 );
            fs.writeByte( 0x06 );
            //フォーマット
            fs.writeByte( 0x00 );
            fs.writeByte( 0x01 );
            //トラック数
            writeUnsignedShort( fs, this.tracks.size() );
            //時間単位
            fs.writeByte( 0x01 );
            fs.writeByte( 0xe0 );

            // master tracks
            //チャンクタイプ
            fs.write( _MTRK, 0, 4 );
            //データ長。とりあえず0を入れておく
            fs.write( new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0, 4 );
            first_position = fs.getFilePointer();
            //トラック名
            writeFlexibleLengthUnsignedLong( fs, 0 );//デルタタイム
            fs.writeByte( 0xff );//ステータスタイプ
            fs.writeByte( 0x03 );//イベントタイプSequence/tracks Name
            fs.writeByte( (byte)_MASTER_TRACK.length );//トラック名の文字数。これは固定
            fs.write( _MASTER_TRACK, 0, _MASTER_TRACK.length );

            Vector<MidiEventEx> events = new Vector<MidiEventEx>();
            for ( TimeSigTableEntry entry : timeSigTable ) {
                events.add( MidiEventEx.generateTimeSigEvent( entry.clock, entry.numerator, entry.denominator ) );
                last_clock = Math.max( last_clock, entry.clock );
            }
            for ( TempoTableEntry entry : tempoTable ) {
                events.add( MidiEventEx.generateTempoChangeEvent( entry.clock, entry.tempo ) );
                last_clock = Math.max( last_clock, entry.clock );
            }
            Collections.sort( events );
            long last = 0;
            for ( MidiEventEx me : events ) {
                writeFlexibleLengthUnsignedLong( fs, me.clock - last );
                me.writeData( fs );
                last = me.clock;
            }

            //WriteFlexibleLengthUnsignedLong( fs, (ulong)(last_clock + 120 - last) );
            writeFlexibleLengthUnsignedLong( fs, 0 );
            fs.writeByte( 0xff );
            fs.writeByte( 0x2f );//イベントタイプEnd of tracks
            fs.writeByte( 0x00 );
            long pos = fs.getFilePointer();
            fs.seek( first_position - 4 );
            writeUnsignedInt( fs, (int)(pos - first_position) );
            fs.seek( pos );

            // トラック
            VsqFile temp = (VsqFile)this.clone();
            temp.tracks.get( 1 ).setMaster( (VsqMaster)master.clone() );
            temp.tracks.get( 1 ).setMixer( (VsqMixer)mixer.clone() );
            printTrack( temp, 1, fs, msPreSend );
            for ( int track = 2; track < tracks.size(); track++ ) {
                printTrack( this, track, fs, msPreSend );
            }
        }catch( Exception ex ){
            System.out.println( "VsqFile.write; ex=" + ex );
        }finally{
            if( fs != null ){
                try{
                    fs.close();
                }catch( Exception ex1 ){
                }
            }
        }
    }

    /// <summary>
    /// メタテキストの行番号から、各行先頭のプレフィクス文字列("DM:0123:"等)を作成します
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public static String getLinePrefix( int count ) {
        int digits = getHowManyDigits( count );
        int c = (digits - 1) / 4 + 1;
        String format = "";
        for ( int i = 0; i < c; i++ ) {
            format += "0000";
        }
        return "DM:" + (new DecimalFormat( format )).format( count ) + ":";
    }

    /// <summary>
    /// 数numberの桁数を調べます。（10進数のみ）
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private static int getHowManyDigits( int number ) {
        int val;
        if ( number > 0 ) {
            val = number;
        } else {
            val = -number;
        }
        int i = 1;
        int digits = 1;
        while ( true ) {
            i++;
            digits *= 10;
            if ( val < digits ) {
                return i - 1;
            }
        }
    }

    /// <summary>
    /// char[]を書き込む。
    /// </summary>
    /// <param name="fs"></param>
    /// <param name="item"></param>
    public static void writeCharArray( RandomAccessFile fs, char[] item ) throws IOException{
        for ( int i = 0; i < item.length; i++ ) {
            fs.writeByte( (byte)item[i] );
        }
    }

    /// <summary>
    /// ushort値をビッグエンディアンでfsに書き込みます
    /// </summary>
    /// <param name="data"></param>
    public static void writeUnsignedShort( RandomAccessFile fs, int data ) throws IOException{
        fs.writeByte( (0xff00 & data) >>> 8 );
        fs.writeByte( 0x00ff & data );
    }

    /// <summary>
    /// uint値をビッグエンディアンでfsに書き込みます
    /// </summary>
    /// <param name="data"></param>
    public static void writeUnsignedInt( RandomAccessFile fs, int data ) throws IOException{
        fs.writeByte( (0xff000000 & data) >>> 24 );
        fs.writeByte( (0x00ff0000 & data) >>> 16 );
        fs.writeByte( (0x0000ff00 & data) >>> 8 );
        fs.writeByte( 0x000000ff & data );
    }

    /// <summary>
    /// SMFの可変長数値表現を使って、ulongをbyte[]に変換します
    /// </summary>
    /// <param name="number"></param>
    public static byte[] getBytesFlexibleLengthUnsignedLong( long number ) {
        boolean[] bits = new boolean[64];
        long val = 0x1;
        bits[0] = (number & val) == val;
        for ( int i = 1; i < 64; i++ ) {
            val = val << 1;
            bits[i] = (number & val) == val;
        }
        int first = 0;
        for ( int i = 63; i >= 0; i-- ) {
            if ( bits[i] ) {
                first = i;
                break;
            }
        }
        // 何バイト必要か？
        int bytes = first / 7 + 1;
        byte[] ret = new byte[bytes];
        for ( int i = 1; i <= bytes; i++ ) {
            int num = 0;
            int count = 0x80;
            for ( int j = (bytes - i + 1) * 7 - 1; j >= (bytes - i + 1) * 7 - 6 - 1; j-- ) {
                count = count >> 1;
                if ( bits[j] ) {
                    num += count;
                }
            }
            if ( i != bytes ) {
                num += 0x80;
            }
            ret[i - 1] = (byte)num;
        }
        return ret;
    }

    /// <summary>
    /// 整数を書き込む。フォーマットはSMFの可変長数値表現。
    /// </summary>
    /// <param name="fs"></param>
    /// <param name="number"></param>
    public static void writeFlexibleLengthUnsignedLong( RandomAccessFile fs, long number ) throws IOException{
        byte[] bytes = getBytesFlexibleLengthUnsignedLong( number );
        fs.write( bytes, 0, bytes.length );
    }
}
