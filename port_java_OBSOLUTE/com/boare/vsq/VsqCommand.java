/*
 * VsqCommand.java
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
import com.boare.corlib.*;

public class VsqCommand {
    public VsqCommandType type;
    /// <summary>
    /// コマンドの処理内容を保持します。Args具体的な内容は、処理するクラスごとに異なります
    /// </summary>
    public Object[] args;
    /// <summary>
    /// 後続するコマンド
    /// </summary>
    public Vector<VsqCommand> children = new Vector<VsqCommand>();
    /// <summary>
    /// このコマンドの親
    /// </summary>
    public VsqCommand parent = null;

    /// <summary>
    /// VsqCommandはgenerateCommand*からコンストラクトしなければならない。
    /// </summary>
    public VsqCommand() {
    }

    public static VsqCommand generateCommandRoot() {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.Root;
        command.args = null;
        return command;
    }

    public static VsqCommand generateCommandReplace( VsqFile vsq ) {
        VsqCommand command = new VsqCommand();
        command.args = new Object[1];
        command.type = VsqCommandType.Replace;
        command.args[0] = (VsqFile)vsq.clone();
        return command;
    }

    public static VsqCommand generateCommandTrackReplace( int track, VsqTrack item ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.TrackReplace;
        command.args = new Object[2];
        command.args[0] = track;
        command.args[1] = (VsqTrack)item.clone();
        return command;
    }

    public static VsqCommand generateCommandUpdateTimesig( int bar_count, int new_barcount, int numerator, int denominator ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.UpdateTimesig;
        command.args = new Object[4];
        command.args[0] = bar_count;
        command.args[1] = numerator;
        command.args[2] = denominator;
        command.args[3] = new_barcount;
        return command;
    }

    public static VsqCommand generateCommandUpdateTimesigRange( int[] bar_counts, int[] new_barcounts, int[] numerators, int[] denominators ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.UpdateTimesigRange;
        command.args = new Object[4];
        command.args[0] = (int[])bar_counts.clone();
        command.args[1] = (int[])numerators.clone();
        command.args[2] = (int[])denominators.clone();
        command.args[3] = (int[])new_barcounts.clone();
        return command;
    }

    public static VsqCommand generateCommandUpdateTempoRange( int[] clocks, int[] new_clocks, int[] tempos ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.UpdateTempoRange;
        command.args = new Object[3];
        command.args[0] = (int[])clocks.clone();
        command.args[1] = (int[])tempos.clone();
        command.args[2] = (int[])new_clocks.clone();
        return command;
    }

    public static VsqCommand generateCommandUpdateTempo( int clock, int new_clock, int tempo ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.UpdateTempo;
        command.args = new Object[3];
        command.args[0] = clock;
        command.args[1] = tempo;
        command.args[2] = new_clock;
        return command;
    }

    public static VsqCommand generateCommandChangePreMeasure( int pre_measure ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.ChangePreMeasure;
        command.args = new Object[1];
        command.args[0] = pre_measure;
        return command;
    }

    public static VsqCommand generateCommandDeleteTrack( int track ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.DeleteTrack;
        command.args = new Object[1];
        command.args[0] = track;
        return command;
    }

    /// <summary>
    /// トラックを追加するコマンドを発行します．trackはClone()して渡さなくてもよい
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandAddTrack( VsqTrack track, VsqMixerEntry mixer, int position ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.AddTrack;
        command.args = new Object[5];
        command.args[0] = track;
        command.args[1] = mixer;
        command.args[2] = position;
        return command;
    }

    /// <summary>
    /// トラック名を変更するコマンドを作成します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="new_name"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandTrackChangeName( int track, String new_name ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.TrackChangeName;
        command.args = new Object[2];
        command.args[0] = track;
        command.args[1] = new_name;
        return command;
    }

    public static VsqCommand generateCommandTrackChangePlayMode( int track, int play_mode ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.TrackChangePlayMode;
        command.args = new Object[2];
        command.args[0] = track;
        command.args[1] = play_mode;
        return command;
    }

    /// <summary>
    /// VsqIDとClockを同時に変更するコマンドを発行します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="internal_ids"></param>
    /// <param name="clocks"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeClockAndIDContaintsRange( int track, int[] internal_ids, int[] clocks, VsqID[] values ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeClockAndIDContaintsRange;
        int count = internal_ids.length;
        command.args = new Object[4];
        command.args[0] = track;
        command.args[1] = (int[])internal_ids.clone();
        command.args[2] = (int[])clocks.clone();
        command.args[3] = (VsqID[])values.clone();
        return command;
    }

    /// <summary>
    /// VsqIDとClockを同時に変更するコマンドを発行します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="internal_id"></param>
    /// <param name="clock"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeClockAndIDContaints( int track, int internal_id, int clock, VsqID value ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeClockAndIDContaints;
        command.args = new Object[4];
        command.args[0] = track;
        command.args[1] = internal_id;
        command.args[2] = clock;
        command.args[3] = (VsqID)value.clone();
        return command;
    }

    /// <summary>
    /// VsqIDの内容を変更するコマンドを発行します。
    /// </summary>
    /// <param name="track"></param>
    /// <param name="internal_ids"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeIDContaintsRange( int track, int[] internal_ids, VsqID[] values ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeIDContaintsRange;
        command.args = new Object[3];
        command.args[0] = track;
        command.args[1] = (int[])internal_ids.clone();
        VsqID[] list = new VsqID[values.length];
        for ( int i = 0; i < values.length; i++ ) {
            list[i] = (VsqID)values[i].clone();
        }
        command.args[2] = list;
        return command;
    }

    /// <summary>
    /// VsqIDの内容を変更するコマンドを発行します。
    /// </summary>
    /// <param name="track"></param>
    /// <param name="internal_id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeIDContaints( int track, int internal_id, VsqID value ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeIDContaints;
        command.args = new Object[3];
        command.args[0] = track;
        command.args[1] = internal_id;
        command.args[2] = (VsqID)value.clone();
        return command;
    }

    /// <summary>
    /// ノートの長さを変更するコマンドを発行します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="internal_id"></param>
    /// <param name="new_clock"></param>
    /// <param name="new_length"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeClockAndLength( int track, int internal_id, int new_clock, int new_length ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeClockAndLength;
        command.args = new Object[4];
        command.args[0] = track;
        command.args[1] = internal_id;
        command.args[2] = new_clock;
        command.args[3] = new_length;
        return command;
    }

    /// <summary>
    /// ノートの長さを変更するコマンドを発行します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="internal_id"></param>
    /// <param name="new_length"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeLength( int track, int internal_id, int new_length ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeLength;
        command.args = new Object[3];
        command.args[0] = track;
        command.args[1] = internal_id;
        command.args[2] = new_length;
        return command;
    }

    /// <summary>
    /// 指定したトラックの，音符のベロシティ(VEL)を変更するコマンドを発行します．
    /// リストvelocityには，音符を指定するInteralIDと，変更したいベロシティの値のペアを登録します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="velocity"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeVelocity( int track, Vector<KeyValuePair<Integer, Integer>> velocity ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeVelocity;
        command.args = new Object[2];
        command.args[0] = track;
        Vector<KeyValuePair<Integer, Integer>> list = new Vector<KeyValuePair<Integer, Integer>>();
        for ( KeyValuePair<Integer, Integer> item : velocity ) {
            list.add( new KeyValuePair<Integer, Integer>( item.key, item.value ) );
        }
        command.args[1] = list;
        return command;
    }

    public static VsqCommand generateCommandEventReplace( int track, VsqEvent item ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventReplace;
        command.args = new Object[2];
        command.args[0] = track;
        command.args[1] = item.clone();
        return command;
    }

    public static VsqCommand generateCommandEventReplaceRange( int track, VsqEvent[] items ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventReplaceRange;
        command.args = new Object[2];
        command.args[0] = track;
        VsqEvent[] objs = new VsqEvent[items.length];
        for( int i = 0; i < items.length; i++ ){
            objs[i] = (VsqEvent)items[i].clone();
        }
        command.args[1] = objs;
        return command;
    }

    /// <summary>
    /// 指定したトラックの、音符のアクセント(Accent)を変更するコマンドを発行します。
    /// リストaccent_listには、音符を指定するInternalIDと、変更したいアクセント値のペアを登録します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="accent_list"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeAccent( int track, Vector<KeyValuePair<Integer, Integer>> accent_list ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeAccent;
        command.args = new Object[2];
        command.args[0] = track;
        Vector<KeyValuePair<Integer, Integer>> list = new Vector<KeyValuePair<Integer, Integer>>();
        for ( KeyValuePair<Integer, Integer> item : accent_list ) {
            list.add( new KeyValuePair<Integer, Integer>( item.key, item.value ) );
        }
        command.args[1] = list;
        return command;
    }

    /// <summary>
    /// 指定したトラックの、音符のディケイ(Decay)を変更するコマンドを発行します。
    /// リストdecay_listには、音符を指定するInternalIDと、変更したいディケイ値のペアを登録します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="decay_list"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeDecay( int track, Vector<KeyValuePair<Integer, Integer>> decay_list ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeDecay;
        command.args = new Object[2];
        command.args[0] = track;
        Vector<KeyValuePair<Integer, Integer>> list = new Vector<KeyValuePair<Integer, Integer>>();
        for ( KeyValuePair<Integer, Integer> item : decay_list ) {
            list.add( new KeyValuePair<Integer, Integer>( item.key, item.value ) );
        }
        command.args[1] = list;
        return command;
    }

    /// <summary>
    /// vsqファイルのカーブを編集するコマンドを発行します．
    /// </summary>
    /// <param name="track"></param>
    /// <param name="target"></param>
    /// <param name="edit"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandTrackEditCurve( int track, String target, Vector<BPPair> edit ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.TrackEditCurve;
        command.args = new Object[5];
        command.args[0] = track;
        command.args[1] = target;
        Vector<BPPair> copied = new Vector<BPPair>();
        for ( BPPair item : edit ) {
            copied.add( item );
        }
        command.args[2] = copied;
        return command;
    }

    public static VsqCommand generateCommandTrackEditCurveRange( int track, String[] targets, Vector<Vector<BPPair>> edits ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.TrackEditCurveRange;
        command.args = new Object[3];
        command.args[0] = track;
        command.args[1] = (String[])targets.clone();
        Vector<Vector<BPPair>> cpy = new Vector<Vector<BPPair>>();//[targets.length];
        for ( int i = 0; i < edits.size(); i++ ) {
            Vector<BPPair> copied = new Vector<BPPair>();
            for ( BPPair item : edits.get( i ) ) {
                copied.add( new BPPair( item.clock, item.value ) );
            }
            cpy.add( copied );
        }
        command.args[2] = cpy;
        return command;
    }

    /// <summary>
    /// 特定位置のイベントの歌詞と発音記号を変更するコマンドを発行します。
    /// </summary>
    /// <param name="track"></param>
    /// <param name="internal_id"></param>
    /// <param name="phrase"></param>
    /// <param name="phonetic_symbol"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeLyric( int track, int internal_id, String phrase, String phonetic_symbol, boolean protect_symbol ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeLyric;
        command.args = new Object[5];
        command.args[0] = track;
        command.args[1] = internal_id;
        command.args[2] = phrase;
        command.args[3] = phonetic_symbol;
        command.args[4] = protect_symbol;
        return command;
    }

    /// <summary>
    /// ノートのクロック位置を変更するコマンドを発行します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="internal_id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeClock( int track, int internal_id, int value ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeClock;
        command.args = new Object[3];
        command.args[0] = track;
        command.args[1] = internal_id;
        command.args[2] = value;
        return command;
    }

    public static VsqCommand generateCommandEventDeleteRange( int track, int[] internal_ids ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventDeleteRange;
        command.args = new Object[2];
        command.args[0] = (int[])internal_ids.clone();
        command.args[1] = track;
        return command;
    }

    /// <summary>
    /// ノートを削除するコマンドを発行します
    /// </summary>
    /// <param name="clock"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventDelete( int track, int internal_id ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventDelete;
        command.args = new Object[2];
        command.args[1] = track;
        command.args[0] = internal_id;
        return command;
    }

    public static VsqCommand generateCommandEventAddRange( int track, VsqEvent[] items ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventAddRange;
        command.args = new Object[2];
        command.args[0] = track;
        command.args[1] = (VsqEvent[])items.clone();
        return command;
    }

    /// <summary>
    /// ノートを追加するコマンドを発行します。
    /// </summary>
    /// <param name="track"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventAdd( int track, VsqEvent item ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventAdd;
        command.args = new Object[2];
        command.args[0] = track;
        command.args[1] = (VsqEvent)item.clone();
        return command;
    }

    /// <summary>
    /// ノートの音程を変更するコマンドを発行します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="internal_id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeNote( int track, int internal_id, int note ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeNote;
        command.args = new Object[3];
        command.args[0] = track;
        command.args[1] = internal_id;
        command.args[2] = note;
        return command;
    }

    /// <summary>
    /// ノートの音程とクロックを変更するコマンドを発行します
    /// </summary>
    /// <param name="track"></param>
    /// <param name="internal_id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static VsqCommand generateCommandEventChangeClockAndNote( int track, int internal_id, int clock, int note ) {
        VsqCommand command = new VsqCommand();
        command.type = VsqCommandType.EventChangeClockAndNote;
        command.args = new Object[4];
        command.args[0] = track;
        command.args[1] = internal_id;
        command.args[2] = clock;
        command.args[3] = note;
        return command;
    }
}
