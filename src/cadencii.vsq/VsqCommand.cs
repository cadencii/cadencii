/*
 * VsqCommand.cs
 * Copyright © 2008-2011 kbinani
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
using cadencii;
using cadencii.java.util;

namespace cadencii.vsq
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class VsqCommand
    {
        public VsqCommandType Type;
        /// <summary>
        /// コマンドの処理内容を保持します。Args具体的な内容は、処理するクラスごとに異なります
        /// </summary>
        public Object[] Args;
        /// <summary>
        /// 後続するコマンド
        /// </summary>
        public List<VsqCommand> Children = new List<VsqCommand>();
        /// <summary>
        /// このコマンドの親
        /// </summary>
        public VsqCommand Parent = null;

        /// <summary>
        /// VsqCommandはgenerateCommand*からコンストラクトしなければならない。
        /// </summary>
        public VsqCommand()
        {
        }

        public static VsqCommand generateCommandRoot()
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.ROOT;
            command.Args = null;
            return command;
        }

        public static VsqCommand generateCommandReplace(VsqFile vsq)
        {
            VsqCommand command = new VsqCommand();
            command.Args = new Object[1];
            command.Type = VsqCommandType.REPLACE;
            command.Args[0] = (VsqFile)vsq.clone();
            return command;
        }

        public static VsqCommand generateCommandTrackReplace(int track, VsqTrack item)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_REPLACE;
            command.Args = new Object[2];
            command.Args[0] = track;
            command.Args[1] = (VsqTrack)item.clone();
            return command;
        }

        public static VsqCommand generateCommandUpdateTimesig(int bar_count, int new_barcount, int numerator, int denominator)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.UPDATE_TIMESIG;
            command.Args = new Object[4];
            command.Args[0] = bar_count;
            command.Args[1] = numerator;
            command.Args[2] = denominator;
            command.Args[3] = new_barcount;
            return command;
        }

        public static VsqCommand generateCommandUpdateTimesigRange(int[] bar_counts, int[] new_barcounts, int[] numerators, int[] denominators)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.UPDATE_TIMESIG_RANGE;
            command.Args = new Object[4];
            command.Args[0] = copyIntArray(bar_counts);
            command.Args[1] = copyIntArray(numerators);
            command.Args[2] = copyIntArray(denominators);
            command.Args[3] = copyIntArray(new_barcounts);
            return command;
        }

        public static VsqCommand generateCommandUpdateTempoRange(int[] clocks, int[] new_clocks, int[] tempos)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.UPDATE_TEMPO_RANGE;
            command.Args = new Object[3];
            command.Args[0] = copyIntArray(clocks);
            command.Args[1] = copyIntArray(tempos);
            command.Args[2] = copyIntArray(new_clocks);
            return command;
        }

        public static VsqCommand generateCommandUpdateTempo(int clock, int new_clock, int tempo)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.UPDATE_TEMPO;
            command.Args = new Object[3];
            command.Args[0] = clock;
            command.Args[1] = tempo;
            command.Args[2] = new_clock;
            return command;
        }

        public static VsqCommand generateCommandChangePreMeasure(int pre_measure)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.CHANGE_PRE_MEASURE;
            command.Args = new Object[1];
            command.Args[0] = pre_measure;
            return command;
        }

        public static VsqCommand generateCommandDeleteTrack(int track)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_DELETE;
            command.Args = new Object[1];
            command.Args[0] = track;
            return command;
        }

        /// <summary>
        /// トラックを追加するコマンドを発行します．trackはClone()して渡さなくてもよい
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandAddTrack(VsqTrack track, VsqMixerEntry mixer, int position)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_ADD;
            command.Args = new Object[3];
            command.Args[0] = track;
            command.Args[1] = mixer;
            command.Args[2] = position;
            return command;
        }

        /// <summary>
        /// トラック名を変更するコマンドを作成します
        /// </summary>
        /// <param name="track"></param>
        /// <param name="new_name"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandTrackChangeName(int track, string new_name)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_CHANGE_NAME;
            command.Args = new Object[2];
            command.Args[0] = track;
            command.Args[1] = new_name;
            return command;
        }

        public static VsqCommand generateCommandTrackChangePlayMode(int track, int play_mode, int last_play_mode)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_CHANGE_PLAY_MODE;
            command.Args = new Object[3];
            command.Args[0] = track;
            command.Args[1] = play_mode;
            command.Args[2] = last_play_mode;
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
        public static VsqCommand generateCommandEventChangeClockAndIDContaintsRange(int track, int[] internal_ids, int[] clocks, VsqID[] values)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_CLOCK_AND_ID_CONTAINTS_RANGE;
            int count = internal_ids.Length;
            command.Args = new Object[4];
            command.Args[0] = track;
            command.Args[1] = copyIntArray(internal_ids);
            command.Args[2] = copyIntArray(clocks);
            VsqID[] cp_values = new VsqID[values.Length];
            for (int i = 0; i < values.Length; i++) {
                cp_values[i] = (VsqID)values[i].clone();
            }
            command.Args[3] = cp_values;
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
        public static VsqCommand generateCommandEventChangeClockAndIDContaints(int track, int internal_id, int clock, VsqID value)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_CLOCK_AND_ID_CONTAINTS;
            command.Args = new Object[4];
            command.Args[0] = track;
            command.Args[1] = internal_id;
            command.Args[2] = clock;
            command.Args[3] = (VsqID)value.clone();
            return command;
        }

        /// <summary>
        /// VsqIDの内容を変更するコマンドを発行します。
        /// </summary>
        /// <param name="track"></param>
        /// <param name="internal_ids"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandEventChangeIDContaintsRange(int track, int[] internal_ids, VsqID[] values)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_ID_CONTAINTS_RANGE;
            command.Args = new Object[3];
            command.Args[0] = track;
            command.Args[1] = copyIntArray(internal_ids);
            VsqID[] list = new VsqID[values.Length];
            for (int i = 0; i < values.Length; i++) {
                list[i] = (VsqID)values[i].clone();
            }
            command.Args[2] = list;
            return command;
        }

        /// <summary>
        /// VsqIDの内容を変更するコマンドを発行します。
        /// </summary>
        /// <param name="track"></param>
        /// <param name="internal_id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandEventChangeIDContaints(int track, int internal_id, VsqID value)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_ID_CONTAINTS;
            command.Args = new Object[3];
            command.Args[0] = track;
            command.Args[1] = internal_id;
            command.Args[2] = (VsqID)value.clone();
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
        public static VsqCommand generateCommandEventChangeClockAndLength(int track, int internal_id, int new_clock, int new_length)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_CLOCK_AND_LENGTH;
            command.Args = new Object[4];
            command.Args[0] = track;
            command.Args[1] = internal_id;
            command.Args[2] = new_clock;
            command.Args[3] = new_length;
            return command;
        }

        /// <summary>
        /// ノートの長さを変更するコマンドを発行します
        /// </summary>
        /// <param name="track"></param>
        /// <param name="internal_id"></param>
        /// <param name="new_length"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandEventChangeLength(int track, int internal_id, int new_length)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_LENGTH;
            command.Args = new Object[3];
            command.Args[0] = track;
            command.Args[1] = internal_id;
            command.Args[2] = new_length;
            return command;
        }

        /// <summary>
        /// 指定したトラックの，音符のベロシティ(VEL)を変更するコマンドを発行します．
        /// リストvelocityには，音符を指定するInteralIDと，変更したいベロシティの値のペアを登録します
        /// </summary>
        /// <param name="track"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandEventChangeVelocity(int track, List<ValuePair<int, int>> velocity)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_VELOCITY;
            command.Args = new Object[2];
            command.Args[0] = track;
            List<ValuePair<int, int>> list = new List<ValuePair<int, int>>();
            foreach (var item in velocity) {
                list.Add(new ValuePair<int, int>(item.getKey(), item.getValue()));
            }
            command.Args[1] = list;
            return command;
        }

        public static VsqCommand generateCommandEventReplace(int track, VsqEvent item)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_REPLACE;
            command.Args = new Object[2];
            command.Args[0] = track;
            command.Args[1] = item.clone();
            return command;
        }

        public static VsqCommand generateCommandEventReplaceRange(int track, VsqEvent[] items)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_REPLACE_RANGE;
            command.Args = new Object[2];
            command.Args[0] = track;
            VsqEvent[] objs = new VsqEvent[items.Length];
            for (int i = 0; i < items.Length; i++) {
                objs[i] = (VsqEvent)items[i].clone();
            }
            command.Args[1] = objs;
            return command;
        }

        /// <summary>
        /// 指定したトラックの、音符のアクセント(Accent)を変更するコマンドを発行します。
        /// リストaccent_listには、音符を指定するInternalIDと、変更したいアクセント値のペアを登録します
        /// </summary>
        /// <param name="track"></param>
        /// <param name="accent_list"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandEventChangeAccent(int track, List<ValuePair<int, int>> accent_list)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_ACCENT;
            command.Args = new Object[2];
            command.Args[0] = track;
            List<ValuePair<int, int>> list = new List<ValuePair<int, int>>();
            foreach (var item in accent_list) {
                list.Add(new ValuePair<int, int>(item.getKey(), item.getValue()));
            }
            command.Args[1] = list;
            return command;
        }

        /// <summary>
        /// 指定したトラックの、音符のディケイ(Decay)を変更するコマンドを発行します。
        /// リストdecay_listには、音符を指定するInternalIDと、変更したいディケイ値のペアを登録します
        /// </summary>
        /// <param name="track"></param>
        /// <param name="decay_list"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandEventChangeDecay(int track, List<ValuePair<int, int>> decay_list)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_DECAY;
            command.Args = new Object[2];
            command.Args[0] = track;
            List<ValuePair<int, int>> list = new List<ValuePair<int, int>>();
            foreach (var item in decay_list) {
                list.Add(new ValuePair<int, int>(item.getKey(), item.getValue()));
            }
            command.Args[1] = list;
            return command;
        }

        public static VsqCommand generateCommandTrackCurveReplaceRange(int track, string[] target_curve, VsqBPList[] bplist)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_CURVE_REPLACE_RANGE;
            command.Args = new Object[3];
            command.Args[0] = track;
            string[] arr = new string[target_curve.Length];
            for (int i = 0; i < target_curve.Length; i++) {
                arr[i] = target_curve[i];
            }
            command.Args[1] = arr;
            VsqBPList[] cp = new VsqBPList[bplist.Length];
            for (int i = 0; i < bplist.Length; i++) {
                cp[i] = (VsqBPList)bplist[i].clone();
            }
            command.Args[2] = cp;
            return command;
        }

        public static VsqCommand generateCommandTrackCurveReplace(int track, string target_curve, VsqBPList bplist)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_CURVE_REPLACE;
            command.Args = new Object[3];
            command.Args[0] = track;
            command.Args[1] = target_curve;
            command.Args[2] = bplist.clone();
            return command;
        }

        /*public static VsqCommand generateCommandTrackRemovePoints( int track, String target, Vector<Long> ids ) {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_CURVE_REMOVE_POINTS;
            command.Args = new Object[3];
            command.Args[0] = track;
            command.Args[1] = target;
            Vector<Long> cpy = new Vector<Long>();
            if ( ids != null ){
                int count = ids.size();
                for ( int i = 0; i < count; i++ ) {
                    cpy.add( ids.get( i ) );
                }
            }
            command.Args[2] = cpy;
            return command;
        }*/

        /// <summary>
        /// vsqファイルのカーブを編集するコマンドを発行します．
        /// </summary>
        /// <param name="track"></param>
        /// <param name="target"></param>
        /// <param name="edit"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandTrackCurveEdit(int track, string target, List<BPPair> edit)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_CURVE_EDIT;
            command.Args = new Object[3];
            command.Args[0] = track;
            command.Args[1] = target;
            List<BPPair> copied = new List<BPPair>();
            foreach (var item in edit) {
                copied.Add(item);
            }
            command.Args[2] = copied;
            return command;
        }

        /// <summary>
        /// コントロールカーブを編集するコマンドを発行します．
        /// </summary>
        /// <param name="track">編集対象のコントロールカーブが含まれるトラックの番号</param>
        /// <param name="target">編集対象のコントロールカーブ名</param>
        /// <param name="delete">削除を行うデータ点のリスト</param>
        /// <param name="add_or_move">追加または移動を行うデータ点のリスト</param>
        /// <returns></returns>
        public static VsqCommand generateCommandTrackCurveEdit2(int track, string target, List<long> delete, SortedDictionary<int, VsqBPPair> add)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_CURVE_EDIT2;
            command.Args = new Object[4];
            command.Args[0] = track;
            command.Args[1] = target;
            List<long> cp_delete = new List<long>();
            foreach (var id in delete) {
                cp_delete.Add(id);
            }
            command.Args[2] = cp_delete;

            SortedDictionary<int, VsqBPPair> cp_add = new SortedDictionary<int, VsqBPPair>();
            foreach (var clock in add.Keys) {
                VsqBPPair item = add[clock];
                cp_add[clock] = item;
            }
            command.Args[3] = cp_add;
            return command;
        }

        public static VsqCommand generateCommandTrackCurveEditRange(int track, List<string> targets, List<List<BPPair>> edits)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_CURVE_EDIT_RANGE;
            command.Args = new Object[3];
            command.Args[0] = track;
            List<string> cp_targets = new List<string>();
            int count = targets.Count;
            for (int i = 0; i < count; i++) {
                cp_targets.Add(targets[i]);
            }
            command.Args[1] = cp_targets;
            List<List<BPPair>> cp_edits = new List<List<BPPair>>();
            count = edits.Count;
            for (int i = 0; i < count; i++) {
                List<BPPair> copied = new List<BPPair>();
                foreach (var item in edits[i]) {
                    copied.Add(new BPPair(item.Clock, item.Value));
                }
                cp_edits.Add(copied);
            }
            command.Args[2] = cp_edits;
            return command;
        }

        /// <summary>
        /// コントロールカーブを編集するコマンドを発行します．
        /// </summary>
        /// <param name="track">編集対象のコントロールカーブが含まれるトラックの番号</param>
        /// <param name="target">編集対象のコントロールカーブ名</param>
        /// <param name="delete">削除を行うデータ点のリスト</param>
        /// <param name="add_or_move">追加または移動を行うデータ点のリスト</param>
        /// <returns></returns>
        public static VsqCommand generateCommandTrackCurveEdit2All(int track, List<string> target, List<List<long>> delete, List<SortedDictionary<int, VsqBPPair>> add)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.TRACK_CURVE_EDIT2_ALL;
            command.Args = new Object[4];
            command.Args[0] = track;
            List<string> cp_target = new List<string>();
            int c = target.Count;
            for (int i = 0; i < c; i++) {
                cp_target.Add(target[i]);
            }
            command.Args[1] = cp_target;

            List<List<long>> cp_vec_delete = new List<List<long>>();
            c = delete.Count;
            for (int i = 0; i < c; i++) {
                List<long> cp_delete = new List<long>();
                foreach (var id in delete[i]) {
                    cp_delete.Add(id);
                }
                cp_vec_delete.Add(cp_delete);
            }
            command.Args[2] = cp_vec_delete;

            List<SortedDictionary<int, VsqBPPair>> cp_vec_add = new List<SortedDictionary<int, VsqBPPair>>();
            c = add.Count;
            for (int i = 0; i < c; i++) {
                SortedDictionary<int, VsqBPPair> cp_add = new SortedDictionary<int, VsqBPPair>();
                SortedDictionary<int, VsqBPPair> tmp = add[i];
                foreach (var clock in tmp.Keys) {
                    VsqBPPair item = tmp[clock];
                    cp_add[clock] = item;
                }
                cp_vec_add.Add(cp_add);
            }
            command.Args[3] = cp_vec_add;
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
        public static VsqCommand generateCommandEventChangeLyric(int track, int internal_id, string phrase, string phonetic_symbol, bool protect_symbol)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_LYRIC;
            command.Args = new Object[5];
            command.Args[0] = track;
            command.Args[1] = internal_id;
            command.Args[2] = phrase;
            command.Args[3] = phonetic_symbol;
            command.Args[4] = protect_symbol;
            return command;
        }

        /// <summary>
        /// ノートのクロック位置を変更するコマンドを発行します
        /// </summary>
        /// <param name="track"></param>
        /// <param name="internal_id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandEventChangeClock(int track, int internal_id, int value)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_CLOCK;
            command.Args = new Object[3];
            command.Args[0] = track;
            command.Args[1] = internal_id;
            command.Args[2] = value;
            return command;
        }

        public static VsqCommand generateCommandEventDeleteRange(int track, List<int> internal_ids)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_DELETE_RANGE;
            command.Args = new Object[2];
            command.Args[0] = track;
            command.Args[1] = copyIntVector(internal_ids);
            return command;
        }

        /// <summary>
        /// ノートを削除するコマンドを発行します
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandEventDelete(int track, int internal_id)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_DELETE;
            command.Args = new Object[2];
            command.Args[0] = track;
            command.Args[1] = internal_id;
            return command;
        }

        public static VsqCommand generateCommandEventAddRange(int track, VsqEvent[] items)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_ADD_RANGE;
            command.Args = new Object[2];
            command.Args[0] = track;
            VsqEvent[] cp_items = new VsqEvent[items.Length];
            for (int i = 0; i < items.Length; i++) {
                cp_items[i] = (VsqEvent)items[i].clone();
            }
            command.Args[1] = cp_items;
            return command;
        }

        /// <summary>
        /// ノートを追加するコマンドを発行します。
        /// </summary>
        /// <param name="track"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandEventAdd(int track, VsqEvent item)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_ADD;
            command.Args = new Object[2];
            command.Args[0] = track;
            command.Args[1] = (VsqEvent)item.clone();
            return command;
        }

        /// <summary>
        /// ノートの音程を変更するコマンドを発行します
        /// </summary>
        /// <param name="track"></param>
        /// <param name="internal_id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandEventChangeNote(int track, int internal_id, int note)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_NOTE;
            command.Args = new Object[3];
            command.Args[0] = track;
            command.Args[1] = internal_id;
            command.Args[2] = note;
            return command;
        }

        /// <summary>
        /// ノートの音程とクロックを変更するコマンドを発行します
        /// </summary>
        /// <param name="track"></param>
        /// <param name="internal_id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static VsqCommand generateCommandEventChangeClockAndNote(int track, int internal_id, int clock, int note)
        {
            VsqCommand command = new VsqCommand();
            command.Type = VsqCommandType.EVENT_CHANGE_CLOCK_AND_NOTE;
            command.Args = new Object[4];
            command.Args[0] = track;
            command.Args[1] = internal_id;
            command.Args[2] = clock;
            command.Args[3] = note;
            return command;
        }

        private static int[] copyIntArray(int[] value)
        {
            int[] ret = new int[value.Length];
            for (int i = 0; i < value.Length; i++) {
                ret[i] = value[i];
            }
            return ret;
        }

        private static List<int> copyIntVector(List<int> value)
        {
            List<int> ret = new List<int>();
            int count = value.Count;
            for (int i = 0; i < count; i++) {
                ret.Add(value[i]);
            }
            return ret;
        }
    }

}
