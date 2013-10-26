/*
 * VsqTrack.cs
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
using cadencii.java.io;

namespace cadencii.vsq
{

    /// <summary>
    /// Stores the data of a vsq track.
    /// </summary>
    [Serializable]
    public class VsqTrack : ICloneable
    {
        public static readonly string[] CURVES = new string[] { 
            "bre", 
            "bri",
            "cle", 
            "dyn", 
            "gen", 
            "ope", 
            "pbs", 
            "pit", 
            "por", 
            "harmonics", 
            "fx2depth", 
            "reso1amp", 
            "reso1bw", 
            "reso1freq", 
            "reso2amp", 
            "reso2bw", 
            "reso2freq", 
            "reso3amp", 
            "reso3bw", 
            "reso3freq", 
            "reso4amp", 
            "reso4bw", 
            "reso4freq", };
        public string Tag;
        public VsqMetaText MetaText;

        private class IndexIterator : Iterator<int>
        {
            VsqEventList list;
            int pos;
            bool kindSinger = false;
            bool kindNote = false;
            bool kindCrescend = false;
            bool kindDecrescend = false;
            bool kindDynaff = false;

            public IndexIterator(VsqEventList list, int iterator_kind)
            {
                this.list = list;
                pos = -1;
                kindSinger = (iterator_kind & IndexIteratorKind.SINGER) == IndexIteratorKind.SINGER;
                kindNote = (iterator_kind & IndexIteratorKind.NOTE) == IndexIteratorKind.NOTE;
                kindCrescend = (iterator_kind & IndexIteratorKind.CRESCEND) == IndexIteratorKind.CRESCEND;
                kindDecrescend = (iterator_kind & IndexIteratorKind.DECRESCEND) == IndexIteratorKind.DECRESCEND;
                kindDynaff = (iterator_kind & IndexIteratorKind.DYNAFF) == IndexIteratorKind.DYNAFF;
            }

            public bool hasNext()
            {
                int count = list.getCount();
                for (int i = pos + 1; i < count; i++) {
                    VsqEvent item = list.getElement(i);
                    if (kindSinger) {
                        if (item.ID.type == VsqIDType.Singer) {
                            return true;
                        }
                    }
                    if (kindNote) {
                        if (item.ID.type == VsqIDType.Anote) {
                            return true;
                        }
                    }
                    if (item.ID.type == VsqIDType.Aicon && item.ID.IconDynamicsHandle != null && item.ID.IconDynamicsHandle.IconID != null) {
                        string iconid = item.ID.IconDynamicsHandle.IconID;
                        if (kindDynaff) {
                            if (iconid.StartsWith(IconDynamicsHandle.ICONID_HEAD_DYNAFF)) {
                                return true;
                            }
                        }
                        if (kindCrescend) {
                            if (iconid.StartsWith(IconDynamicsHandle.ICONID_HEAD_CRESCEND)) {
                                return true;
                            }
                        }
                        if (kindDecrescend) {
                            if (iconid.StartsWith(IconDynamicsHandle.ICONID_HEAD_DECRESCEND)) {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }

            public int next()
            {
                int count = list.getCount();
                for (int i = pos + 1; i < count; i++) {
                    VsqEvent item = list.getElement(i);
                    if (kindSinger) {
                        if (item.ID.type == VsqIDType.Singer) {
                            pos = i;
                            return i;
                        }
                    }
                    if (kindNote) {
                        if (item.ID.type == VsqIDType.Anote) {
                            pos = i;
                            return i;
                        }
                    }
                    if (kindDynaff || kindCrescend || kindDecrescend) {
                        if (item.ID.type == VsqIDType.Aicon && item.ID.IconDynamicsHandle != null && item.ID.IconDynamicsHandle.IconID != null) {
                            string iconid = item.ID.IconDynamicsHandle.IconID;
                            if (kindDynaff) {
                                if (iconid.StartsWith(IconDynamicsHandle.ICONID_HEAD_DYNAFF)) {
                                    pos = i;
                                    return i;
                                }
                            }
                            if (kindCrescend) {
                                if (iconid.StartsWith(IconDynamicsHandle.ICONID_HEAD_CRESCEND)) {
                                    pos = i;
                                    return i;
                                }
                            }
                            if (kindDecrescend) {
                                if (iconid.StartsWith(IconDynamicsHandle.ICONID_HEAD_DECRESCEND)) {
                                    pos = i;
                                    return i;
                                }
                            }
                        }
                    }
                }
                return -1;
            }

            public void remove()
            {
                if (0 <= pos && pos < list.getCount()) {
                    list.removeAt(pos);
                }
            }
        }

        /// <summary>
        /// 歌手変更イベントの反復子
        /// </summary>
        protected class SingerEventIterator : Iterator<VsqEvent>
        {
            VsqEventList source;
            int lastIndex;
            int start;
            int end;

            /// <summary>
            /// イベントのリストと、反復する時間の区間を指定して初期化する。
            /// 引数 start, end はそれぞれ「以上」、「以下」を意味する。
            /// </summary>
            /// <param name="list">イベントのリスト</param>
            /// <param name="start">区間の開始位置。省略可能。省略した場合は int.MinValue と見做される</param>
            /// <param name="end">区間の終了位置。省略可能。省略した場合は int.MaxValue と見做される</param>
            public SingerEventIterator(VsqEventList list, int start = int.MinValue, int end = int.MaxValue)
            {
                source = list;
                lastIndex = -1;
                this.start = start;
                this.end = end;
            }

            public bool hasNext()
            {
                int nextIndex = findNextIndex();
                return 0 <= nextIndex && nextIndex < source.getCount();
            }

            public VsqEvent next()
            {
                int nextIndex = findNextIndex();
                if (0 <= nextIndex && nextIndex < source.getCount()) {
                    lastIndex = nextIndex;
                    return source.getElement(nextIndex);
                } else {
                    return null;
                }
            }

            public void remove()
            {
                if (0 <= lastIndex && lastIndex < source.getCount()) {
                    source.removeAt(lastIndex);
                }
            }

            private int findNextIndex()
            {
                int count = source.getCount();
                for (int i = lastIndex + 1; i < count; ++i) {
                    var item = source.getElement(i);
                    if (start <= item.Clock && item.Clock <= end && item.ID.type == VsqIDType.Singer) {
                        return i;
                    }
                }
                return -1;
            }
        }

        private class NoteEventEnumerator : IEnumerable<VsqEvent>, IEnumerator<VsqEvent>
        {
            VsqEventList m_list;
            int m_pos;

            public NoteEventEnumerator(VsqEventList list)
            {
                m_list = list;
                m_pos = -1;
            }

            public void Reset()
            {
                m_pos = -1;
            }

            public bool MoveNext()
            {
                int count = m_list.getCount();
                for (int i = m_pos + 1; i < count; i++) {
                    VsqEvent item = m_list.getElement(i);
                    if (item.ID.type == VsqIDType.Anote) {
                        m_pos = i;
                        return true;
                    }
                }
                m_pos = count;
                return false;
            }

            public VsqEvent Current { get { return m_list.getElement(m_pos); } }

            object System.Collections.IEnumerator.Current { get { return m_list.getElement(m_pos); } }

            public void Dispose() { }

            public System.Collections.Generic.IEnumerator<VsqEvent> GetEnumerator() { return this; }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return this; }
        }

        private class DynamicsEventIterator : Iterator<VsqEvent>
        {
            VsqEventList m_list;
            int m_pos;

            public DynamicsEventIterator(VsqEventList list)
            {
                m_list = list;
                m_pos = -1;
            }

            public bool hasNext()
            {
                int c = m_list.getCount();
                for (int i = m_pos + 1; i < c; i++) {
                    if (m_list.getElement(i).ID.type == VsqIDType.Aicon) {
                        return true;
                    }
                }
                return false;
            }

            public VsqEvent next()
            {
                int c = m_list.getCount();
                for (int i = m_pos + 1; i < c; i++) {
                    VsqEvent item = m_list.getElement(i);
                    if (item.ID.type == VsqIDType.Aicon) {
                        m_pos = i;
                        return item;
                    }
                }
                return null;
            }

            public void remove()
            {
                if (0 <= m_pos && m_pos < m_list.getCount()) {
                    m_list.removeAt(m_pos);
                }
            }
        }

        private class EventIterator : Iterator<VsqEvent>
        {
            private VsqEventList m_list;
            private int m_pos;

            public EventIterator(VsqEventList list)
            {
                m_list = list;
                m_pos = -1;
            }

            public bool hasNext()
            {
                if (0 <= m_pos + 1 && m_pos + 1 < m_list.getCount()) {
                    return true;
                }
                return false;
            }

            public VsqEvent next()
            {
                m_pos++;
                return m_list.getElement(m_pos);
            }

            public void remove()
            {
                if (0 <= m_pos && m_pos < m_list.getCount()) {
                    m_list.removeAt(m_pos);
                }
            }
        }

        /// <summary>
        /// 指定した位置に，指定した量の空白を挿入します
        /// </summary>
        /// <param name="clock_start">空白を挿入する位置</param>
        /// <param name="clock_amount">挿入する空白の量</param>
        public void insertBlank(int clock_start, int clock_amount)
        {
            // イベントをシフト
            for (Iterator<VsqEvent> itr = getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                if (item.ID.type == VsqIDType.Singer && item.Clock <= 0) {
                    continue;
                }
                if (clock_start <= item.Clock) {
                    item.Clock += clock_amount;
                }
            }

            // コントロールカーブをシフト
            foreach (string name in CURVES) {
                VsqBPList list = getCurve(name);
                if (list == null) {
                    continue;
                }

                // 後ろからシフトしないといけない
                int size = list.size();
                for (int i = size - 1; i >= 0; i--) {
                    int clock = list.getKeyClock(i);
                    if (clock_start <= clock) {
                        int value = list.getElementA(i);
                        list.move(clock, clock + clock_amount, value);
                    }
                }
            }
        }

        /// <summary>
        /// このトラックの指定した範囲を削除し，削除範囲以降の部分を削除開始位置までシフトします
        /// </summary>
        /// <param name="clock_start"></param>
        /// <param name="clock_end"></param>
        public void removePart(int clock_start, int clock_end)
        {
            int dclock = clock_end - clock_start;

            // 削除する範囲に歌手変更イベントが存在するかどうかを検査。
            VsqEvent t_last_singer = null;
            for (Iterator<VsqEvent> itr = getSingerEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = itr.next();
                if (clock_start <= ve.Clock && ve.Clock < clock_end) {
                    t_last_singer = ve;
                }
                if (ve.Clock == clock_end) {
                    t_last_singer = null; // 後でclock_endの位置に補うが、そこにに既に歌手変更イベントがあるとまずいので。
                }
            }
            VsqEvent last_singer = null;
            if (t_last_singer != null) {
                last_singer = (VsqEvent)t_last_singer.clone();
                last_singer.Clock = clock_end;
            }

            bool changed = true;
            // イベントの削除
            while (changed) {
                changed = false;
                int numEvents = getEventCount();
                for (int i = 0; i < numEvents; i++) {
                    VsqEvent itemi = getEvent(i);
                    if (clock_start <= itemi.Clock && itemi.Clock < clock_end) {
                        removeEvent(i);
                        changed = true;
                        break;
                    }
                }
            }

            // クロックのシフト
            if (last_singer != null) {
                addEvent(last_singer); //歌手変更イベントを補う
            }
            int num_events = getEventCount();
            for (int i = 0; i < num_events; i++) {
                VsqEvent itemi = getEvent(i);
                if (clock_end <= itemi.Clock) {
                    itemi.Clock -= dclock;
                }
            }

            for (int i = 0; i < VsqTrack.CURVES.Length; i++) {
                string curve = VsqTrack.CURVES[i];
                VsqBPList bplist = getCurve(curve);
                if (bplist == null) {
                    continue;
                }
                VsqBPList buf_bplist = (VsqBPList)bplist.clone();
                bplist.clear();
                int value_at_end = buf_bplist.getValue(clock_end);
                bool at_end_added = false;
                foreach (var key in buf_bplist.keyClockIterator()) {
                    if (key < clock_start) {
                        bplist.add(key, buf_bplist.getValue(key));
                    } else if (clock_end <= key) {
                        if (key == clock_end) {
                            at_end_added = true;
                        }
                        bplist.add(key - dclock, buf_bplist.getValue(key));
                    }
                }
                if (!at_end_added) {
                    bplist.add(clock_end - dclock, value_at_end);
                }
            }
        }

        /// <summary>
        /// 指定された種類のイベントのインデクスを順に返すイテレータを取得します．
        /// </summary>
        /// <param name="iterator_kind"></param>
        /// <returns></returns>
        public Iterator<int> indexIterator(int iterator_kind)
        {
            if (MetaText == null) {
                return new IndexIterator(new VsqEventList(), iterator_kind);
            } else {
                return new IndexIterator(MetaText.getEventList(), iterator_kind);
            }
        }

        /// <summary>
        /// このトラックの再生モードを取得します．
        /// </summary>
        /// <returns>PlayMode.PlayAfterSynthまたはPlayMode.PlayWithSynth</returns>
        public int getPlayMode()
        {
            if (MetaText == null) {
                return PlayMode.PlayWithSynth;
            }
            if (MetaText.Common == null) {
                return PlayMode.PlayWithSynth;
            }
            if (MetaText.Common.LastPlayMode != PlayMode.PlayAfterSynth && MetaText.Common.LastPlayMode != PlayMode.PlayWithSynth) {
                MetaText.Common.LastPlayMode = PlayMode.PlayWithSynth;
            }
            return MetaText.Common.LastPlayMode;
        }

        /// <summary>
        /// このトラックの再生モードを設定します．
        /// </summary>
        /// <param name="value">PlayMode.PlayAfterSynth, PlayMode.PlayWithSynth, またはPlayMode.Offのいずれかを指定します</param>
        public void setPlayMode(int value)
        {
            if (MetaText == null) return;
            if (MetaText.Common == null) {
                MetaText.Common = new VsqCommon("Miku", 128, 128, 128, DynamicsMode.Expert, value);
                return;
            }
            if (value == PlayMode.Off) {
                if (MetaText.Common.PlayMode != PlayMode.Off) {
                    MetaText.Common.LastPlayMode = MetaText.Common.PlayMode;
                }
            } else {
                MetaText.Common.LastPlayMode = value;
            }
            MetaText.Common.PlayMode = value;
        }

        /// <summary>
        /// このトラックがレンダリングされるかどうかを取得します．
        /// </summary>
        /// <returns></returns>
        public bool isTrackOn()
        {
            if (MetaText == null) return true;
            if (MetaText.Common == null) return true;
            return MetaText.Common.PlayMode != PlayMode.Off;
        }

        /// <summary>
        /// このトラックがレンダリングされるかどうかを設定します，
        /// </summary>
        /// <param name="value"></param>
        public void setTrackOn(bool value)
        {
            if (MetaText == null) return;
            if (MetaText.Common == null) {
                MetaText.Common = new VsqCommon("Miku", 128, 128, 128, DynamicsMode.Expert, value ? PlayMode.PlayWithSynth : PlayMode.Off);
            }
            if (value) {
                if (MetaText.Common.LastPlayMode != PlayMode.PlayAfterSynth &&
                     MetaText.Common.LastPlayMode != PlayMode.PlayWithSynth) {
                    MetaText.Common.LastPlayMode = PlayMode.PlayWithSynth;
                }
                MetaText.Common.PlayMode = MetaText.Common.LastPlayMode;
            } else {
                if (MetaText.Common.PlayMode == PlayMode.PlayAfterSynth ||
                     MetaText.Common.PlayMode == PlayMode.PlayWithSynth) {
                    MetaText.Common.LastPlayMode = MetaText.Common.PlayMode;
                }
                MetaText.Common.PlayMode = PlayMode.Off;
            }
        }

        /// <summary>
        /// このトラックの名前を取得します．
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            if (MetaText == null || (MetaText != null && MetaText.Common == null)) {
                return "Master Track";
            } else {
                return MetaText.Common.Name;
            }
        }

        /// <summary>
        /// このトラックの名前を設定します．
        /// </summary>
        /// <param name="value"></param>
        public void setName(string value)
        {
            if (MetaText != null) {
                if (MetaText.Common == null) {
                    MetaText.Common = new VsqCommon();
                }
                MetaText.Common.Name = value;
            }
        }

        /// <summary>
        /// XMLシリアライズ用．このトラックの名前を設定します．
        /// </summary>
        [Obsolete]
        public string Name
        {
            get
            {
                return getName();
            }
            set
            {
                setName(value);
            }
        }

        /// <summary>
        /// このトラックの，指定したゲートタイムにおけるピッチベンドを取得します．単位はCentです．
        /// </summary>
        /// <param name="clock">ピッチベンドを取得するゲートタイム</param>
        /// <returns></returns>
        public double getPitchAt(int clock)
        {
            double inv2_13 = 1.0 / 8192.0;
            int pit = MetaText.PIT.getValue(clock);
            int pbs = MetaText.PBS.getValue(clock);
            return (double)pit * (double)pbs * inv2_13 * 100.0;
        }

        /// <summary>
        /// クレッシェンド，デクレッシェンド，および強弱記号をダイナミクスカーブに反映させます．
        /// この操作によって，ダイナミクスカーブに設定されたデータは全て削除されます．
        /// </summary>
        public void reflectDynamics()
        {
            VsqBPList dyn = getCurve("dyn");
            dyn.clear();
            for (Iterator<VsqEvent> itr = getDynamicsEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                IconDynamicsHandle handle = item.ID.IconDynamicsHandle;
                if (handle == null) {
                    continue;
                }
                int clock = item.Clock;
                int length = item.ID.getLength();

                if (handle.isDynaffType()) {
                    // 強弱記号
                    dyn.add(clock, handle.getStartDyn());
                } else {
                    // クレッシェンド，デクレッシェンド
                    int start_dyn = dyn.getValue(clock);

                    // 範囲内のアイテムを削除
                    int count = dyn.size();
                    for (int i = count - 1; i >= 0; i--) {
                        int c = dyn.getKeyClock(i);
                        if (clock <= c && c <= clock + length) {
                            dyn.removeElementAt(i);
                        } else if (c < clock) {
                            break;
                        }
                    }

                    VibratoBPList bplist = handle.getDynBP();
                    if (bplist == null || (bplist != null && bplist.getCount() <= 0)) {
                        // カーブデータが無い場合
                        double a = 0.0;
                        if (length > 0) {
                            a = (handle.getEndDyn() - handle.getStartDyn()) / (double)length;
                        }
                        int last_val = start_dyn;
                        for (int i = clock; i < clock + length; i++) {
                            int val = start_dyn + (int)(a * (i - clock));
                            if (val < dyn.getMinimum()) {
                                val = dyn.getMinimum();
                            } else if (dyn.getMaximum() < val) {
                                val = dyn.getMaximum();
                            }
                            if (last_val != val) {
                                dyn.add(i, val);
                                last_val = val;
                            }
                        }
                    } else {
                        // カーブデータがある場合
                        int last_val = handle.getStartDyn();
                        int last_clock = clock;
                        int bpnum = bplist.getCount();
                        int last = start_dyn;

                        // bplistに指定されている分のデータ点を追加
                        for (int i = 0; i < bpnum; i++) {
                            VibratoBPPair point = bplist.getElement(i);
                            int pointClock = clock + (int)(length * point.X);
                            if (pointClock <= last_clock) {
                                continue;
                            }
                            int pointValue = point.Y;
                            double a = (pointValue - last_val) / (double)(pointClock - last_clock);
                            for (int j = last_clock; j <= pointClock; j++) {
                                int val = start_dyn + (int)((j - last_clock) * a);
                                if (val < dyn.getMinimum()) {
                                    val = dyn.getMinimum();
                                } else if (dyn.getMaximum() < val) {
                                    val = dyn.getMaximum();
                                }
                                if (val != last) {
                                    dyn.add(j, val);
                                    last = val;
                                }
                            }
                            last_val = point.Y;
                            last_clock = pointClock;
                        }

                        // bplistの末尾から，clock => clock + lengthまでのデータ点を追加
                        int last2 = last;
                        if (last_clock < clock + length) {
                            double a = (handle.getEndDyn() - last_val) / (double)(clock + length - last_clock);
                            for (int j = last_clock; j < clock + length; j++) {
                                int val = last2 + (int)((j - last_clock) * a);
                                if (val < dyn.getMinimum()) {
                                    val = dyn.getMinimum();
                                } else if (dyn.getMaximum() < val) {
                                    val = dyn.getMaximum();
                                }
                                if (val != last) {
                                    dyn.add(j, val);
                                    last = val;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 指定したゲートタイムにおいて、歌唱を担当している歌手のVsqEventを取得します．
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public VsqEvent getSingerEventAt(int clock)
        {
            VsqEvent last = null;
            for (Iterator<VsqEvent> itr = getSingerEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                if (clock < item.Clock) {
                    return last;
                }
                last = item;
            }
            return last;
        }

        /// <summary>
        /// このトラックに設定されているイベントを，ゲートタイム順に並べ替えます．
        /// </summary>
        public void sortEvent()
        {
            MetaText.Events.sort();
        }

        /// <summary>
        /// 歌手変更イベントを，曲の先頭から順に返すIteratorを取得します．
        /// </summary>
        /// <param name="start">区間の開始時刻</param>
        /// <param name="end">区間の終了時刻</param>
        /// <returns></returns>
        public Iterator<VsqEvent> getSingerEventIterator(int start = int.MinValue, int end = int.MaxValue)
        {
            return new SingerEventIterator(MetaText.getEventList(), start, end);
        }

        /// <summary>
        /// 音符イベントを，曲の先頭から順に返すIteratorを取得します．
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VsqEvent> getNoteEventIterator()
        {
            if (MetaText == null) {
                return new NoteEventEnumerator(new VsqEventList());
            } else {
                return new NoteEventEnumerator(MetaText.getEventList());
            }
        }

        /// <summary>
        /// クレッシェンド，デクレッシェンド，および強弱記号を表すイベントを，曲の先頭から順に返すIteratorを取得します．
        /// </summary>
        /// <returns></returns>
        public Iterator<VsqEvent> getDynamicsEventIterator()
        {
            if (MetaText == null) {
                return new DynamicsEventIterator(new VsqEventList());
            } else {
                return new DynamicsEventIterator(MetaText.getEventList());
            }
        }

        /// <summary>
        /// このトラックのメタテキストをストリームに出力します．
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="encode"></param>
        /// <param name="eos"></param>
        /// <param name="start"></param>
        public void printMetaText(ITextWriter sw, int eos, int start)
        {
            MetaText.print(sw, eos, start);
        }

        /// <summary>
        /// このトラックのメタテキストを，指定されたファイルに出力します．
        /// </summary>
        /// <param name="file"></param>
        public void printMetaText(string file, string encoding = "Shift_JIS")
        {
            TextStream tms = new TextStream();
            int count = MetaText.getEventList().getCount();
            int clLast = MetaText.getEventList().getElement(count - 1).Clock + 480;
            MetaText.print(tms, clLast, 0);
            InternalStreamWriter sw = null;
            try {
                sw = new InternalStreamWriter(file, encoding);
                tms.setPointer(-1);
                while (tms.ready()) {
                    string line = tms.readLine().ToString();
                    sw.write(line);
                    sw.newLine();
                }
            } catch (Exception ex) {
                serr.println("VsqTrack#printMetaText; ex=" + ex);
            } finally {
                if (sw != null) {
                    try {
                        sw.close();
                    } catch (Exception ex2) {
                        serr.println("VsqTrack#printMetaText; ex2=" + ex2);
                    }
                }
            }
        }

        /// <summary>
        /// このトラックのMasterを取得します．
        /// </summary>
        public VsqMaster getMaster()
        {
            if (MetaText == null) {
                return null;
            } else {
                return MetaText.master;
            }
        }

        /// <summary>
        /// このトラックのMasterを設定します．
        /// </summary>
        /// <param name="value"></param>
        public void setMaster(VsqMaster value)
        {
            if (MetaText != null) {
                MetaText.master = value;
            } else {
                serr.println("VsqTrack#setMaster; MetaText is null");
            }
        }

        /// <summary>
        /// このトラックのMixerを取得します．
        /// </summary>
        public VsqMixer getMixer()
        {
            if (MetaText == null) {
                return null;
            } else {
                return MetaText.mixer;
            }
        }

        /// <summary>
        /// このトラックのMixerを設定します．
        /// </summary>
        /// <param name="value"></param>
        public void setMixer(VsqMixer value)
        {
            if (MetaText != null) {
                MetaText.mixer = value;
            } else {
                serr.println("VsqTrack#setMixer; MetaText is null");
            }
        }

        /// <summary>
        /// Commonを取得します
        /// </summary>
        /// <returns></returns>
        public VsqCommon getCommon()
        {
            if (MetaText == null) {
                return null;
            } else {
                return MetaText.Common;
            }
        }

        /// <summary>
        /// 指定したトラックのレンダラーを変更します
        /// </summary>
        /// <param name="track"></param>
        /// <param name="new_renderer"></param>
        /// <param name="singers"></param>
        public void changeRenderer(string new_renderer, List<VsqID> singers)
        {
            VsqID default_id = null;
            int singers_size = singers.Count;
            if (singers_size <= 0) {
                default_id = new VsqID();
                default_id.type = VsqIDType.Singer;
                IconHandle singer_handle = new IconHandle();
                singer_handle.IconID = "$0701" + PortUtil.toHexString(0, 4);
                singer_handle.IDS = "Unknown";
                singer_handle.Index = 0;
                singer_handle.Language = 0;
                singer_handle.setLength(1);
                singer_handle.Original = 0;
                singer_handle.Program = 0;
                singer_handle.Caption = "";
                default_id.IconHandle = singer_handle;
            } else {
                default_id = singers[0];
            }

            for (Iterator<VsqEvent> itr = getSingerEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = itr.next();
                IconHandle singer_handle = (IconHandle)ve.ID.IconHandle;
                int program = singer_handle.Program;
                bool found = false;
                for (int i = 0; i < singers_size; i++) {
                    VsqID id = singers[i];
                    if (program == singer_handle.Program) {
                        ve.ID = (VsqID)id.clone();
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    VsqID add = (VsqID)default_id.clone();
                    add.IconHandle.Program = program;
                    ve.ID = add;
                }
            }
            MetaText.Common.Version = new_renderer;
        }

        /// <summary>
        /// このトラックが保持している，指定されたカーブのBPListを取得します
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public VsqBPList getCurve(string curve)
        {
            return MetaText.getElement(curve);
        }

        public void setCurve(string curve, VsqBPList value)
        {
            MetaText.setElement(curve, value);
        }

        public int getEventCount()
        {
            return MetaText.getEventList().getCount();
        }

        public VsqEvent getEvent(int index)
        {
            return MetaText.getEventList().getElement(index);
        }

        public IEnumerable<VsqEvent> events()
        {
            return MetaText.Events.Events;
        }

        public VsqEvent findEventFromID(int internal_id)
        {
            return MetaText.getEventList().findFromID(internal_id);
        }

        public int findEventIndexFromID(int internal_id)
        {
            return MetaText.getEventList().findIndexFromID(internal_id);
        }

        public void setEvent(int index, VsqEvent item)
        {
            MetaText.getEventList().setElement(index, item);
        }

        public int addEvent(VsqEvent item)
        {
            return MetaText.getEventList().add(item);
        }

        public void addEvent(VsqEvent item, int internal_id)
        {
            MetaText.Events.add(item, internal_id);
        }

        public Iterator<VsqEvent> getEventIterator()
        {
            return new EventIterator(MetaText.getEventList());
        }

        public void removeEvent(int index)
        {
            MetaText.getEventList().removeAt(index);
        }

        /// <summary>
        /// このインスタンスのコピーを作成します
        /// </summary>
        /// <returns></returns>
        public Object clone()
        {
            VsqTrack res = new VsqTrack();
            res.setName(getName());
            if (MetaText != null) {
                res.MetaText = (VsqMetaText)MetaText.clone();
            }
            res.Tag = Tag;
            return res;
        }

        public object Clone()
        {
            return clone();
        }

        /// <summary>
        /// Master Trackを構築
        /// </summary>
        /// <param name="tempo"></param>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        public VsqTrack(int tempo, int numerator, int denominator)
        {
            //this.Name = "Master Track";
            // metatextがnullのとき，トラック名はMaster Track
            this.MetaText = null;
        }

        /// <summary>
        /// Master Trackでないトラックを構築。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="singer"></param>
        public VsqTrack(string name, string singer)
        {
            MetaText = new VsqMetaText(name, singer);
        }

        public VsqTrack()
            : this("Voice1", "Miku")
        {
        }

        /// <summary>
        /// 歌詞の文字数を調べます
        /// </summary>
        /// <returns></returns>
        public int getLyricLength()
        {
            int counter = 0;
            for (int i = 0; i < MetaText.getEventList().getCount(); i++) {
                if (MetaText.getEventList().getElement(i).ID.type == VsqIDType.Anote) {
                    counter++;
                }
            }
            return counter;
        }

        public VsqTrack(List<MidiEvent> midi_event, string encoding)
        {
            string track_name = "";

            TextStream sw = null;
            try {
                sw = new TextStream();
                int count = midi_event.Count;
                List<int> buffer = new List<int>();
                for (int i = 0; i < count; i++) {
                    MidiEvent item = midi_event[i];
                    if (item.firstByte == 0xff && item.data.Length > 0) {
                        // meta textを抽出
                        int type = item.data[0];
                        if (type == 0x01 || type == 0x03) {
                            if (type == 0x01) {
                                int colon_count = 0;
                                for (int j = 0; j < item.data.Length - 1; j++) {
                                    int d = item.data[j + 1];
                                    if (d == 0x3a) {
                                        colon_count++;
                                        if (colon_count <= 2) {
                                            continue;
                                        }
                                    }
                                    if (colon_count < 2) {
                                        continue;
                                    }
                                    buffer.Add(d);
                                }

                                int index_0x0a = buffer.IndexOf(0x0a);
                                while (index_0x0a >= 0) {
                                    int[] cpy = new int[index_0x0a];
                                    for (int j = 0; j < index_0x0a; j++) {
                                        cpy[j] = 0xff & (int)buffer[0];
                                        buffer.RemoveAt(0);
                                    }

                                    string line = PortUtil.getDecodedString(encoding, cpy);
                                    sw.writeLine(line);
                                    buffer.RemoveAt(0);
                                    index_0x0a = buffer.IndexOf(0x0a);
                                }
                            } else {
                                for (int j = 0; j < item.data.Length - 1; j++) {
                                    buffer.Add(item.data[j + 1]);
                                }
                                int c = buffer.Count;
                                int[] d = new int[c];
                                for (int j = 0; j < c; j++) {
                                    d[j] = 0xff & buffer[j];
                                }
                                track_name = PortUtil.getDecodedString(encoding, d);
                                buffer.Clear();
                            }
                        }
                    } else {
                        continue;
                    }
                }
                // oketa ketaoさんありがとう =>
                int remain = buffer.Count;
                if (remain > 0) {
                    int[] cpy = new int[remain];
                    for (int j = 0; j < remain; j++) {
                        cpy[j] = 0xff & buffer[j];
                    }
                    string line = PortUtil.getDecodedString(encoding, cpy);
                    sw.writeLine(line);
                }
                // <=
                //sw.rewind();
                MetaText = new VsqMetaText(sw);
                setName(track_name);
            } catch (Exception ex) {
                serr.println("org.kbinani.vsq.VsqTrack#.ctor; ex=" + ex);
            } finally {
                if (sw != null) {
                    try {
                        sw.close();
                    } catch (Exception ex2) {
                        serr.println("org.kbinani.vsq.VsqTrack#.ctor; ex2=" + ex2);
                    }
                }
            }
        }
    }

}
