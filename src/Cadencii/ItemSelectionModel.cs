/*
 * ItemSelectionModel.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System.Collections.Generic;

namespace cadencii
{

    using System;
    using cadencii.java.util;
    using cadencii.vsq;

    /// <summary>
    /// アイテムの選択状態を管理するクラスです．
    /// </summary>
    public class ItemSelectionModel
    {
        /// <summary>
        /// 選択されているベジエ点のリスト
        /// </summary>
        private List<SelectedBezierPoint> mBezier = new List<SelectedBezierPoint>();
        /// <summary>
        /// 最後に選択されたベジエ点
        /// </summary>
        private SelectedBezierPoint mSelectedBezier = new SelectedBezierPoint();
        /// <summary>
        /// 選択されている拍子変更イベントのリスト
        /// </summary>
        private SortedDictionary<int, SelectedTimesigEntry> mTimesig = new SortedDictionary<int, SelectedTimesigEntry>();
        private int mSelectedTimesig = -1;
        /// <summary>
        /// 選択されているテンポ変更イベントのリスト
        /// </summary>
        private SortedDictionary<int, SelectedTempoEntry> mTempo = new SortedDictionary<int, SelectedTempoEntry>();
        private int mLastTempo = -1;
        /// <summary>
        /// 選択されているイベントのリスト
        /// </summary>
        private List<SelectedEventEntry> mEvents = new List<SelectedEventEntry>();
        private List<long> mPointIDs = new List<long>();
        /// <summary>
        /// selectedPointIDsに格納されているデータ点の，CurveType
        /// </summary>
        private CurveType mPointCurveType = CurveType.Empty;

        /// <summary>
        /// 選択状態のアイテムが変化した時発生するイベント
        /// </summary>
        public event SelectedEventChangedEventHandler SelectedEventChanged;

        #region Bezier
        /// <summary>
        /// 選択されているベジエ曲線のデータ点を順に返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectedBezierPoint> getBezierIterator()
        {
            return mBezier;
        }

        /// <summary>
        /// 最後に選択状態となったベジエ曲線のデータ点を取得します。
        /// </summary>
        /// <returns>最後に選択状態となったベジエ曲線のデータ点を返します。選択状態となっているベジエ曲線がなければnullを返します。</returns>
        public SelectedBezierPoint getLastBezier()
        {
            if (mSelectedBezier.chainID < 0 || mSelectedBezier.pointID < 0) {
                return null;
            } else {
                return mSelectedBezier;
            }
        }

        /// <summary>
        /// 指定されたベジエ曲線のデータ点を選択状態にします。
        /// </summary>
        /// <param name="selected">選択状態にするデータ点。</param>
        public void addBezier(SelectedBezierPoint selected)
        {
            mSelectedBezier = selected;
            int index = -1;
            for (int i = 0; i < mBezier.Count; i++) {
                if (mBezier[i].chainID == selected.chainID &&
                    mBezier[i].pointID == selected.pointID) {
                    index = i;
                    break;
                }
            }
            if (index >= 0) {
                mBezier[index] = selected;
            } else {
                mBezier.Add(selected);
            }
            checkSelectedItemExistence();
        }

        /// <summary>
        /// すべてのベジエ曲線のデータ点の選択状態を解除します。
        /// </summary>
        public void clearBezier()
        {
            mBezier.Clear();
            mSelectedBezier.chainID = -1;
            mSelectedBezier.pointID = -1;
            checkSelectedItemExistence();
        }
        #endregion

        #region Timesig
        /// <summary>
        /// 最後に選択状態となった拍子変更設定を取得します。
        /// </summary>
        /// <returns>最後に選択状態となった拍子変更設定を返します。選択状態となっている拍子変更設定が無ければnullを返します。</returns>
        public SelectedTimesigEntry getLastTimesig()
        {
            if (mTimesig.ContainsKey(mSelectedTimesig)) {
                return mTimesig[mSelectedTimesig];
            } else {
                return null;
            }
        }

        public int getLastTimesigBarcount()
        {
            return mSelectedTimesig;
        }

        public void addTimesig(int barcount)
        {
            clearEvent(); //ここ注意！
            clearTempo();
            mSelectedTimesig = barcount;
            if (!mTimesig.ContainsKey(barcount)) {
                foreach (var tte in AppManager.getVsqFile().TimesigTable) {
                    if (tte.BarCount == barcount) {
                        mTimesig[barcount] = new SelectedTimesigEntry(tte, (TimeSigTableEntry)tte.clone());
                        break;
                    }
                }
            }
            checkSelectedItemExistence();
        }

        public void clearTimesig()
        {
            mTimesig.Clear();
            mSelectedTimesig = -1;
            checkSelectedItemExistence();
        }

        public int getTimesigCount()
        {
            return mTimesig.Count;
        }

        public IEnumerable<ValuePair<int, SelectedTimesigEntry>> getTimesigIterator()
        {
            List<ValuePair<int, SelectedTimesigEntry>> list = new List<ValuePair<int, SelectedTimesigEntry>>();
            foreach (var clock in mTimesig.Keys) {
                list.Add(new ValuePair<int, SelectedTimesigEntry>(clock, mTimesig[clock]));
            }
            return list;
        }

        public bool isTimesigContains(int barcount)
        {
            return mTimesig.ContainsKey(barcount);
        }

        public SelectedTimesigEntry getTimesig(int barcount)
        {
            if (mTimesig.ContainsKey(barcount)) {
                return mTimesig[barcount];
            } else {
                return null;
            }
        }

        public void removeTimesig(int barcount)
        {
            if (mTimesig.ContainsKey(barcount)) {
                mTimesig.Remove(barcount);
                checkSelectedItemExistence();
            }
        }
        #endregion

        #region Tempo
        public SelectedTempoEntry getLastTempo()
        {
            if (mTempo.ContainsKey(mLastTempo)) {
                return mTempo[mLastTempo];
            } else {
                return null;
            }
        }

        public int getLastTempoClock()
        {
            return mLastTempo;
        }

        public void addTempo(int clock)
        {
            clearEvent(); //ここ注意！
            clearTimesig();
            mLastTempo = clock;
            if (!mTempo.ContainsKey(clock)) {
                foreach (var tte in AppManager.getVsqFile().TempoTable) {
                    if (tte.Clock == clock) {
                        mTempo[clock] = new SelectedTempoEntry(tte, (TempoTableEntry)tte.clone());
                        break;
                    }
                }
            }
            checkSelectedItemExistence();
        }

        public void clearTempo()
        {
            mTempo.Clear();
            mLastTempo = -1;
            checkSelectedItemExistence();
        }

        public int getTempoCount()
        {
            return mTempo.Count;
        }

        public IEnumerable<ValuePair<int, SelectedTempoEntry>> getTempoIterator()
        {
            List<ValuePair<int, SelectedTempoEntry>> list = new List<ValuePair<int, SelectedTempoEntry>>();
            foreach (var clock in mTempo.Keys) {
                list.Add(new ValuePair<int, SelectedTempoEntry>(clock, mTempo[clock]));
            }
            return list;
        }

        public bool isTempoContains(int clock)
        {
            return mTempo.ContainsKey(clock);
        }

        public SelectedTempoEntry getTempo(int clock)
        {
            if (mTempo.ContainsKey(clock)) {
                return mTempo[clock];
            } else {
                return null;
            }
        }

        public void removeTempo(int clock)
        {
            if (mTempo.ContainsKey(clock)) {
                mTempo.Remove(clock);
                checkSelectedItemExistence();
            }
        }
        #endregion

        #region Event
        public void removeEvent(int id)
        {
            removeEventCor(id, false);
            checkSelectedItemExistence();
        }

        public void removeEventSilent(int id)
        {
            removeEventCor(id, true);
            checkSelectedItemExistence();
        }

        private void removeEventCor(int id, bool silent)
        {
            int count = mEvents.Count;
            for (int i = 0; i < count; i++) {
                if (mEvents[i].original.InternalID == id) {
                    mEvents.RemoveAt(i);
                    break;
                }
            }
            if (!silent) {
#if ENABLE_PROPERTY
                AppManager.propertyPanel.updateValue(AppManager.getSelected());
#endif
            }
        }

        public void removeEventRange(int[] ids)
        {
            List<int> v_ids = new List<int>(PortUtil.convertIntArray(ids));
            List<int> index = new List<int>();
            int count = mEvents.Count;
            for (int i = 0; i < count; i++) {
                if (v_ids.Contains(mEvents[i].original.InternalID)) {
                    index.Add(i);
                    if (index.Count == ids.Length) {
                        break;
                    }
                }
            }
            count = index.Count;
            for (int i = count - 1; i >= 0; i--) {
                mEvents.RemoveAt(i);
            }
#if ENABLE_PROPERTY
            AppManager.propertyPanel.updateValue(AppManager.getSelected());
#endif
            checkSelectedItemExistence();
        }

        public void addEventAll(List<int> list)
        {
            clearTempo();
            clearTimesig();
            VsqEvent[] index = new VsqEvent[list.Count];
            int count = 0;
            int c = list.Count;
            int selected = AppManager.getSelected();
            for (Iterator<VsqEvent> itr = AppManager.getVsqFile().Track[selected].getEventIterator(); itr.hasNext(); ) {
                VsqEvent ev = itr.next();
                int find = -1;
                for (int i = 0; i < c; i++) {
                    if (list[i] == ev.InternalID) {
                        find = i;
                        break;
                    }
                }
                if (0 <= find) {
                    index[find] = ev;
                    count++;
                }
                if (count == list.Count) {
                    break;
                }
            }
            for (int i = 0; i < index.Length; i++) {
                if (!isEventContains(selected, index[i].InternalID)) {
                    mEvents.Add(new SelectedEventEntry(selected, index[i], (VsqEvent)index[i].clone()));
                }
            }
#if ENABLE_PROPERTY
            AppManager.propertyPanel.updateValue(selected);
#endif
            checkSelectedItemExistence();
        }

        public void addEvent(int id)
        {
            addEventCor(id, false);
            checkSelectedItemExistence();
        }

        public void addEventSilent(int id)
        {
            addEventCor(id, true);
            checkSelectedItemExistence();
        }

        private void addEventCor(int id, bool silent)
        {
            clearTempo();
            clearTimesig();
            int selected = AppManager.getSelected();
            for (Iterator<VsqEvent> itr = AppManager.getVsqFile().Track[selected].getEventIterator(); itr.hasNext(); ) {
                VsqEvent ev = itr.next();
                if (ev.InternalID == id) {
                    if (isEventContains(selected, id)) {
                        // すでに選択されていた場合
                        int count = mEvents.Count;
                        for (int i = 0; i < count; i++) {
                            SelectedEventEntry item = mEvents[i];
                            if (item.original.InternalID == id) {
                                mEvents.RemoveAt(i);
                                break;
                            }
                        }
                    }

                    mEvents.Add(new SelectedEventEntry(selected, ev, (VsqEvent)ev.clone()));
                    if (!silent) {
                        invokeSelectedEventChangedEvent(false);
                    }
                    break;
                }
            }
            if (!silent) {
#if ENABLE_PROPERTY
                AppManager.propertyPanel.updateValue(selected);
#endif
            }
        }

        public void clearEvent()
        {
            mEvents.Clear();
#if ENABLE_PROPERTY
            AppManager.propertyPanel.updateValue(AppManager.getSelected());
#endif
            checkSelectedItemExistence();
        }

        public bool isEventContains(int track, int id)
        {
            int count = mEvents.Count;
            for (int i = 0; i < count; i++) {
                SelectedEventEntry item = mEvents[i];
                if (item.original.InternalID == id && item.track == track) {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<SelectedEventEntry> getEventIterator()
        {
            return mEvents;
        }

        public SelectedEventEntry getLastEvent()
        {
            if (mEvents.Count <= 0) {
                return null;
            } else {
                return mEvents[mEvents.Count - 1];
            }
        }

        public int getEventCount()
        {
            return mEvents.Count;
        }
        #endregion

        #region Point
        public void clearPoint()
        {
            mPointIDs.Clear();
            mPointCurveType = CurveType.Empty;
            checkSelectedItemExistence();
        }

        public void addPoint(CurveType curve, long id)
        {
            addPointAll(curve, new long[] { id });
            checkSelectedItemExistence();
        }

        public void addPointAll(CurveType curve, long[] ids)
        {
            if (!curve.equals(mPointCurveType)) {
                mPointIDs.Clear();
                mPointCurveType = curve;
            }
            for (int i = 0; i < ids.Length; i++) {
                if (!mPointIDs.Contains(ids[i])) {
                    mPointIDs.Add(ids[i]);
                }
            }
            checkSelectedItemExistence();
        }

        public bool isPointContains(long id)
        {
            return mPointIDs.Contains(id);
        }

        public CurveType getPointCurveType()
        {
            return mPointCurveType;
        }

        public IEnumerable<long> getPointIDIterator()
        {
            return mPointIDs;
        }

        public int getPointIDCount()
        {
            return mPointIDs.Count;
        }

        public void removePoint(long id)
        {
            mPointIDs.Remove(id);
            checkSelectedItemExistence();
        }
        #endregion

        /// <summary>
        /// 選択中のアイテムが編集された場合、編集にあわせてオブジェクトを更新する。
        /// </summary>
        public void updateSelectedEventInstance()
        {
            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track[selected];

            for (int i = 0; i < mEvents.Count; i++) {
                SelectedEventEntry item = mEvents[i];
                VsqEvent ev = null;
                if (item.track == selected) {
                    int internal_id = item.original.InternalID;
                    ev = vsq_track.findEventFromID(internal_id);
                }
                if (ev != null) {
                    mEvents[i] = new SelectedEventEntry(selected, ev, (VsqEvent)ev.clone());
                } else {
                    mEvents.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// 現在選択されたアイテムが存在するかどうかを調べ，必要であればSelectedEventChangedイベントを発生させます
        /// </summary>
        private void checkSelectedItemExistence()
        {
            bool ret = mBezier.Count == 0 &&
                       mEvents.Count == 0 &&
                       mTempo.Count == 0 &&
                       mTimesig.Count == 0 &&
                       mPointIDs.Count == 0;
            invokeSelectedEventChangedEvent(ret);
        }

        /// <summary>
        /// SelectedEventChangedEventを発生させます．
        /// </summary>
        /// <param name="ret"></param>
        private void invokeSelectedEventChangedEvent(bool ret)
        {
            try {
                if (SelectedEventChanged != null) {
                    SelectedEventChanged.Invoke(typeof(ItemSelectionModel), ret);
                }
            } catch (Exception ex) {
                serr.println("ItemSelectionModel#checkSelectedItemExistence; ex=" + ex);
                Logger.write(typeof(ItemSelectionModel) + ".checkSelectedItemExistence; ex=" + ex + "\n");
            }
        }

    }

}
