/*
 * BezierCurves.cs
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
using System.Collections.Generic;
using cadencii.java.util;

namespace cadencii
{

    /// <summary>
    /// AtatchedCurveをXMLシリアライズするためのクラス
    /// であると同時に，各トラックに付随する情報を格納するためのクラス←New!
    /// </summary>
    public class BezierCurves : ICloneable
    {
        public static readonly CurveType[] CURVES = new CurveType[]{
            CurveType.DYN,
            CurveType.BRE,
            CurveType.BRI,
            CurveType.CLE,
            CurveType.OPE,
            CurveType.GEN,
            CurveType.POR,
            CurveType.VibratoRate,
            CurveType.VibratoDepth,
            CurveType.harmonics,
            CurveType.fx2depth,
            CurveType.reso1freq,
            CurveType.reso1bw,
            CurveType.reso1amp,
            CurveType.reso2freq,
            CurveType.reso2bw,
            CurveType.reso2amp,
            CurveType.reso3freq,
            CurveType.reso3bw,
            CurveType.reso3amp,
            CurveType.reso4freq,
            CurveType.reso4bw,
            CurveType.reso4amp,
            CurveType.PIT,
            CurveType.PBS, };

        public List<BezierChain> Dynamics;
        public List<BezierChain> Brethiness;
        public List<BezierChain> Brightness;
        public List<BezierChain> Clearness;
        public List<BezierChain> Opening;
        public List<BezierChain> GenderFactor;
        public List<BezierChain> PortamentoTiming;
        public List<BezierChain> VibratoRate;
        public List<BezierChain> VibratoDepth;
        public List<BezierChain> Harmonics;
        public List<BezierChain> FX2Depth;
        public List<BezierChain> Reso1Freq;
        public List<BezierChain> Reso1BW;
        public List<BezierChain> Reso1Amp;
        public List<BezierChain> Reso2Freq;
        public List<BezierChain> Reso2BW;
        public List<BezierChain> Reso2Amp;
        public List<BezierChain> Reso3Freq;
        public List<BezierChain> Reso3BW;
        public List<BezierChain> Reso3Amp;
        public List<BezierChain> Reso4Freq;
        public List<BezierChain> Reso4BW;
        public List<BezierChain> Reso4Amp;
        public List<BezierChain> PitchBend;
        public List<BezierChain> PitchBendSensitivity;

        public BezierCurves()
        {
            Dynamics = new List<BezierChain>();
            Brethiness = new List<BezierChain>();
            Brightness = new List<BezierChain>();
            Clearness = new List<BezierChain>();
            Opening = new List<BezierChain>();
            GenderFactor = new List<BezierChain>();
            PortamentoTiming = new List<BezierChain>();
            VibratoRate = new List<BezierChain>();
            VibratoDepth = new List<BezierChain>();
            Harmonics = new List<BezierChain>();
            FX2Depth = new List<BezierChain>();
            Reso1Freq = new List<BezierChain>();
            Reso1BW = new List<BezierChain>();
            Reso1Amp = new List<BezierChain>();
            Reso2Freq = new List<BezierChain>();
            Reso2BW = new List<BezierChain>();
            Reso2Amp = new List<BezierChain>();
            Reso3Freq = new List<BezierChain>();
            Reso3BW = new List<BezierChain>();
            Reso3Amp = new List<BezierChain>();
            Reso4Freq = new List<BezierChain>();
            Reso4BW = new List<BezierChain>();
            Reso4Amp = new List<BezierChain>();
            PitchBend = new List<BezierChain>();
            PitchBendSensitivity = new List<BezierChain>();
        }

        public BezierChain getBezierChain(CurveType curve_type, int chain_id)
        {
            List<BezierChain> list = this.get(curve_type);
            int count = list.Count;
            for (int i = 0; i < count; i++) {
                if (list[i].id == chain_id) {
                    return list[i];
                }
            }
            return null;
        }

        public void setBezierChain(CurveType curve_type, int chain_id, BezierChain item)
        {
            List<BezierChain> list = this.get(curve_type);
            int count = list.Count;
            for (int i = 0; i < count; i++) {
                if (list[i].id == chain_id) {
                    list[i] = item;
                    break;
                }
            }
        }

        /// <summary>
        /// 指定した種類のコントロールカーブにベジエ曲線を追加します。
        /// AddBezierChainとの違い、オーバーラップする部分があれば自動的に結合されます。
        /// chainには2個以上のデータ点が含まれている必要がある
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="chain"></param>
        public void mergeBezierChain(CurveType curve, BezierChain chain)
        {
            if (chain.points.Count <= 1) {
                return;
            }
            int chain_start = (int)chain.getStart();
            int chain_end = (int)chain.getEnd();

            // まず、全削除する必要のあるBezierChainを検索
            List<int> delete_list = new List<int>();
            List<BezierChain> src = this.get(curve);
            //foreach ( int id in this[curve].Keys ) {
            for (int j = 0; j < src.Count; j++) {
                //BezierChain bc = this[curve][id];
                BezierChain bc = src[j];
                if (bc.points.Count <= 0) {
                    continue;
                }
                int bc_start = (int)bc.getStart();
                int bc_end = (int)bc.getEnd();
                if (chain_start <= bc_start && bc_end <= chain_end) {
                    delete_list.Add(bc.id);
                }
            }

            // 削除を実行
            foreach (var id in delete_list) {
                remove(curve, id);
                //this[curve].Remove( id );
            }

            // マージする必要があるかどうかを検査。
            bool processed = true;
            while (processed) {
                processed = false;
                List<BezierChain> list = this.get(curve);
                //foreach ( int id in this[curve].Keys ) {
                for (int j = 0; j < list.Count; j++) {
                    //BezierChain bc = this[curve][id];
                    BezierChain bc = list[j];
                    int id = bc.id;
                    int start = (int)bc.getStart();
                    int end = (int)bc.getEnd();

                    // 被っている箇所が2箇所以上ある可能性があるので、ifでヒットしてもbreakしない
                    if (start < chain_start && chain_start <= end && end < chain_end) {
                        // bcのchain_start ~ endを削除し、chain_startで結合
                        BezierChain bc_edit = null;
                        try {
                            bc_edit = bc.extractPartialBezier(start, chain_start);
                        } catch (Exception ex) {
                            Logger.write(typeof(BezierCurves) + ".mergeBezierChain; ex=" + ex + "\n");
                            continue;
                        }
                        bc_edit.id = bc.id;
                        int last = bc_edit.size() - 1;

                        // 接合部分では、制御点無しでステップ変化する
                        bc_edit.points[last].setControlRightType(BezierControlType.None);
                        chain.points[0].setControlLeftType(BezierControlType.None);

                        int copy_start = 0;
                        if (bc_edit.points[last].getBase().getY() == chain.points[0].getBase().getY()) {
                            // bcの終点とchainの始点の座標が一致している場合
                            if (bc_edit.points[last].getControlLeftType() != BezierControlType.None) {
                                bc_edit.points[last].setControlLeftType(BezierControlType.Master);
                            }
                            bc_edit.points[last].setControlRight(chain.points[0].controlLeft);
                            if (chain.points[0].getControlRightType() != BezierControlType.None) {
                                bc_edit.points[last].setControlLeftType(BezierControlType.Master);
                            }
                            copy_start = 1;
                        }
                        for (int i = copy_start; i < chain.points.Count; i++) {
                            chain.points[i].setID(bc_edit.getNextId());
                            bc_edit.add(chain.points[i]);
                        }
                        //this[curve].Remove( id );
                        remove(curve, id);
                        chain = bc_edit;
                        chain_start = (int)chain.getStart();
                        chain_end = (int)chain.getEnd();
                        processed = true;
                        break;
                    } else if (chain_start < start && start <= chain_end && chain_end < end) {
                        // bcのstart ~ chain_endを削除し、chain_endで結合
                        BezierChain bc_edit = null;
                        try {
                            bc_edit = bc.extractPartialBezier(chain_end, end);
                        } catch (Exception ex) {
                            Logger.write(typeof(BezierCurves) + ".mergeBezierChain; ex=" + ex + "\n");
                            continue;
                        }
                        bc_edit.id = bc.id;
                        int last = chain.size() - 1;

                        // 接合部分では、制御点無しでステップ変化する
                        bc_edit.points[0].setControlLeftType(BezierControlType.None);
                        chain.points[last].setControlRightType(BezierControlType.None);

                        int copy_end = last;
                        if (chain.points[last].getBase().getY() == bc_edit.points[0].getBase().getY()) {
                            // bcの終点とchainの始点の座標が一致している場合
                            if (chain.points[last].getControlLeftType() != BezierControlType.None) {
                                chain.points[last].setControlLeftType(BezierControlType.Master);
                            }
                            chain.points[last].setControlRight(bc_edit.points[0].controlLeft);
                            if (bc_edit.points[0].getControlRightType() != BezierControlType.None) {
                                chain.points[last].setControlLeftType(BezierControlType.Master);
                            }
                            copy_end = last - 1;
                        }
                        for (int i = 0; i <= copy_end; i++) {
                            chain.points[i].setID(bc_edit.getNextId());
                            bc_edit.add(chain.points[i]);
                        }
                        //this[curve].Remove( id );
                        remove(curve, id);
                        chain = bc_edit;
                        chain_start = (int)chain.getStart();
                        chain_end = (int)chain.getEnd();
                        processed = true;
                        break;
                    } else if (start < chain_start && chain_end < end) {
                        // bcのchain_start ~ chain_endをchainで置き換え
                        // left + chain + right
                        BezierChain left = null;
                        try {
                            left = bc.extractPartialBezier(start, chain_start);
                        } catch (Exception ex) {
                            Logger.write(typeof(BezierCurves) + ".mergeBezierChain; ex=" + ex + "\n");
                            continue;
                        }
                        BezierChain right = null;
                        try {
                            right = bc.extractPartialBezier(chain_end, end);
                        } catch (Exception ex) {
                            Logger.write(typeof(BezierCurves) + ".mergeBezierChain; ex=" + ex + "\n");
                            continue;
                        }
                        left.id = bc.id;

                        // 接合部ではステップ変化
                        left.points[left.size() - 1].setControlRightType(BezierControlType.None);
                        chain.points[0].setControlLeftType(BezierControlType.None);
                        chain.points[chain.size() - 1].setControlRightType(BezierControlType.None);
                        right.points[0].setControlLeftType(BezierControlType.None);

                        int copy_start = 0;
                        int copy_end = chain.size() - 1;

                        if (left.points[left.size() - 1].getBase().getY() == chain.points[0].getBase().getY()) {
                            // bcの終点とchainの始点の座標が一致している場合
                            if (left.points[left.size() - 1].getControlLeftType() != BezierControlType.None) {
                                left.points[left.size() - 1].setControlLeftType(BezierControlType.Master);
                            }
                            left.points[left.size() - 1].setControlRight(chain.points[0].controlLeft);
                            if (chain.points[0].getControlRightType() != BezierControlType.None) {
                                left.points[left.size() - 1].setControlLeftType(BezierControlType.Master);
                            }
                            copy_start = 1;
                        }

                        if (chain.points[chain.size() - 1].getBase().getY() == right.points[0].getBase().getY()) {
                            // bcの終点とchainの始点の座標が一致している場合
                            if (chain.points[chain.size() - 1].getControlLeftType() != BezierControlType.None) {
                                chain.points[chain.size() - 1].setControlLeftType(BezierControlType.Master);
                            }
                            chain.points[chain.size() - 1].setControlRight(right.points[0].controlLeft);
                            if (right.points[0].getControlRightType() != BezierControlType.None) {
                                chain.points[chain.size() - 1].setControlLeftType(BezierControlType.Master);
                            }
                            copy_end = chain.size() - 2;
                        }

                        // 追加
                        for (int i = copy_start; i <= copy_end; i++) {
                            chain.points[i].setID(left.getNextId());
                            left.add(chain.points[i]);
                        }
                        for (int i = 0; i < right.points.Count; i++) {
                            right.points[i].setID(left.getNextId());
                            left.add(right.points[i]);
                        }
                        //this[curve].Remove( id );
                        remove(curve, id);
                        chain = left;
                        chain_start = (int)chain.getStart();
                        chain_end = (int)chain.getEnd();
                        processed = true;
                        break;
                    }
                }
            }

            if (!processed) {
                chain.id = this.getNextId(curve);
            }
            //this[curve].Add( chain.ID, chain );
            addBezierChain(curve, chain, chain.id);
        }

        /// <summary>
        /// 指定した位置に，指定した量の空白を挿入します
        /// </summary>
        /// <param name="clock_start">空白を挿入する位置</param>
        /// <param name="clock_amount">挿入する空白の量</param>
        public void insertBlank(int clock_start, int clock_amount)
        {
            // 全種類のカーブ
            List<CurveType> target_curve = new List<CurveType>();
            foreach (CurveType ct in CURVES) {
                List<BezierChain> vbc = get(ct);
                foreach (var bc in vbc) {
                    int size = bc.points.Count;
                    for (int i = 0; i < size; i++) {
                        BezierPoint bp = bc.points[i];
                        PointD p = bp.getBase();
                        if (clock_start <= p.getX()) {
                            p.setX(p.getX() + clock_amount);
                            bp.setBase(p);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 全種類のカーブの指定した範囲を削除します．
        /// 削除範囲以降の部分は，操作後には(clock_end - clock_start)だけシフトされます．
        /// </summary>
        /// <param name="clock_start"></param>
        /// <param name="clock_end"></param>
        public void removePart(int clock_start, int clock_end)
        {
            // 全種類のカーブが対象
            List<CurveType> target_curve = new List<CurveType>();
            foreach (CurveType ct in CURVES) {
                target_curve.Add(ct);
            }

            // 便利メソッドで削除をやる
            deleteBeziers(target_curve, clock_start, clock_end);

            // データ点のclockがclock_end以降になってるものについて，シフトを行う
            int delta = clock_end - clock_start;
            foreach (var ct in target_curve) {
                List<BezierChain> vbc = get(ct);
                foreach (var bc in vbc) {
                    int size = bc.points.Count;
                    for (int i = 0; i < size; i++) {
                        BezierPoint bp = bc.points[i];
                        PointD p = bp.getBase();
                        if (clock_end <= p.getX()) {
                            p.setX(p.getX() - delta);
                            bp.setBase(p);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 指定したカーブ種類のベジエ曲線の，指定した範囲を削除します．
        /// 削除はclock_startからclock_endの範囲について行われ，clock_end以降のシフト操作は行われません．
        /// つまり，操作後にclock_end以降のイベントが(clock_end - clock_start)だけ前方にシフトしてくることはありません．
        /// </summary>
        /// <param name="target_curve"></param>
        /// <param name="clock_start"></param>
        /// <param name="clock_end"></param>
        /// <returns></returns>
        public bool deleteBeziers(
            List<CurveType> target_curve,
            int clock_start,
            int clock_end
        )
        {
            bool edited = false;
            foreach (var curve in target_curve) {
                if (curve.isScalar() || curve.isAttachNote()) {
                    continue;
                }
                List<BezierChain> tmp = new List<BezierChain>();
                foreach (var bc in this.get(curve)) {
                    int len = bc.points.Count;
                    if (len < 1) {
                        continue;
                    }
                    int chain_start = (int)bc.points[0].getBase().getX();
                    int chain_end;
                    if (len < 2) {
                        chain_end = chain_start;
                    } else {
                        chain_end = (int)bc.points[len - 1].getBase().getX();
                    }
                    if (clock_start < chain_start && chain_start < clock_end && clock_end < chain_end) {
                        // end ~ chain_endを残す
                        try {
                            BezierChain chain = bc.extractPartialBezier(clock_end, chain_end);
                            chain.id = bc.id;
                            tmp.Add(chain);
                            edited = true;
                        } catch (Exception ex) {
                            Logger.write(typeof(BezierCurves) + ".deleteBeziers; ex=" + ex + "\n");
                        }
                    } else if (chain_start <= clock_start && clock_end <= chain_end) {
                        // chain_start ~ startとend ~ chain_endを残す
                        try {
                            BezierChain chain1 = bc.extractPartialBezier(chain_start, clock_start);
                            chain1.id = bc.id;
                            BezierChain chain2 = bc.extractPartialBezier(clock_end, chain_end);
                            chain2.id = -1;  // 後で番号をつける
                            tmp.Add(chain1);
                            tmp.Add(chain2);
                            edited = true;
                        } catch (Exception ex) {
                            Logger.write(typeof(BezierCurves) + ".deleteBeziers; ex=" + ex + "\n");
                        }
                    } else if (chain_start < clock_start && clock_start < chain_end && chain_end < clock_end) {
                        // chain_start ~ startを残す
                        try {
                            BezierChain chain = bc.extractPartialBezier(chain_start, clock_start);
                            chain.id = bc.id;
                            tmp.Add(chain);
                            edited = true;
                        } catch (Exception ex) {
                            Logger.write(typeof(BezierCurves) + ".deleteBeiers; ex=" + ex + "\n");
                        }
                    } else if (clock_start <= chain_start && chain_end <= clock_end) {
                        // 全体を削除
                        edited = true;
                    } else {
                        // 全体を残す
                        tmp.Add((BezierChain)bc.clone());
                    }
                }
                this.get(curve).Clear();
                foreach (var bc in tmp) {
                    if (bc.id >= 0) {
                        addBezierChain(curve, bc, bc.id);
                    }
                }
                foreach (var bc in tmp) {
                    if (bc.id < 0) {
                        bc.id = this.getNextId(curve);
                        addBezierChain(curve, bc, bc.id);
                    }
                }
            }
            return edited;
        }

        public void remove(CurveType curve_type, int chain_id)
        {
            List<BezierChain> list = this.get(curve_type);
            for (int i = 0; i < list.Count; i++) {
                if (list[i].id == chain_id) {
                    list.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 指定したコントロールカーブにベジエ曲線を追加します。
        /// </summary>
        /// <param name="curve_type"></param>
        /// <param name="chain"></param>
        public void addBezierChain(CurveType curve_type, BezierChain chain, int chain_id)
        {
            BezierChain add = (BezierChain)chain.clone();
            add.id = chain_id;
            this.get(curve_type).Add(add);
        }

        public Object clone()
        {
            BezierCurves ret = new BezierCurves();
            for (int j = 0; j < Utility.CURVE_USAGE.Length; j++) {
                CurveType ct = Utility.CURVE_USAGE[j];
                List<BezierChain> src = this.get(ct);
                ret.set(ct, new List<BezierChain>());
                int count = src.Count;
                for (int i = 0; i < count; i++) {
                    ret.get(ct).Add((BezierChain)src[i].clone());
                }
            }
            return ret;
        }

        public Object Clone()
        {
            return clone();
        }

        public int getNextId(CurveType curve_type)
        {
            List<BezierChain> bc = this.get(curve_type);
            int ret = bc.Count;
            bool found = true;
            while (found) {
                found = false;
                for (int i = 0; i < bc.Count; i++) {
                    if (bc[i].id == ret) {
                        found = true;
                        ret++;
                        break;
                    }
                }
            }
            return ret;
        }

        public List<BezierChain> get(CurveType curve)
        {
            if (curve.equals(CurveType.BRE)) {
                return Brethiness;
            } else if (curve.equals(CurveType.BRI)) {
                return Brightness;
            } else if (curve.equals(CurveType.CLE)) {
                return Clearness;
            } else if (curve.equals(CurveType.DYN)) {
                return Dynamics;
            } else if (curve.equals(CurveType.fx2depth)) {
                return FX2Depth;
            } else if (curve.equals(CurveType.GEN)) {
                return GenderFactor;
            } else if (curve.equals(CurveType.harmonics)) {
                return Harmonics;
            } else if (curve.equals(CurveType.OPE)) {
                return Opening;
            } else if (curve.equals(CurveType.POR)) {
                return PortamentoTiming;
            } else if (curve.equals(CurveType.PIT)) {
                return PitchBend;
            } else if (curve.equals(CurveType.PBS)) {
                return PitchBendSensitivity;
            } else if (curve.equals(CurveType.reso1amp)) {
                return Reso1Amp;
            } else if (curve.equals(CurveType.reso1bw)) {
                return Reso1BW;
            } else if (curve.equals(CurveType.reso1freq)) {
                return Reso1Freq;
            } else if (curve.equals(CurveType.reso2amp)) {
                return Reso2Amp;
            } else if (curve.equals(CurveType.reso2bw)) {
                return Reso2BW;
            } else if (curve.equals(CurveType.reso2freq)) {
                return Reso2Freq;
            } else if (curve.equals(CurveType.reso3amp)) {
                return Reso3Amp;
            } else if (curve.equals(CurveType.reso3bw)) {
                return Reso3BW;
            } else if (curve.equals(CurveType.reso3freq)) {
                return Reso3Freq;
            } else if (curve.equals(CurveType.reso4amp)) {
                return Reso4Amp;
            } else if (curve.equals(CurveType.reso4bw)) {
                return Reso4BW;
            } else if (curve.equals(CurveType.reso4freq)) {
                return Reso4Freq;
            } else if (curve.equals(CurveType.VibratoDepth)) {
                return VibratoDepth;
            } else if (curve.equals(CurveType.VibratoRate)) {
                return VibratoRate;
            } else {
                return null;
            }
        }

        public void set(CurveType curve, List<BezierChain> value)
        {
            if (curve.equals(CurveType.BRE)) {
                Brethiness = value;
            } else if (curve.equals(CurveType.BRI)) {
                Brightness = value;
            } else if (curve.equals(CurveType.CLE)) {
                Clearness = value;
            } else if (curve.equals(CurveType.DYN)) {
                Dynamics = value;
            } else if (curve.equals(CurveType.fx2depth)) {
                FX2Depth = value;
            } else if (curve.equals(CurveType.GEN)) {
                GenderFactor = value;
            } else if (curve.equals(CurveType.harmonics)) {
                Harmonics = value;
            } else if (curve.equals(CurveType.OPE)) {
                Opening = value;
            } else if (curve.equals(CurveType.POR)) {
                PortamentoTiming = value;
            } else if (curve.equals(CurveType.PIT)) {
                PitchBend = value;
            } else if (curve.equals(CurveType.PBS)) {
                PitchBendSensitivity = value;
            } else if (curve.equals(CurveType.reso1amp)) {
                Reso1Amp = value;
            } else if (curve.equals(CurveType.reso1bw)) {
                Reso1BW = value;
            } else if (curve.equals(CurveType.reso1freq)) {
                Reso1Freq = value;
            } else if (curve.equals(CurveType.reso2amp)) {
                Reso2Amp = value;
            } else if (curve.equals(CurveType.reso2bw)) {
                Reso2BW = value;
            } else if (curve.equals(CurveType.reso2freq)) {
                Reso2Freq = value;
            } else if (curve.equals(CurveType.reso3amp)) {
                Reso3Amp = value;
            } else if (curve.equals(CurveType.reso3bw)) {
                Reso3BW = value;
            } else if (curve.equals(CurveType.reso3freq)) {
                Reso3Freq = value;
            } else if (curve.equals(CurveType.reso4amp)) {
                Reso4Amp = value;
            } else if (curve.equals(CurveType.reso4bw)) {
                Reso4BW = value;
            } else if (curve.equals(CurveType.reso4freq)) {
                Reso4Freq = value;
            } else if (curve.equals(CurveType.VibratoDepth)) {
                VibratoDepth = value;
            } else if (curve.equals(CurveType.VibratoRate)) {
                VibratoRate = value;
            }
        }
    }

}
