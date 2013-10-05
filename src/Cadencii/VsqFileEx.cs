/*
 * VsqFileEx.cs
 * Copyright © 2008-2011 kbinani
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
using System;
using System.IO;
using System.Collections.Generic;
using cadencii.vsq;
using cadencii;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.xml;

namespace cadencii
{

    [Serializable]
    public class VsqFileEx : VsqFile, ICloneable, ICommandRunnable
    {
        private static XmlSerializer mVsqSerializer;

        public const string TAG_VSQEVENT_AQUESTONE_RELEASE = "org.kbinani.cadencii.AquesToneRelease";
        public const string TAG_VSQTRACK_RENDERER_KIND = "org.kbinani.cadencii.RendererKind";
        /// <summary>
        /// トラックをUTAUモードで合成するとき，何番目の互換合成器で合成するかどうかを指定する
        /// </summary>
        public const string TAG_VSQTRACK_RESAMPLER_USED = "org.kbinani.cadencii.ResamplerUsed";

        public const string RENDERER_DSB2 = "DSB2";
        public const string RENDERER_DSB3 = "DSB3";
        public const string RENDERER_UTU0 = "UTU0";
        public const string RENDERER_STR0 = "STR0";
        public const string RENDERER_AQT0 = "AQT0";
        public const string RENDERER_AQT1 = "AQT1";
        /// <summary>
        /// EmtryRenderingRunnerが使用される
        /// </summary>
        public const string RENDERER_NULL = "NUL0";

        public AttachedCurve AttachedCurves;
        public List<BgmFile> BgmFiles = new List<BgmFile>();
        /// <summary>
        /// キャッシュ用ディレクトリのパス
        /// </summary>
        public string cacheDir = "";
        [System.Xml.Serialization.XmlIgnore]
        public EditorStatus editorStatus = new EditorStatus();
        /// <summary>
        /// シーケンスの設定
        /// <version>3.3+</version>
        /// </summary>
        public SequenceConfig config = new SequenceConfig();

        static VsqFileEx()
        {
            mVsqSerializer = new XmlSerializer(typeof(VsqFileEx));
        }

        /// <summary>
        /// 指定したトラックに対して使用する，UTAU互換合成器の番号を取得します
        /// </summary>
        /// <param name="vsq_track"></param>
        /// <returns></returns>
        public static int getTrackResamplerUsed(VsqTrack vsq_track)
        {
            string str_indx = getTagCor(vsq_track.Tag, TAG_VSQTRACK_RESAMPLER_USED);
            int ret = 0;
            try {
                ret = int.Parse(str_indx);
            } catch (Exception ex) {
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// 指定したトラックに対して使用するUTAU互換合成器の番号を設定します
        /// </summary>
        /// <param name="vsq_track"></param>
        /// <param name="index"></param>
        public static void setTrackResamplerUsed(VsqTrack vsq_track, int index)
        {
            vsq_track.Tag = setTagCor(vsq_track.Tag, TAG_VSQTRACK_RESAMPLER_USED, index + "");
        }

        /// <summary>
        /// 指定したトラックの音声合成器の種類を取得します
        /// </summary>
        /// <param name="vsq_track"></param>
        /// <returns></returns>
        public static RendererKind getTrackRendererKind(VsqTrack vsq_track)
        {
            string str_kind = getTagCor(vsq_track.Tag, TAG_VSQTRACK_RENDERER_KIND);
            if (str_kind != null && !str_kind.Equals("")) {
                RendererKind[] values = (RendererKind[])Enum.GetValues(typeof(RendererKind));
                foreach (RendererKind kind in values) {
                    if (str_kind.Equals(kind + "")) {
                        return kind;
                    }
                }
            }

            // タグからの判定ができないので、VsqTrackのVsqCommonから判定を試みる。
            VsqCommon vsq_common = vsq_track.getCommon();
            if (vsq_common == null) {
                // お手上げである。
                return RendererKind.VOCALOID2;
            }
            string version = vsq_common.Version;
            if (version == null) {
                // お手上げである。その２
                return RendererKind.VOCALOID2;
            }

            if (version.StartsWith(RENDERER_AQT0)) {
                return RendererKind.AQUES_TONE;
            } else if (version.StartsWith(RENDERER_AQT1)) {
                return RendererKind.AQUES_TONE2;
            } else if (version.StartsWith(RENDERER_DSB3)) {
                return RendererKind.VOCALOID2;
            } else if (version.StartsWith(RENDERER_STR0)) {
                return RendererKind.VCNT;
            } else if (version.StartsWith(RENDERER_UTU0)) {
                return RendererKind.UTAU;
            } else if (version.StartsWith(RENDERER_NULL)) {
                return RendererKind.NULL;
            } else {
                return RendererKind.VOCALOID1;
            }
        }

        public static void setTrackRendererKind(VsqTrack vsq_track, RendererKind renderer_kind)
        {
            vsq_track.Tag = setTagCor(vsq_track.Tag, TAG_VSQTRACK_RENDERER_KIND, renderer_kind + "");
            VsqCommon vsq_common = vsq_track.getCommon();
            if (vsq_common != null) {
                if (renderer_kind == RendererKind.AQUES_TONE) {
                    vsq_common.Version = RENDERER_AQT0;
                } else if (renderer_kind == RendererKind.AQUES_TONE2) {
                    vsq_common.Version = RENDERER_AQT1;
                } else if (renderer_kind == RendererKind.VCNT) {
                    vsq_common.Version = RENDERER_STR0;
                } else if (renderer_kind == RendererKind.UTAU) {
                    vsq_common.Version = RENDERER_UTU0;
                } else if (renderer_kind == RendererKind.VOCALOID1) {
                    vsq_common.Version = RENDERER_DSB2;
                } else if (renderer_kind == RendererKind.VOCALOID2) {
                    vsq_common.Version = RENDERER_DSB3;
                } else if (renderer_kind == RendererKind.NULL) {
                    vsq_common.Version = RENDERER_NULL;
                }
            }
        }

        private static string getTagCor(string tag, string tag_name)
        {
            if (tag_name == null) return "";
            if (tag_name.Equals("")) return "";
            if (tag == null) return "";
            if (tag.Equals("")) return "";
            string[] spl = PortUtil.splitString(tag, ';');
            foreach (string s in spl) {
                string[] spl2 = PortUtil.splitString(s, ':');
                if (spl2.Length == 2) {
                    if (tag_name.Equals(spl2[0])) {
                        return spl2[1];
                    }
                }
            }
            return "";
        }

        private static string setTagCor(string old_tag, string name, string value)
        {
            if (name == null) return old_tag;
            if (name.Equals("")) return old_tag;
            string v = value.Replace(":", "").Replace(";", "");
            if (old_tag == null) {
                return name + ":" + value;
            } else {
                string newtag = "";
                string[] spl = PortUtil.splitString(old_tag, ';');
                bool is_first = true;
                bool added = false;
                foreach (string s in spl) {
                    string[] spl2 = PortUtil.splitString(s, ':');
                    if (spl2.Length == 2) {
                        string add = "";
                        if (name.Equals(spl2[0])) {
                            add = name + ":" + v;
                            added = true;
                        } else {
                            add = spl2[0] + ":" + spl2[1];
                        }
                        newtag += (is_first ? "" : ";") + add;
                        is_first = false;
                    }
                }
                if (is_first) {
                    newtag = name + ":" + v;
                } else if (!added) {
                    newtag += ";" + name + ":" + v;
                }
                return newtag;
            }
        }

        public static string getEventTag(VsqEvent item, string name)
        {
            return getTagCor(item.Tag, name);
        }

        public static void setEventTag(VsqEvent item, string name, string value)
        {
            item.Tag = setTagCor(item.Tag, name, value);
        }

        /// <summary>
        /// 指定した位置に，指定した量の空白を挿入します
        /// </summary>
        /// <param name="clock_start">空白を挿入する位置</param>
        /// <param name="clock_amount">挿入する空白の量</param>
        public void insertBlank(int clock_start, int clock_amount)
        {
            base.insertBlank(clock_start, clock_amount);
            List<BezierCurves> curves = AttachedCurves.getCurves();
            int size = curves.Count;
            for (int i = 0; i < size; i++) {
                BezierCurves bcs = curves[i];
                bcs.insertBlank(clock_start, clock_amount);
            }
        }

        /// <summary>
        /// 指定した位置に，指定した量の空白を挿入します
        /// </summary>
        /// <param name="track">挿入を行う対象のトラック</param>
        /// <param name="clock_start">空白を挿入する位置</param>
        /// <param name="clock_amount">挿入する空白の量</param>
        public void insertBlank(int track, int clock_start, int clock_amount)
        {
            VsqTrack vsq_track = Track[track];
            vsq_track.insertBlank(clock_start, clock_amount);
            BezierCurves bcs = AttachedCurves.get(track - 1);
            bcs.insertBlank(clock_start, clock_amount);
        }

        /// <summary>
        /// VSQファイルの指定されたクロック範囲のイベント等を削除します
        /// </summary>
        /// <param name="clock_start">削除を行う範囲の開始クロック</param>
        /// <param name="clock_end">削除を行う範囲の終了クロック</param>
        public new void removePart(int clock_start, int clock_end)
        {
            base.removePart(clock_start, clock_end);
            List<BezierCurves> curves = AttachedCurves.getCurves();
            int size = curves.Count;
            for (int i = 0; i < size; i++) {
                BezierCurves bcs = curves[i];
                bcs.removePart(clock_start, clock_end);
            }
        }

        /// <summary>
        /// 指定したトラックの，指定した範囲のイベント等を削除します
        /// </summary>
        /// <param name="track">削除を行う対象のトラック</param>
        /// <param name="clock_start">削除を行う範囲の開始クロック</param>
        /// <param name="clock_end">削除を行う範囲の終了クロック</param>
        public void removePart(int track, int clock_start, int clock_end)
        {
            VsqTrack vsq_track = Track[track];
            vsq_track.removePart(clock_start, clock_end);
            BezierCurves bcs = AttachedCurves.get(track - 1);
            bcs.removePart(clock_start, clock_end);
        }

        /// <summary>
        /// MasterMute, トラックのMute指定、トラックのSolo指定、トラックのPlayModeを考慮し、このVSQシーケンスの指定したトラックがミュートされた状態かどうかを判定します。
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool getActualMuted(int track)
        {
            if (track < 1 || Track.Count <= track) return true;
            if (getMasterMute()) return true;
            if (getMute(track)) return true;
            if (!Track[track].isTrackOn()) return true;

            bool soloSpecificationExists = false;
            for (int i = 1; i < Track.Count; i++) {
                if (getSolo(i)) {
                    soloSpecificationExists = true;
                    break;
                }
            }
            if (soloSpecificationExists) {
                if (getSolo(track)) {
                    return getMute(track);
                } else {
                    return true;
                }
            } else {
                return getMute(track);
            }
        }

        /// <summary>
        /// このVSQシーケンスのマスタートラックをミュートするかどうかを取得します。
        /// </summary>
        /// <returns></returns>
        public bool getMasterMute()
        {
            if (Mixer == null) return false;
            return Mixer.MasterMute == 1;
        }

        /// <summary>
        /// このVSQシーケンスのマスタートラックをミュートするかどうかを設定します。
        /// </summary>
        public void setMasterMute(bool value)
        {
            if (Mixer == null) return;
            Mixer.MasterMute = value ? 1 : 0;
        }

        /// <summary>
        /// このVSQシーケンスの指定したトラックをミュートするかどうかを取得します。
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool getMute(int track)
        {
            if (Mixer == null) return false;
            if (Mixer.Slave == null) return false;
            if (track < 0) return false;
            if (track == 0) {
                return Mixer.MasterMute == 1;
            } else if (track - 1 < Mixer.Slave.Count) {
                return Mixer.Slave[track - 1].Mute == 1;
            } else {
                return false;
            }
        }

        /// <summary>
        /// このVSQシーケンスの指定したトラックをミュートするかどうかを設定します。
        /// </summary>
        /// <param name="track"></param>
        /// <param name="value"></param>
        public void setMute(int track, bool value)
        {
            if (Mixer == null) return;
            if (Mixer.Slave == null) return;
            if (track < 0) {
                return;
            } else if (track == 0) {
                Mixer.MasterMute = value ? 1 : 0;
            } else if (track - 1 < Mixer.Slave.Count) {
                Mixer.Slave[track - 1].Mute = value ? 1 : 0;
            }
        }

        /// <summary>
        /// このVSQシーケンスの指定したトラックをソロモードとするかどうかを取得します。
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool getSolo(int track)
        {
            if (Mixer == null) return false;
            if (Mixer.Slave == null) return false;
            if (track < 0) return false;
            if (track == 0) {
                return false;
            } else if (track - 1 < Mixer.Slave.Count) {
                return Mixer.Slave[track - 1].Solo == 1;
            } else {
                return false;
            }
        }

        /// <summary>
        /// このVSQシーケンスの指定したトラックをソロモードとするかどうかを設定します。
        /// </summary>
        /// <param name="track"></param>
        /// <param name="value"></param>
        public void setSolo(int track, bool value)
        {
            if (Mixer == null) return;
            if (Mixer.Slave == null) return;
            if (track < 0) {
                return;
            } else if (track == 0) {
                return;
            } else if (track - 1 < Mixer.Slave.Count) {
                Mixer.Slave[track - 1].Solo = value ? 1 : 0;
                if (value) {
                    for (int i = 0; i < Mixer.Slave.Count; i++) {
                        if (i + 1 != track && Mixer.Slave[i].Solo == 1) {
                            Mixer.Slave[i].Solo = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// VsqEvent, VsqBPList, BezierCurvesの全てのクロックを、tempoに格納されているテンポテーブルに
        /// 合致するようにシフトします
        /// </summary>
        /// <param name="work"></param>
        /// <param name="tempo"></param>
        public override void adjustClockToMatchWith(TempoVector tempo)
        {
            base.adjustClockToMatchWith(tempo);
            double premeasure_sec_target = getSecFromClock(getPreMeasureClocks());
            double premeasure_sec_tempo = premeasure_sec_target;
#if DEBUG
            sout.println("FormMain#ShiftClockToMatchWith; premeasure_sec_target=" + premeasure_sec_target + "; premeasre_sec_tempo=" + premeasure_sec_tempo);
#endif

            // テンポをリプレースする場合。
            // まずクロック値を、リプレース後のモノに置き換え
            for (int track = 1; track < this.Track.Count; track++) {
                // ベジエカーブをシフト
                for (int i = 0; i < Utility.CURVE_USAGE.Length; i++) {
                    CurveType ct = Utility.CURVE_USAGE[i];
                    List<BezierChain> list = this.AttachedCurves.get(track - 1).get(ct);
                    if (list == null) {
                        continue;
                    }
                    foreach (var chain in list) {
                        foreach (var point in chain.points) {
                            PointD bse = new PointD(tempo.getClockFromSec(this.getSecFromClock(point.getBase().getX()) - premeasure_sec_target + premeasure_sec_tempo),
                                                     point.getBase().getY());
                            double rx = point.getBase().getX() + point.controlRight.getX();
                            double new_rx = tempo.getClockFromSec(this.getSecFromClock(rx) - premeasure_sec_target + premeasure_sec_tempo);
                            PointD ctrl_r = new PointD(new_rx - bse.getX(), point.controlRight.getY());

                            double lx = point.getBase().getX() + point.controlLeft.getX();
                            double new_lx = tempo.getClockFromSec(this.getSecFromClock(lx) - premeasure_sec_target + premeasure_sec_tempo);
                            PointD ctrl_l = new PointD(new_lx - bse.getX(), point.controlLeft.getY());
                            point.setBase(bse);
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
        public static void shift(VsqFileEx vsq, double sec, int first_tempo)
        {
            bool first = true; // 負になった最初のアイテムかどうか

            // 最初にテンポをずらす．
            // 古いのから情報をコピー
            VsqFile tempo = new VsqFile("Miku", vsq.getPreMeasure(), 4, 4, 500000);
            tempo.TempoTable.Clear();
            foreach (var item in vsq.TempoTable) {
                tempo.TempoTable.Add(item);
            }
            tempo.updateTempoInfo();
            int tempo_count = tempo.TempoTable.Count;
            if (sec < 0.0) {
                first = true;
                for (int i = tempo_count - 1; i >= 0; i--) {
                    TempoTableEntry item = tempo.TempoTable[i];
                    if (item.Time + sec <= 0.0) {
                        if (first) {
                            first_tempo = item.Tempo;
                            first = false;
                        } else {
                            break;
                        }
                    }
                }
            }
            vsq.TempoTable.Clear();
            vsq.TempoTable.Add(new TempoTableEntry(0, first_tempo, 0.0));
            for (int i = 0; i < tempo_count; i++) {
                TempoTableEntry item = tempo.TempoTable[i];
                double t = item.Time + sec;
                int new_clock = (int)vsq.getClockFromSec(t);
                double new_time = vsq.getSecFromClock(new_clock);
                vsq.TempoTable.Add(new TempoTableEntry(new_clock, item.Tempo, new_time));
            }
            vsq.updateTempoInfo();

            int tracks = vsq.Track.Count;
            int pre_measure_clocks = vsq.getPreMeasureClocks();
            for (int i = 1; i < tracks; i++) {
                VsqTrack track = vsq.Track[i];
                List<int> remove_required_event = new List<int>(); // 削除が要求されたイベントのインデクス

                // 歌手変更・音符イベントをシフト
                // 時刻が負になる場合は，後で考える
                int events = track.getEventCount();
                first = true;
                for (int k = events - 1; k >= 0; k--) {
                    VsqEvent item = track.getEvent(k);
                    double t = vsq.getSecFromClock(item.Clock) + sec;
                    int clock = (int)vsq.getClockFromSec(t);
                    if (item.ID.type == VsqIDType.Anote) {
                        // 音符の長さ
                        double t_end = vsq.getSecFromClock(item.Clock + item.ID.getLength()) + sec;
                        int clock_end = (int)vsq.getClockFromSec(t_end);
                        int length = clock_end - clock;

                        if (clock < pre_measure_clocks) {
                            if (pre_measure_clocks < clock_end) {
                                // 音符の開始位置がプリメジャーよりも早く，音符の開始位置がプリメジャーより後の場合
                                clock = pre_measure_clocks;
                                length = clock_end - pre_measure_clocks;
                                // ビブラート
                                if (item.ID.VibratoHandle != null) {
                                    double vibrato_percent = item.ID.VibratoHandle.getLength() / (double)item.ID.getLength() * 100.0;
                                    double t_clock = vsq.getSecFromClock(clock); // 音符の開始時刻
                                    double t_vibrato = t_end - (t_end - t_clock) * vibrato_percent / 100.0; // ビブラートの開始時刻
                                    int clock_vibrato_start = (int)vsq.getClockFromSec(t_vibrato);
                                    item.ID.VibratoHandle.setLength(clock_end - clock_vibrato_start);
                                    item.ID.VibratoDelay = clock_vibrato_start - clock;
                                }
                                item.Clock = clock;
                                item.ID.setLength(length);
                            } else {
                                // 範囲外なので削除
                                remove_required_event.Add(k);
                            }
                        } else {
                            // ビブラート
                            if (item.ID.VibratoHandle != null) {
                                double t_vibrato_start = vsq.getSecFromClock(item.Clock + item.ID.getLength() - item.ID.VibratoHandle.getLength()) + sec;
                                int clock_vibrato_start = (int)vsq.getClockFromSec(t_vibrato_start);
                                item.ID.VibratoHandle.setLength(clock_vibrato_start - clock);
                                item.ID.VibratoDelay = clock_vibrato_start - clock;
                            }
                            item.Clock = clock;
                            item.ID.setLength(length);
                        }
                    } else if (item.ID.type == VsqIDType.Singer) {
                        if (item.Clock <= 0) {
                            if (sec >= 0.0) {
                                clock = 0;
                                item.Clock = clock;
                            } else {
                                if (first) {
                                    clock = 0;
                                    first = false;
                                    item.Clock = clock;
                                } else {
                                    remove_required_event.Add(k);
                                }
                            }
                        } else {
                            if (clock < 0) {
                                if (first) {
                                    clock = 0;
                                    first = false;
                                } else {
                                    remove_required_event.Add(k);
                                }
                            }
                            item.Clock = clock;
                        }
                    }
                }
                // 削除が要求されたものを削除
                remove_required_event.Sort();
                int count = remove_required_event.Count;
                for (int j = count - 1; j >= 0; j--) {
                    int index = remove_required_event[j];
                    track.removeEvent(index);
                }

                // コントロールカーブをシフト
                for (int k = 0; k < Utility.CURVE_USAGE.Length; k++) {
                    CurveType ct = Utility.CURVE_USAGE[k];
                    VsqBPList item = track.getCurve(ct.getName());
                    if (item == null) {
                        continue;
                    }
                    VsqBPList repl = new VsqBPList(item.getName(), item.getDefault(), item.getMinimum(), item.getMaximum());
                    int c = item.size();
                    first = true;
                    for (int j = c - 1; j >= 0; j--) {
                        int clock = item.getKeyClock(j);
                        int value = item.getElement(j);
                        double t = vsq.getSecFromClock(clock) + sec;
                        int clock_new = (int)vsq.getClockFromSec(t);
                        if (clock_new < pre_measure_clocks) {
                            if (first) {
                                clock_new = pre_measure_clocks;
                                first = false;
                            } else {
                                break;
                            }
                        }
                        repl.add(clock_new, value);
                    }
                    track.setCurve(ct.getName(), repl);
                }

                // ベジエカーブをシフト
                for (int k = 0; k < Utility.CURVE_USAGE.Length; k++) {
                    CurveType ct = Utility.CURVE_USAGE[k];
                    List<BezierChain> list = vsq.AttachedCurves.get(i - 1).get(ct);
                    if (list == null) {
                        continue;
                    }
                    remove_required_event.Clear(); //削除するBezierChainのID
                    int list_count = list.Count;
                    for (int j = 0; j < list_count; j++) {
                        BezierChain chain = list[j];
                        foreach (var point in chain.points) {
                            PointD bse = new PointD(vsq.getClockFromSec(vsq.getSecFromClock(point.getBase().getX()) + sec),
                                                     point.getBase().getY());
                            double rx = point.getBase().getX() + point.controlRight.getX();
                            double new_rx = vsq.getClockFromSec(vsq.getSecFromClock(rx) + sec);
                            PointD ctrl_r = new PointD(new_rx - bse.getX(), point.controlRight.getY());

                            double lx = point.getBase().getX() + point.controlLeft.getX();
                            double new_lx = vsq.getClockFromSec(vsq.getSecFromClock(lx) + sec);
                            PointD ctrl_l = new PointD(new_lx - bse.getX(), point.controlLeft.getY());
                            point.setBase(bse);
                            point.controlLeft = ctrl_l;
                            point.controlRight = ctrl_r;
                        }
                        double start = chain.getStart();
                        double end = chain.getEnd();
                        if (start < pre_measure_clocks) {
                            if (pre_measure_clocks < end) {
                                // プリメジャーのところでカットし，既存のものと置き換える
                                BezierChain new_chain = null;
                                try {
                                    new_chain = chain.extractPartialBezier(pre_measure_clocks, end);
                                    new_chain.id = chain.id;
                                    list[j] = new_chain;
                                } catch (Exception ex) {
                                    serr.println("VsqFileEx#shift; ex=" + ex);
                                    Logger.write(typeof(VsqFileEx) + ".shift; ex=" + ex + "\n");
                                }
                            } else {
                                remove_required_event.Add(chain.id);
                            }
                        }
                    }

                    // 削除が要求されたベジエカーブを削除
                    count = remove_required_event.Count;
                    for (int j = 0; j < count; j++) {
                        int id = remove_required_event[j];
                        list_count = list.Count;
                        for (int m = 0; m < list_count; m++) {
                            if (id == list[m].id) {
                                list.RemoveAt(m);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public new Object Clone()
        {
            return clone();
        }

        public new Object clone()
        {
            VsqFileEx ret = new VsqFileEx("Miku", 1, 4, 4, 500000);
            ret.Track = new List<VsqTrack>();
            int c = Track.Count;
            for (int i = 0; i < c; i++) {
                ret.Track.Add((VsqTrack)Track[i].clone());
            }
            ret.TempoTable = new TempoVector();
            c = TempoTable.Count;
            for (int i = 0; i < c; i++) {
                ret.TempoTable.Add((TempoTableEntry)TempoTable[i].clone());
            }
            ret.TimesigTable = new TimesigVector();// Vector<TimeSigTableEntry>();
            c = TimesigTable.Count;
            for (int i = 0; i < c; i++) {
                ret.TimesigTable.Add((TimeSigTableEntry)TimesigTable[i].clone());
            }
            ret.TotalClocks = TotalClocks;
            ret.Master = (VsqMaster)Master.clone();
            ret.Mixer = (VsqMixer)Mixer.clone();
            ret.AttachedCurves = (AttachedCurve)AttachedCurves.clone();
            c = BgmFiles.Count;
            for (int i = 0; i < c; i++) {
                ret.BgmFiles.Add((BgmFile)BgmFiles[i].clone());
            }
            ret.cacheDir = cacheDir;
            ret.config = (SequenceConfig)this.config.clone();
            return ret;
        }

        /// <summary>
        /// BGMリストの内容を更新するコマンドを発行します
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static CadenciiCommand generateCommandBgmUpdate(List<BgmFile> list)
        {
            CadenciiCommand command = new CadenciiCommand();
            command.type = CadenciiCommandType.BGM_UPDATE;
            command.args = new Object[1];
            List<BgmFile> copy = new List<BgmFile>();
            int count = list.Count;
            for (int i = 0; i < count; i++) {
                copy.Add((BgmFile)list[i].clone());
            }
            command.args[0] = copy;
            return command;
        }

        /// <summary>
        /// トラックを削除するコマンドを発行します。VstRendererを取り扱う関係上、VsqCommandを使ってはならない。
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static CadenciiCommand generateCommandDeleteTrack(int track)
        {
            CadenciiCommand command = new CadenciiCommand();
            command.type = CadenciiCommandType.TRACK_DELETE;
            command.args = new Object[1];
            command.args[0] = track;
            return command;
        }

        public static CadenciiCommand generateCommandTrackReplace(int track, VsqTrack item, BezierCurves attached_curve)
        {
            CadenciiCommand command = new CadenciiCommand();
            command.type = CadenciiCommandType.TRACK_REPLACE;
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
        public static CadenciiCommand generateCommandAddTrack(VsqTrack track, VsqMixerEntry mixer, int position, BezierCurves attached_curve)
        {
            CadenciiCommand command = new CadenciiCommand();
            command.type = CadenciiCommandType.TRACK_ADD;
            command.args = new Object[4];
            command.args[0] = track.clone();
            command.args[1] = mixer;
            command.args[2] = position;
            command.args[3] = attached_curve.clone();
            return command;
        }

        public static CadenciiCommand generateCommandAddBezierChain(int track, CurveType curve_type, int chain_id, int clock_resolution, BezierChain chain)
        {
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

        public static CadenciiCommand generateCommandChangeSequenceConfig(int sample_rate, int channels, bool output_master, int pre_measure)
        {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.CHANGE_SEQUENCE_CONFIG;
            ret.args = new Object[] { sample_rate, channels, output_master, pre_measure };
            return ret;
        }

        public static CadenciiCommand generateCommandDeleteBezierChain(int track, CurveType curve_type, int chain_id, int clock_resolution)
        {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.BEZIER_CHAIN_DELETE;
            ret.args = new Object[4];
            ret.args[0] = track;
            ret.args[1] = curve_type;
            ret.args[2] = chain_id;
            ret.args[3] = clock_resolution;
            return ret;
        }

        public static CadenciiCommand generateCommandReplaceBezierChain(int track, CurveType curve_type, int chain_id, BezierChain chain, int clock_resolution)
        {
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

        public static CadenciiCommand generateCommandReplace(VsqFileEx vsq)
        {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.REPLACE;
            ret.args = new Object[1];
            ret.args[0] = (VsqFileEx)vsq.clone();
            return ret;
        }

        public static CadenciiCommand generateCommandReplaceAttachedCurveRange(int track, SortedDictionary<CurveType, List<BezierChain>> attached_curves)
        {
            CadenciiCommand ret = new CadenciiCommand();
            ret.type = CadenciiCommandType.ATTACHED_CURVE_REPLACE_RANGE;
            ret.args = new Object[2];
            ret.args[0] = track;
            SortedDictionary<CurveType, List<BezierChain>> copy = new SortedDictionary<CurveType, List<BezierChain>>();
            foreach (var ct in attached_curves.Keys) {
                List<BezierChain> list = attached_curves[ct];
                List<BezierChain> copy_list = new List<BezierChain>();
                foreach (var chain in list) {
                    copy_list.Add((BezierChain)chain.clone());
                }
                copy[ct] = copy_list;
            }
            ret.args[1] = copy;
            return ret;
        }

        public ICommand executeCommand(ICommand com)
        {
#if DEBUG
            AppManager.debugWriteLine("VsqFileEx.Execute");
#endif
            CadenciiCommand command = (CadenciiCommand)com;
#if DEBUG
            AppManager.debugWriteLine("VsqFileEx.Execute; command.Type=" + command.type);
#endif
            CadenciiCommand ret = null;
            if (command.type == CadenciiCommandType.VSQ_COMMAND) {
                ret = new CadenciiCommand();
                ret.type = CadenciiCommandType.VSQ_COMMAND;
                ret.vsqCommand = base.executeCommand(command.vsqCommand);

                // 再レンダリングが必要になったかどうかを判定
                VsqCommandType type = command.vsqCommand.Type;
                if (type == VsqCommandType.CHANGE_PRE_MEASURE ||
                     type == VsqCommandType.REPLACE ||
                     type == VsqCommandType.UPDATE_TEMPO ||
                     type == VsqCommandType.UPDATE_TEMPO_RANGE) {
                    int count = Track.Count;
                    for (int i = 0; i < count - 1; i++) {
                        editorStatus.renderRequired[i] = true;
                    }
                } else if (type == VsqCommandType.EVENT_ADD ||
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
                            type == VsqCommandType.TRACK_REPLACE) {
                    int track = (int)command.vsqCommand.Args[0];
                    editorStatus.renderRequired[track - 1] = true;
                } else if (type == VsqCommandType.TRACK_ADD) {
                    int position = (int)command.vsqCommand.Args[2];
                    for (int i = 15; i >= position; i--) {
                        editorStatus.renderRequired[i] = editorStatus.renderRequired[i - 1];
                    }
                    editorStatus.renderRequired[position - 1] = true;
                } else if (type == VsqCommandType.TRACK_DELETE) {
                    int track = (int)command.vsqCommand.Args[0];
                    for (int i = track - 1; i < 15; i++) {
                        editorStatus.renderRequired[i] = editorStatus.renderRequired[i + 1];
                    }
                    editorStatus.renderRequired[15] = false;
                }
            } else {
                if (command.type == CadenciiCommandType.BEZIER_CHAIN_ADD) {
                    #region AddBezierChain
#if DEBUG
                    AppManager.debugWriteLine("    AddBezierChain");
#endif
                    int track = (int)command.args[0];
                    CurveType curve_type = (CurveType)command.args[1];
                    BezierChain chain = (BezierChain)command.args[2];
                    int clock_resolution = (int)command.args[3];
                    int added_id = (int)command.args[4];
                    AttachedCurves.get(track - 1).addBezierChain(curve_type, chain, added_id);
                    ret = generateCommandDeleteBezierChain(track, curve_type, added_id, clock_resolution);
                    if (chain.size() >= 1) {
                        // ベジエ曲線が，時間軸方向のどの範囲にわたって指定されているか判定
                        int min = (int)chain.points[0].getBase().getX();
                        int max = min;
                        int points_size = chain.points.Count;
                        for (int i = 1; i < points_size; i++) {
                            int x = (int)chain.points[i].getBase().getX();
                            min = Math.Min(min, x);
                            max = Math.Max(max, x);
                        }

                        int max_value = curve_type.getMaximum();
                        int min_value = curve_type.getMinimum();
                        VsqBPList list = Track[track].getCurve(curve_type.getName());
                        if (min <= max && list != null) {
                            // minクロック以上maxクロック以下のコントロールカーブに対して，編集を実行

                            // 最初に，min <= clock <= maxとなる範囲のデータ点を抽出（削除コマンドに指定）
                            List<long> delete = new List<long>();
                            int list_size = list.size();
                            for (int i = 0; i < list_size; i++) {
                                int clock = list.getKeyClock(i);
                                if (min <= clock && clock <= max) {
                                    VsqBPPair item = list.getElementB(i);
                                    delete.Add(item.id);
                                }
                            }

                            // 追加するデータ点を列挙
                            SortedDictionary<int, VsqBPPair> add = new SortedDictionary<int, VsqBPPair>();
                            if (chain.points.Count == 1) {
                                BezierPoint p = chain.points[0];
                                add[(int)p.getBase().getX()] = new VsqBPPair((int)p.getBase().getY(), list.getMaxID() + 1);
                            } else {
                                int last_value = int.MaxValue;
                                int index = 0;
                                for (int clock = min; clock <= max; clock += clock_resolution) {
                                    int value = (int)chain.getValue((float)clock);
                                    if (value < min_value) {
                                        value = min_value;
                                    } else if (max_value < value) {
                                        value = max_value;
                                    }
                                    if (value != last_value) {
#if DEBUG
                                        sout.println("VsqFileEx#executeCommand; clock,value=" + clock + "," + value);
#endif
                                        index++;
                                        add[clock] = new VsqBPPair(value, list.getMaxID() + index);
                                        last_value = value;
                                    }
                                }
                            }
                            command.vsqCommand = VsqCommand.generateCommandTrackCurveEdit2(track, curve_type.getName(), delete, add);
                        }
                    }

                    editorStatus.renderRequired[track - 1] = true;
                    #endregion
                } else if (command.type == CadenciiCommandType.BEZIER_CHAIN_DELETE) {
                    #region DeleteBezierChain
                    int track = (int)command.args[0];
                    CurveType curve_type = (CurveType)command.args[1];
                    int chain_id = (int)command.args[2];
                    int clock_resolution = (int)command.args[3];
                    BezierChain chain = (BezierChain)AttachedCurves.get(track - 1).getBezierChain(curve_type, chain_id).clone();
                    AttachedCurves.get(track - 1).remove(curve_type, chain_id);
                    ret = generateCommandAddBezierChain(track, curve_type, chain_id, clock_resolution, chain);
                    int points_size = chain.points.Count;
                    int min = (int)chain.points[0].getBase().getX();
                    int max = min;
                    for (int i = 1; i < points_size; i++) {
                        int x = (int)chain.points[i].getBase().getX();
                        min = Math.Min(min, x);
                        max = Math.Max(max, x);
                    }
                    VsqBPList list = Track[track].getCurve(curve_type.getName());
                    int list_size = list.size();
                    List<long> delete = new List<long>();
                    for (int i = 0; i < list_size; i++) {
                        int clock = list.getKeyClock(i);
                        if (min <= clock && clock <= max) {
                            delete.Add(list.getElementB(i).id);
                        } else if (max < clock) {
                            break;
                        }
                    }
                    command.vsqCommand = VsqCommand.generateCommandTrackCurveEdit2(track, curve_type.getName(), delete, new SortedDictionary<int, VsqBPPair>());
                    editorStatus.renderRequired[track - 1] = true;
                    #endregion
                } else if (command.type == CadenciiCommandType.BEZIER_CHAIN_REPLACE) {
                    #region ReplaceBezierChain
                    int track = (int)command.args[0];
                    CurveType curve_type = (CurveType)command.args[1];
                    int chain_id = (int)command.args[2];
                    BezierChain chain = (BezierChain)command.args[3];
                    int clock_resolution = (int)command.args[4];
                    BezierChain target = (BezierChain)AttachedCurves.get(track - 1).getBezierChain(curve_type, chain_id).clone();
                    AttachedCurves.get(track - 1).setBezierChain(curve_type, chain_id, chain);
                    VsqBPList list = Track[track].getCurve(curve_type.getName());
                    ret = generateCommandReplaceBezierChain(track, curve_type, chain_id, target, clock_resolution);
                    if (chain.size() == 1) {
                        // リプレース後のベジエ曲線が，1個のデータ点のみからなる場合
                        int ex_min = (int)chain.points[0].getBase().getX();
                        int ex_max = ex_min;
                        if (target.points.Count > 1) {
                            // リプレースされる前のベジエ曲線が，どの時間範囲にあったか？
                            int points_size = target.points.Count;
                            for (int i = 1; i < points_size; i++) {
                                int x = (int)target.points[i].getBase().getX();
                                ex_min = Math.Min(ex_min, x);
                                ex_max = Math.Max(ex_max, x);
                            }
                            if (ex_min < ex_max) {
                                // ex_min以上ex_max以下の範囲にあるデータ点を消す
                                List<long> delete = new List<long>();
                                int list_size = list.size();
                                for (int i = 0; i < list_size; i++) {
                                    int clock = list.getKeyClock(i);
                                    if (ex_min <= clock && clock <= ex_max) {
                                        delete.Add(list.getElementB(i).id);
                                    }
                                    if (ex_max < clock) {
                                        break;
                                    }
                                }

                                // リプレース後のデータ点は1個だけ
                                SortedDictionary<int, VsqBPPair> add = new SortedDictionary<int, VsqBPPair>();
                                PointD p = chain.points[0].getBase();
                                add[(int)p.getX()] = new VsqBPPair((int)p.getY(), list.getMaxID() + 1);

                                command.vsqCommand = VsqCommand.generateCommandTrackCurveEdit2(track, curve_type.getName(), delete, add);
                            }
                        }
                    } else if (chain.size() > 1) {
                        // リプレース後のベジエ曲線の範囲
                        int min = (int)chain.points[0].getBase().getX();
                        int max = min;
                        int points_size = chain.points.Count;
                        for (int i = 1; i < points_size; i++) {
                            int x = (int)chain.points[i].getBase().getX();
                            min = Math.Min(min, x);
                            max = Math.Max(max, x);
                        }

                        // リプレース前のベジエ曲線の範囲
                        int ex_min = min;
                        int ex_max = max;
                        if (target.points.Count > 0) {
                            ex_min = (int)target.points[0].getBase().getX();
                            ex_max = ex_min;
                            int target_points_size = target.points.Count;
                            for (int i = 1; i < target_points_size; i++) {
                                int x = (int)target.points[i].getBase().getX();
                                ex_min = Math.Min(ex_min, x);
                                ex_max = Math.Max(ex_max, x);
                            }
                        }

                        // 削除するのを列挙
                        List<long> delete = new List<long>();
                        int list_size = list.size();
                        for (int i = 0; i < list_size; i++) {
                            int clock = list.getKeyClock(i);
                            if (ex_min <= clock && clock <= ex_max) {
                                delete.Add(list.getElementB(i).id);
                            }
                            if (ex_max < clock) {
                                break;
                            }
                        }

                        // 追加するのを列挙
                        int max_value = curve_type.getMaximum();
                        int min_value = curve_type.getMinimum();
                        SortedDictionary<int, VsqBPPair> add = new SortedDictionary<int, VsqBPPair>();
                        if (min < max) {
                            int last_value = int.MaxValue;
                            int index = 0;
                            for (int clock = min; clock < max; clock += clock_resolution) {
                                int value = (int)chain.getValue((float)clock);
                                if (value < min_value) {
                                    value = min_value;
                                } else if (max_value < value) {
                                    value = max_value;
                                }
                                if (last_value != value) {
                                    index++;
                                    add[clock] = new VsqBPPair(value, list.getMaxID() + index);
                                }
                            }
                        }
                        command.vsqCommand = VsqCommand.generateCommandTrackCurveEdit2(track, curve_type.getName(), delete, add);
                    }

                    editorStatus.renderRequired[track - 1] = true;
                    #endregion
                } else if (command.type == CadenciiCommandType.REPLACE) {
                    #region Replace
                    VsqFileEx vsq = (VsqFileEx)command.args[0];
                    VsqFileEx inv = (VsqFileEx)this.clone();
                    Track.Clear();
                    int c = vsq.Track.Count;
                    for (int i = 0; i < c; i++) {
                        Track.Add((VsqTrack)vsq.Track[i].clone());
                    }
                    TempoTable.Clear();
                    c = vsq.TempoTable.Count;
                    for (int i = 0; i < c; i++) {
                        TempoTable.Add((TempoTableEntry)vsq.TempoTable[i].clone());
                    }
                    TimesigTable.Clear();
                    c = vsq.TimesigTable.Count;
                    for (int i = 0; i < c; i++) {
                        TimesigTable.Add((TimeSigTableEntry)vsq.TimesigTable[i].clone());
                    }
                    //m_tpq = vsq.m_tpq;
                    TotalClocks = vsq.TotalClocks;
                    //m_base_tempo = vsq.m_base_tempo;
                    Master = (VsqMaster)vsq.Master.clone();
                    Mixer = (VsqMixer)vsq.Mixer.clone();
                    AttachedCurves = (AttachedCurve)vsq.AttachedCurves.clone();
                    updateTotalClocks();
                    ret = generateCommandReplace(inv);

                    int count = Track.Count;
                    for (int i = 0; i < count - 1; i++) {
                        editorStatus.renderRequired[i] = true;
                    }
                    for (int i = count - 1; i < AppManager.MAX_NUM_TRACK; i++) {
                        editorStatus.renderRequired[i] = false;
                    }
                    #endregion
                } else if (command.type == CadenciiCommandType.ATTACHED_CURVE_REPLACE_RANGE) {
                    #region ReplaceAttachedCurveRange
                    int track = (int)command.args[0];
                    SortedDictionary<CurveType, List<BezierChain>> curves = (SortedDictionary<CurveType, List<BezierChain>>)command.args[1];
                    SortedDictionary<CurveType, List<BezierChain>> inv = new SortedDictionary<CurveType, List<BezierChain>>();
                    foreach (var ct in curves.Keys) {
                        List<BezierChain> chains = new List<BezierChain>();
                        List<BezierChain> src = this.AttachedCurves.get(track - 1).get(ct);
                        for (int i = 0; i < src.Count; i++) {
                            chains.Add((BezierChain)src[i].clone());
                        }
                        inv[ct] = chains;

                        this.AttachedCurves.get(track - 1).get(ct).Clear();
                        foreach (var bc in curves[ct]) {
                            this.AttachedCurves.get(track - 1).get(ct).Add(bc);
                        }
                    }
                    ret = generateCommandReplaceAttachedCurveRange(track, inv);

                    editorStatus.renderRequired[track - 1] = true;
                    #endregion
                } else if (command.type == CadenciiCommandType.TRACK_ADD) {
                    #region AddTrack
                    VsqTrack track = (VsqTrack)command.args[0];
                    VsqMixerEntry mixer = (VsqMixerEntry)command.args[1];
                    int position = (int)command.args[2];
                    BezierCurves attached_curve = (BezierCurves)command.args[3];
                    ret = VsqFileEx.generateCommandDeleteTrack(position);
                    if (Track.Count <= 17) {
                        Track.Insert(position, (VsqTrack)track.clone());
                        AttachedCurves.insertElementAt(position - 1, attached_curve);
                        Mixer.Slave.Insert(position - 1, (VsqMixerEntry)mixer.clone());
                    }

                    for (int i = 15; i >= position; i--) {
                        editorStatus.renderRequired[i] = editorStatus.renderRequired[i - 1];
                    }
                    editorStatus.renderRequired[position - 1] = true;
                    #endregion
                } else if (command.type == CadenciiCommandType.TRACK_DELETE) {
                    #region DeleteTrack
                    int track = (int)command.args[0];
                    ret = VsqFileEx.generateCommandAddTrack(Track[track], Mixer.Slave[track - 1], track, AttachedCurves.get(track - 1));
                    Track.RemoveAt(track);
                    AttachedCurves.removeElementAt(track - 1);
                    Mixer.Slave.RemoveAt(track - 1);
                    updateTotalClocks();

                    for (int i = track - 1; i < 15; i++) {
                        editorStatus.renderRequired[i] = editorStatus.renderRequired[i + 1];
                    }
                    editorStatus.renderRequired[15] = false;
                    #endregion
                } else if (command.type == CadenciiCommandType.TRACK_REPLACE) {
                    #region TrackReplace
                    int track = (int)command.args[0];
                    VsqTrack item = (VsqTrack)command.args[1];
                    BezierCurves bezier_curves = (BezierCurves)command.args[2];
                    ret = VsqFileEx.generateCommandTrackReplace(track, Track[track], AttachedCurves.get(track - 1));
                    Track[track] = item;
                    AttachedCurves.set(track - 1, bezier_curves);
                    updateTotalClocks();

                    editorStatus.renderRequired[track - 1] = true;
                    #endregion
                } else if (command.type == CadenciiCommandType.BGM_UPDATE) {
                    #region BGM_UPDATE
                    List<BgmFile> list = (List<BgmFile>)command.args[0];
                    ret = VsqFileEx.generateCommandBgmUpdate(BgmFiles);
                    BgmFiles.Clear();
                    int count = list.Count;
                    for (int i = 0; i < count; i++) {
                        BgmFiles.Add(list[i]);
                    }
                    #endregion
                } else if (command.type == CadenciiCommandType.CHANGE_SEQUENCE_CONFIG) {
                    #region CHANGE_SEQUENCE_CONFIG
                    int old_pre_measure = Master.PreMeasure;
                    ret = VsqFileEx.generateCommandChangeSequenceConfig(
                        config.SamplingRate, config.WaveFileOutputChannel, config.WaveFileOutputFromMasterTrack, old_pre_measure);
                    int sample_rate = (int)command.args[0];
                    int channels = (int)command.args[1];
                    bool output_master = (Boolean)command.args[2];
                    int pre_measure = (int)command.args[3];
                    config.SamplingRate = sample_rate;
                    config.WaveFileOutputChannel = channels;
                    config.WaveFileOutputFromMasterTrack = output_master;
                    Master.PreMeasure = pre_measure;
                    if (pre_measure != old_pre_measure) {
                        updateTimesigInfo();
                    }
                    #endregion
                }
                if (command.vsqCommand != null && ret != null) {
#if DEBUG
                    AppManager.debugWriteLine("VsqFileEx.executeCommand; command.VsqCommand.Type=" + command.vsqCommand.Type);
#endif
                    ret.vsqCommand = base.executeCommand(command.vsqCommand);
                }
            }
            return ret;
        }

        public VsqFileEx()
            :
 this("Miku", 1, 4, 4, 500000)
        {
            Track.Clear();
            TempoTable.Clear();
            TimesigTable.Clear();
        }

        public VsqFileEx(string singer, int pre_measure, int numerator, int denominator, int tempo)
            :
 base(singer, pre_measure, numerator, denominator, tempo)
        {
            AttachedCurves = new AttachedCurve();
            int count = Track.Count;
            for (int i = 1; i < count; i++) {
                AttachedCurves.add(new BezierCurves());
            }
        }

        public VsqFileEx(UstFile ust)
            :
 base(ust)
        {
            AttachedCurves = new AttachedCurve();
            int count = Track.Count;
            for (int i = 1; i < count; i++) {
                AttachedCurves.add(new BezierCurves());
            }
        }

        public VsqFileEx(string _fpath, string encoding)
            :
 base(_fpath, encoding)
        {
            AttachedCurves = new AttachedCurve();

            string xml = Path.Combine(PortUtil.getDirectoryName(_fpath), PortUtil.getFileName(_fpath) + ".xml");
            for (int i = 1; i < Track.Count; i++) {
                AttachedCurves.add(new BezierCurves());
            }

            // UTAUでエクスポートしたIconHandleは、IDS=UTAUとなっているので探知する
            int count = Track.Count;
            for (int i = 1; i < count; i++) {
                VsqTrack track = Track[i];
                for (Iterator<VsqEvent> itr = track.getSingerEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = itr.next();
                    if (((IconHandle)ve.ID.IconHandle).IDS.ToLower().Equals("utau")) {
                        setTrackRendererKind(track, RendererKind.UTAU);
                        break;
                    }
                }
            }
        }

        public void writeAsXml(string file)
        {
            FileStream xw = null;
            try {
                xw = new FileStream(file, FileMode.Create, FileAccess.Write);
                mVsqSerializer.serialize(xw, this);
            } catch (Exception ex) {
                serr.println("VsqFileEx#writeAsXml; ex=" + ex);
                Logger.write(typeof(VsqFileEx) + ".writeAsXml; ex=" + ex + "\n");
            } finally {
                if (xw != null) {
                    try {
                        xw.Close();
                    } catch (Exception ex2) {
                        serr.println("VsqFileEx#writeAsXml; ex2=" + ex2);
                        Logger.write(typeof(VsqFileEx) + ".writeAsXml; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        public static VsqFileEx readFromXml(string file)
        {
            VsqFileEx ret = null;
            FileStream fs = null;
            try {
                fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                ret = (VsqFileEx)mVsqSerializer.deserialize(fs);
            } catch (Exception ex) {
                serr.println("VsqFileEx#readFromXml; ex=" + ex);
                Logger.write(typeof(VsqFileEx) + ".readFromXml; ex=" + ex + "\n");
            } finally {
                if (fs != null) {
                    try {
                        fs.Close();
                    } catch (Exception ex2) {
                        serr.println("VsqFileEx#readFromXml; ex2=" + ex2);
                        Logger.write(typeof(VsqFileEx) + ".readFromXml; ex=" + ex2 + "\n");
                    }
                }
            }

            if (ret == null) {
                return null;
            }

            // ベジエ曲線のIDを播番
            if (ret.AttachedCurves != null) {
                int numTrack = ret.Track.Count;
                if (ret.AttachedCurves.getCurves().Count + 1 != numTrack) {
                    // ベジエ曲線のデータコンテナの個数と、トラックの個数が一致しなかった場合
                    ret.AttachedCurves.getCurves().Clear();
                    for (int i = 1; i < numTrack; i++) {
                        ret.AttachedCurves.add(new BezierCurves());
                    }
                } else {
                    foreach (var bc in ret.AttachedCurves.getCurves()) {
                        for (int k = 0; k < Utility.CURVE_USAGE.Length; k++) {
                            CurveType ct = Utility.CURVE_USAGE[k];
                            List<BezierChain> list = bc.get(ct);
                            int list_size = list.Count;
                            for (int i = 0; i < list_size; i++) {
                                BezierChain chain = list[i];
                                chain.id = i + 1;
                                int points_size = chain.points.Count;
                                for (int j = 0; j < points_size; j++) {
                                    chain.points[j].setID(j + 1);
                                }
                            }
                        }
                    }
                }
            } else {
                int count = ret.Track.Count;
                for (int i = 1; i < count; i++) {
                    ret.AttachedCurves.add(new BezierCurves());
                }
            }

            // VsqBPListのNameを更新
            int c = ret.Track.Count;
            for (int i = 0; i < c; i++) {
                VsqTrack track = ret.Track[i];
                foreach (CurveType s in Utility.CURVE_USAGE) {
                    VsqBPList list = track.getCurve(s.getName());
                    if (list != null) {
                        list.setName(s.getName().ToLower());
                    }
                }
            }
            return ret;
        }
    }

}
