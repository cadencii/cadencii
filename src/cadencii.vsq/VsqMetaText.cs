/*
 * VsqMetaText.cs
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
using System.IO;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii.vsq
{

    /// <summary>
    /// vsqのメタテキストの中身を処理するためのクラス
    /// </summary>
    [Serializable]
    public class VsqMetaText : ICloneable
    {
        public VsqCommon Common;
        public VsqMaster master;
        public VsqMixer mixer;
        public VsqEventList Events;
        /// <summary>
        /// PIT。ピッチベンド(pitchBendBPList)。default=0
        /// </summary>
        public VsqBPList PIT;
        /// <summary>
        /// PBS。ピッチベンドセンシティビティ(pitchBendSensBPList)。dfault=2
        /// </summary>
        public VsqBPList PBS;
        /// <summary>
        /// DYN。ダイナミクス(dynamicsBPList)。default=64
        /// </summary>
        public VsqBPList DYN;
        /// <summary>
        /// BRE。ブレシネス(epRResidualBPList)。default=0
        /// </summary>
        public VsqBPList BRE;
        /// <summary>
        /// BRI。ブライトネス(epRESlopeBPList)。default=64
        /// </summary>
        public VsqBPList BRI;
        /// <summary>
        /// CLE。クリアネス(epRESlopeDepthBPList)。default=0
        /// </summary>
        public VsqBPList CLE;
        public VsqBPList reso1FreqBPList;
        public VsqBPList reso2FreqBPList;
        public VsqBPList reso3FreqBPList;
        public VsqBPList reso4FreqBPList;
        public VsqBPList reso1BWBPList;
        public VsqBPList reso2BWBPList;
        public VsqBPList reso3BWBPList;
        public VsqBPList reso4BWBPList;
        public VsqBPList reso1AmpBPList;
        public VsqBPList reso2AmpBPList;
        public VsqBPList reso3AmpBPList;
        public VsqBPList reso4AmpBPList;
        /// <summary>
        /// Harmonics。(EpRSineBPList)default = 64
        /// </summary>
        public VsqBPList harmonics;
        /// <summary>
        /// Effect2 Depth。
        /// </summary>
        public VsqBPList fx2depth;
        /// <summary>
        /// GEN。ジェンダーファクター(genderFactorBPList)。default=64
        /// </summary>
        public VsqBPList GEN;
        /// <summary>
        /// POR。ポルタメントタイミング(portamentoTimingBPList)。default=64
        /// </summary>
        public VsqBPList POR;
        /// <summary>
        /// OPE。オープニング(openingBPList)。default=127
        /// </summary>
        public VsqBPList OPE;

        public Object clone()
        {
            VsqMetaText res = new VsqMetaText();
            if (Common != null) {
                res.Common = (VsqCommon)Common.clone();
            }
            if (master != null) {
                res.master = (VsqMaster)master.clone();
            }
            if (mixer != null) {
                res.mixer = (VsqMixer)mixer.clone();
            }
            if (Events != null) {
                res.Events = new VsqEventList();
                foreach (var item in Events.iterator()) {
                    res.Events.add((VsqEvent)item.clone(), item.InternalID);
                }
            }
            if (PIT != null) {
                res.PIT = (VsqBPList)PIT.clone();
            }
            if (PBS != null) {
                res.PBS = (VsqBPList)PBS.clone();
            }
            if (DYN != null) {
                res.DYN = (VsqBPList)DYN.clone();
            }
            if (BRE != null) {
                res.BRE = (VsqBPList)BRE.clone();
            }
            if (BRI != null) {
                res.BRI = (VsqBPList)BRI.clone();
            }
            if (CLE != null) {
                res.CLE = (VsqBPList)CLE.clone();
            }
            if (reso1FreqBPList != null) {
                res.reso1FreqBPList = (VsqBPList)reso1FreqBPList.clone();
            }
            if (reso2FreqBPList != null) {
                res.reso2FreqBPList = (VsqBPList)reso2FreqBPList.clone();
            }
            if (reso3FreqBPList != null) {
                res.reso3FreqBPList = (VsqBPList)reso3FreqBPList.clone();
            }
            if (reso4FreqBPList != null) {
                res.reso4FreqBPList = (VsqBPList)reso4FreqBPList.clone();
            }
            if (reso1BWBPList != null) {
                res.reso1BWBPList = (VsqBPList)reso1BWBPList.clone();
            }
            if (reso2BWBPList != null) {
                res.reso2BWBPList = (VsqBPList)reso2BWBPList.clone();
            }
            if (reso3BWBPList != null) {
                res.reso3BWBPList = (VsqBPList)reso3BWBPList.clone();
            }
            if (reso4BWBPList != null) {
                res.reso4BWBPList = (VsqBPList)reso4BWBPList.clone();
            }
            if (reso1AmpBPList != null) {
                res.reso1AmpBPList = (VsqBPList)reso1AmpBPList.clone();
            }
            if (reso2AmpBPList != null) {
                res.reso2AmpBPList = (VsqBPList)reso2AmpBPList.clone();
            }
            if (reso3AmpBPList != null) {
                res.reso3AmpBPList = (VsqBPList)reso3AmpBPList.clone();
            }
            if (reso4AmpBPList != null) {
                res.reso4AmpBPList = (VsqBPList)reso4AmpBPList.clone();
            }
            if (harmonics != null) {
                res.harmonics = (VsqBPList)harmonics.clone();
            }
            if (fx2depth != null) {
                res.fx2depth = (VsqBPList)fx2depth.clone();
            }
            if (GEN != null) {
                res.GEN = (VsqBPList)GEN.clone();
            }
            if (POR != null) {
                res.POR = (VsqBPList)POR.clone();
            }
            if (OPE != null) {
                res.OPE = (VsqBPList)OPE.clone();
            }
            return res;
        }

        public object Clone()
        {
            return clone();
        }

        public VsqEventList getEventList()
        {
            return Events;
        }

        public VsqBPList getElement(string curve)
        {
            if (curve == null) return null;
            string search = curve.Trim().ToLower();
            if (search == "bre") {
                return this.BRE;
            } else if (search == "bri") {
                return this.BRI;
            } else if (search == "cle") {
                return this.CLE;
            } else if (search == "dyn") {
                return this.DYN;
            } else if (search == "gen") {
                return this.GEN;
            } else if (search == "ope") {
                return this.OPE;
            } else if (search == "pbs") {
                return this.PBS;
            } else if (search == "pit") {
                return this.PIT;
            } else if (search == "por") {
                return this.POR;
            } else if (search == "harmonics") {
                return this.harmonics;
            } else if (search == "fx2depth") {
                return this.fx2depth;
            } else if (search == "reso1amp") {
                return this.reso1AmpBPList;
            } else if (search == "reso1bw") {
                return this.reso1BWBPList;
            } else if (search == "reso1freq") {
                return this.reso1FreqBPList;
            } else if (search == "reso2amp") {
                return this.reso2AmpBPList;
            } else if (search == "reso2bw") {
                return this.reso2BWBPList;
            } else if (search == "reso2freq") {
                return this.reso2FreqBPList;
            } else if (search == "reso3amp") {
                return this.reso3AmpBPList;
            } else if (search == "reso3bw") {
                return this.reso3BWBPList;
            } else if (search == "reso3freq") {
                return this.reso3FreqBPList;
            } else if (search == "reso4amp") {
                return this.reso4AmpBPList;
            } else if (search == "reso4bw") {
                return this.reso4BWBPList;
            } else if (search == "reso4freq") {
                return this.reso4FreqBPList;
            } else {
                return null;
            }
        }

        public void setElement(string curve, VsqBPList value)
        {
            if (curve == null) return;
            string search = curve.Trim().ToLower();
            if (search == "bre") {
                this.BRE = value;
            } else if (search == "bri") {
                this.BRI = value;
            } else if (search == "cle") {
                this.CLE = value;
            } else if (search == "dyn") {
                this.DYN = value;
            } else if (search == "gen") {
                this.GEN = value;
            } else if (search == "ope") {
                this.OPE = value;
            } else if (search == "pbs") {
                this.PBS = value;
            } else if (search == "pit") {
                this.PIT = value;
            } else if (search == "por") {
                this.POR = value;
            } else if (search == "harmonics") {
                this.harmonics = value;
            } else if (search == "fx2depth") {
                this.fx2depth = value;
            } else if (search == "reso1amp") {
                this.reso1AmpBPList = value;
            } else if (search == "reso1bw") {
                this.reso1BWBPList = value;
            } else if (search == "reso1freq") {
                this.reso1FreqBPList = value;
            } else if (search == "reso2amp") {
                this.reso2AmpBPList = value;
            } else if (search == "reso2bw") {
                this.reso2BWBPList = value;
            } else if (search == "reso2freq") {
                this.reso2FreqBPList = value;
            } else if (search == "reso3amp") {
                this.reso3AmpBPList = value;
            } else if (search == "reso3bw") {
                this.reso3BWBPList = value;
            } else if (search == "reso3freq") {
                this.reso3FreqBPList = value;
            } else if (search == "reso4amp") {
                this.reso4AmpBPList = value;
            } else if (search == "reso4bw") {
                this.reso4BWBPList = value;
            } else if (search == "reso4freq") {
                this.reso4FreqBPList = value;
            } else {
#if DEBUG
                sout.println("VsqMetaText#setElement; warning:unknown curve; curve=" + curve);
#endif
            }
        }

        /// <summary>
        /// Editor画面上で上からindex番目のカーブを表すBPListを求めます
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VsqBPList getCurve(int index)
        {
            switch (index) {
                case 1:
                return DYN;
                case 2:
                return BRE;
                case 3:
                return BRI;
                case 4:
                return CLE;
                case 5:
                return OPE;
                case 6:
                return GEN;
                case 7:
                return POR;
                case 8:
                return PIT;
                case 9:
                return PBS;
                default:
                return null;
            }
        }


        /// <summary>
        /// Editor画面上で上からindex番目のカーブの名前を調べます
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string getCurveName(int index)
        {
            switch (index) {
                case 0:
                return "VEL";
                case 1:
                return "DYN";
                case 2:
                return "BRE";
                case 3:
                return "BRI";
                case 4:
                return "CLE";
                case 5:
                return "OPE";
                case 6:
                return "GEN";
                case 7:
                return "POR";
                case 8:
                return "PIT";
                case 9:
                return "PBS";
                default:
                return "";
            }
        }

        /// <summary>
        /// Singerプロパティに指定されている
        /// </summary>
        public string getSinger()
        {
            if (Events != null) {
                foreach (var item in Events.iterator()) {
                    if (item.ID.type == VsqIDType.Singer) {
                        return item.ID.IconHandle.IDS;
                    }
                }
            }
            return "";
        }

        public void setSinger(string value)
        {
            if (Events == null) return;
            foreach (var item in Events.iterator()) {
                if (item.ID.type == VsqIDType.Singer) {
                    ((IconHandle)item.ID.IconHandle).IDS = value;
                    break;
                }
            }
        }

        /// <summary>
        /// EOSイベントが記録されているクロックを取得します。
        /// </summary>
        /// <returns></returns>
        public int getIndexOfEOS()
        {
            if (Events == null) {
                return -1;
            }
            int result;
            if (Events.getCount() > 0) {
                int ilast = Events.getCount() - 1;
                result = Events.getElement(ilast).Clock;
            } else {
                result = -1;
            }
            return result;
        }

        /// <summary>
        /// このインスタンスから、Handleのリストを作成すると同時に、Eventsに登録されているVsqEventのvalue値および各ハンドルのvalue値を更新します
        /// </summary>
        /// <returns></returns>
        private List<VsqHandle> buildHandleList()
        {
            List<VsqHandle> handle = new List<VsqHandle>();
            int current_id = -1;
            int current_handle = -1;
            bool add_quotation_mark = true;
            bool is_vocalo1 = Common.Version.StartsWith("DSB2");
            bool is_vocalo2 = Common.Version.StartsWith("DSB3");
            foreach (var item in Events.iterator()) {
                current_id++;
                item.ID.value = current_id;
                // IconHandle
                if (item.ID.IconHandle != null) {
                    if (item.ID.IconHandle is IconHandle) {
                        IconHandle ish = (IconHandle)item.ID.IconHandle;
                        current_handle++;
                        VsqHandle handle_item = VsqHandle.castFromIconHandle(ish);
                        handle_item.Index = current_handle;
                        handle.Add(handle_item);
                        item.ID.IconHandle_index = current_handle;
                        if (is_vocalo1) {
                            VsqVoiceLanguage lang = VocaloSysUtil.getLanguageFromName(ish.IDS);
                            add_quotation_mark = lang == VsqVoiceLanguage.Japanese;
                        } else if (is_vocalo2) {
                            VsqVoiceLanguage lang = VocaloSysUtil.getLanguageFromName(ish.IDS);
                            add_quotation_mark = lang == VsqVoiceLanguage.Japanese;
                        }
                    }
                }
                // LyricHandle
                if (item.ID.LyricHandle != null) {
                    current_handle++;
                    VsqHandle handle_item = VsqHandle.castFromLyricHandle(item.ID.LyricHandle);
                    handle_item.Index = current_handle;
                    handle_item.addQuotationMark = add_quotation_mark;
                    handle.Add(handle_item);
                    item.ID.LyricHandle_index = current_handle;
                }
                // VibratoHandle
                if (item.ID.VibratoHandle != null) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.VibratoHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle.Add(handle_item);
                    item.ID.VibratoHandle_index = current_handle;
                }
                // NoteHeadHandle
                if (item.ID.NoteHeadHandle != null) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.NoteHeadHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle.Add(handle_item);
                    item.ID.NoteHeadHandle_index = current_handle;
                }
                // IconDynamicsHandle
                if (item.ID.IconDynamicsHandle != null) {
                    current_handle++;
                    VsqHandle handle_item = item.ID.IconDynamicsHandle.castToVsqHandle();
                    handle_item.Index = current_handle;
                    handle_item.setLength(item.ID.getLength());
                    handle.Add(handle_item);
                    item.ID.IconHandle_index = current_handle;
                }
            }
            return handle;
        }

        /// <summary>
        /// このインスタンスの内容を指定されたファイルに出力します。
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="encode"></param>
        public void print(ITextWriter sw, int eos, int start)
        {
            if (Common != null) {
                Common.write(sw);
            }
            if (master != null) {
                master.write(sw);
            }
            if (mixer != null) {
                mixer.write(sw);
            }
            List<VsqHandle> handle = writeEventList(sw, eos);
            foreach (var item in Events.iterator()) {
                item.write(sw);
            }
            for (int i = 0; i < handle.Count; i++) {
                handle[i].write(sw);
            }
            string version = Common.Version;
            if (PIT.size() > 0) {
                PIT.print(sw, start, "[PitchBendBPList]");
            }
            if (PBS.size() > 0) {
                PBS.print(sw, start, "[PitchBendSensBPList]");
            }
            if (DYN.size() > 0) {
                DYN.print(sw, start, "[DynamicsBPList]");
            }
            if (BRE.size() > 0) {
                BRE.print(sw, start, "[EpRResidualBPList]");
            }
            if (BRI.size() > 0) {
                BRI.print(sw, start, "[EpRESlopeBPList]");
            }
            if (CLE.size() > 0) {
                CLE.print(sw, start, "[EpRESlopeDepthBPList]");
            }
            if (version.StartsWith("DSB2")) {
                if (harmonics.size() > 0) {
                    harmonics.print(sw, start, "[EpRSineBPList]");
                }
                if (fx2depth.size() > 0) {
                    fx2depth.print(sw, start, "[VibTremDepthBPList]");
                }

                if (reso1FreqBPList.size() > 0) {
                    reso1FreqBPList.print(sw, start, "[Reso1FreqBPList]");
                }
                if (reso2FreqBPList.size() > 0) {
                    reso2FreqBPList.print(sw, start, "[Reso2FreqBPList]");
                }
                if (reso3FreqBPList.size() > 0) {
                    reso3FreqBPList.print(sw, start, "[Reso3FreqBPList]");
                }
                if (reso4FreqBPList.size() > 0) {
                    reso4FreqBPList.print(sw, start, "[Reso4FreqBPList]");
                }

                if (reso1BWBPList.size() > 0) {
                    reso1BWBPList.print(sw, start, "[Reso1BWBPList]");
                }
                if (reso2BWBPList.size() > 0) {
                    reso2BWBPList.print(sw, start, "[Reso2BWBPList]");
                }
                if (reso3BWBPList.size() > 0) {
                    reso3BWBPList.print(sw, start, "[Reso3BWBPList]");
                }
                if (reso4BWBPList.size() > 0) {
                    reso4BWBPList.print(sw, start, "[Reso4BWBPList]");
                }

                if (reso1AmpBPList.size() > 0) {
                    reso1AmpBPList.print(sw, start, "[Reso1AmpBPList]");
                }
                if (reso2AmpBPList.size() > 0) {
                    reso2AmpBPList.print(sw, start, "[Reso2AmpBPList]");
                }
                if (reso3AmpBPList.size() > 0) {
                    reso3AmpBPList.print(sw, start, "[Reso3AmpBPList]");
                }
                if (reso4AmpBPList.size() > 0) {
                    reso4AmpBPList.print(sw, start, "[Reso4AmpBPList]");
                }
            }

            if (GEN.size() > 0) {
                GEN.print(sw, start, "[GenderFactorBPList]");
            }
            if (POR.size() > 0) {
                POR.print(sw, start, "[PortamentoTimingBPList]");
            }
            if (version.StartsWith("DSB3")) {
                if (OPE.size() > 0) {
                    OPE.print(sw, start, "[OpeningBPList]");
                }
            }
        }

        private List<VsqHandle> writeEventListCor(ITextWriter writer, int eos)
        {
            List<VsqHandle> handles = buildHandleList();
            writer.writeLine("[EventList]");
            List<VsqEvent> temp = new List<VsqEvent>();
            foreach (var @event in Events.iterator()) {
                temp.Add(@event);
            }
            temp.Sort();
            int i = 0;
            while (i < temp.Count) {
                VsqEvent item = temp[i];
                if (!item.ID.Equals(VsqID.EOS)) {
                    string ids = "ID#" + PortUtil.formatDecimal("0000", item.ID.value);
                    int clock = temp[i].Clock;
                    while (i + 1 < temp.Count && clock == temp[i + 1].Clock) {
                        i++;
                        ids += ",ID#" + PortUtil.formatDecimal("0000", temp[i].ID.value);
                    }
                    writer.writeLine(clock + "=" + ids);
                }
                i++;
            }
            writer.writeLine(eos + "=EOS");
            return handles;
        }

        public List<VsqHandle> writeEventList(ITextWriter sw, int eos)
        {
            return writeEventListCor(sw, eos);
        }

        public List<VsqHandle> writeEventList(StreamWriter stream_writer, int eos)
        {
            return writeEventListCor(new WrappedStreamWriter(stream_writer), eos);
        }

        /// <summary>
        /// 何も無いVsqMetaTextを構築する。これは、Master Track用のMetaTextとしてのみ使用されるべき
        /// </summary>
        public VsqMetaText()
        {
        }

        /// <summary>
        /// 最初のトラック以外の一般のメタテキストを構築。(Masterが作られない)
        /// </summary>
        public VsqMetaText(string name, string singer)
            :
 this(name, 0, singer, false)
        {
        }

        /// <summary>
        /// 最初のトラックのメタテキストを構築。(Masterが作られる)
        /// </summary>
        /// <param name="pre_measure"></param>
        public VsqMetaText(string name, string singer, int pre_measure)
            :
 this(name, pre_measure, singer, true)
        {
        }

        private VsqMetaText(string name, int pre_measure, string singer, bool is_first_track)
        {
            Common = new VsqCommon(name, 179, 181, 123, 1, 1);
            PIT = new VsqBPList("pit", 0, -8192, 8191);
            PBS = new VsqBPList("pbs", 2, 0, 24);
            DYN = new VsqBPList("dyn", 64, 0, 127);
            BRE = new VsqBPList("bre", 0, 0, 127);
            BRI = new VsqBPList("bri", 64, 0, 127);
            CLE = new VsqBPList("cle", 0, 0, 127);
            reso1FreqBPList = new VsqBPList("reso1freq", 64, 0, 127);
            reso2FreqBPList = new VsqBPList("reso2freq", 64, 0, 127);
            reso3FreqBPList = new VsqBPList("reso3freq", 64, 0, 127);
            reso4FreqBPList = new VsqBPList("reso4freq", 64, 0, 127);
            reso1BWBPList = new VsqBPList("reso1bw", 64, 0, 127);
            reso2BWBPList = new VsqBPList("reso2bw", 64, 0, 127);
            reso3BWBPList = new VsqBPList("reso3bw", 64, 0, 127);
            reso4BWBPList = new VsqBPList("reso4bw", 64, 0, 127);
            reso1AmpBPList = new VsqBPList("reso1amp", 64, 0, 127);
            reso2AmpBPList = new VsqBPList("reso2amp", 64, 0, 127);
            reso3AmpBPList = new VsqBPList("reso3amp", 64, 0, 127);
            reso4AmpBPList = new VsqBPList("reso4amp", 64, 0, 127);
            harmonics = new VsqBPList("harmonics", 64, 0, 127);
            fx2depth = new VsqBPList("fx2depth", 64, 0, 127);
            GEN = new VsqBPList("gen", 64, 0, 127);
            POR = new VsqBPList("por", 64, 0, 127);
            OPE = new VsqBPList("ope", 127, 0, 127);
            if (is_first_track) {
                master = new VsqMaster(pre_measure);
            } else {
                master = null;
            }
            Events = new VsqEventList();
            VsqID id = new VsqID(0);
            id.type = VsqIDType.Singer;
            IconHandle ish = new IconHandle();
            ish.IconID = "$07010000";
            ish.IDS = singer;
            ish.Original = 0;
            ish.Caption = "";
            ish.setLength(1);
            ish.Language = 0;
            ish.Program = 0;
            id.IconHandle = ish;
            Events.add(new VsqEvent(0, id));
        }

        public VsqMetaText(TextStream sr)
        {
            List<ValuePair<int, int>> t_event_list = new List<ValuePair<int, int>>();
            SortedDictionary<int, VsqID> __id = new SortedDictionary<int, VsqID>();
            SortedDictionary<int, VsqHandle> __handle = new SortedDictionary<int, VsqHandle>();
            PIT = new VsqBPList("pit", 0, -8192, 8191);
            PBS = new VsqBPList("pbs", 2, 0, 24);
            DYN = new VsqBPList("dyn", 64, 0, 127);
            BRE = new VsqBPList("bre", 0, 0, 127);
            BRI = new VsqBPList("bri", 64, 0, 127);
            CLE = new VsqBPList("cle", 0, 0, 127);
            reso1FreqBPList = new VsqBPList("reso1freq", 64, 0, 127);
            reso2FreqBPList = new VsqBPList("reso2freq", 64, 0, 127);
            reso3FreqBPList = new VsqBPList("reso3freq", 64, 0, 127);
            reso4FreqBPList = new VsqBPList("reso4freq", 64, 0, 127);
            reso1BWBPList = new VsqBPList("reso1bw", 64, 0, 127);
            reso2BWBPList = new VsqBPList("reso2bw", 64, 0, 127);
            reso3BWBPList = new VsqBPList("reso3bw", 64, 0, 127);
            reso4BWBPList = new VsqBPList("reso4bw", 64, 0, 127);
            reso1AmpBPList = new VsqBPList("reso1amp", 64, 0, 127);
            reso2AmpBPList = new VsqBPList("reso2amp", 64, 0, 127);
            reso3AmpBPList = new VsqBPList("reso3amp", 64, 0, 127);
            reso4AmpBPList = new VsqBPList("reso4amp", 64, 0, 127);
            harmonics = new VsqBPList("harmonics", 64, 0, 127);
            fx2depth = new VsqBPList("fx2depth", 64, 0, 127);
            GEN = new VsqBPList("gen", 64, 0, 127);
            POR = new VsqBPList("por", 64, 0, 127);
            OPE = new VsqBPList("ope", 127, 0, 127);

            ByRef<string> last_line = new ByRef<string>(sr.readLine());
            while (true) {
                #region "TextMemoryStreamから順次読込み"
                if (last_line.value.Length == 0) {
                    break;
                }
                if (last_line.value == "[Common]") {
                    Common = new VsqCommon(sr, last_line);
                } else if (last_line.value == "[Master]") {
                    master = new VsqMaster(sr, last_line);
                } else if (last_line.value == "[Mixer]") {
                    mixer = new VsqMixer(sr, last_line);
                } else if (last_line.value == "[EventList]") {
                    last_line.value = sr.readLine();
                    while (!last_line.value.StartsWith("[")) {
                        string[] spl2 = PortUtil.splitString(last_line.value, new char[] { '=' });
                        int clock = int.Parse(spl2[0]);
                        int id_number = -1;
                        if (spl2[1] != "EOS") {
                            string[] ids = PortUtil.splitString(spl2[1], ',');
                            for (int i = 0; i < ids.Length; i++) {
                                string[] spl3 = PortUtil.splitString(ids[i], new char[] { '#' });
                                id_number = int.Parse(spl3[1]);
                                t_event_list.Add(new ValuePair<int, int>(clock, id_number));
                            }
                        } else {
                            t_event_list.Add(new ValuePair<int, int>(clock, -1));
                        }
                        if (!sr.ready()) {
                            break;
                        } else {
                            last_line.value = sr.readLine();
                        }
                    }
                } else if (last_line.value == "[PitchBendBPList]") {
                    last_line.value = PIT.appendFromText(sr);
                } else if (last_line.value == "[PitchBendSensBPList]") {
                    last_line.value = PBS.appendFromText(sr);
                } else if (last_line.value == "[DynamicsBPList]") {
                    last_line.value = DYN.appendFromText(sr);
                } else if (last_line.value == "[EpRResidualBPList]") {
                    last_line.value = BRE.appendFromText(sr);
                } else if (last_line.value == "[EpRESlopeBPList]") {
                    last_line.value = BRI.appendFromText(sr);
                } else if (last_line.value == "[EpRESlopeDepthBPList]") {
                    last_line.value = CLE.appendFromText(sr);
                } else if (last_line.value == "[EpRSineBPList]") {
                    last_line.value = harmonics.appendFromText(sr);
                } else if (last_line.value == "[VibTremDepthBPList]") {
                    last_line.value = fx2depth.appendFromText(sr);
                } else if (last_line.value == "[Reso1FreqBPList]") {
                    last_line.value = reso1FreqBPList.appendFromText(sr);
                } else if (last_line.value == "[Reso2FreqBPList]") {
                    last_line.value = reso2FreqBPList.appendFromText(sr);
                } else if (last_line.value == "[Reso3FreqBPList]") {
                    last_line.value = reso3FreqBPList.appendFromText(sr);
                } else if (last_line.value == "[Reso4FreqBPList]") {
                    last_line.value = reso4FreqBPList.appendFromText(sr);
                } else if (last_line.value == "[Reso1BWBPList]") {
                    last_line.value = reso1BWBPList.appendFromText(sr);
                } else if (last_line.value == "[Reso2BWBPList]") {
                    last_line.value = reso2BWBPList.appendFromText(sr);
                } else if (last_line.value == "[Reso3BWBPList]") {
                    last_line.value = reso3BWBPList.appendFromText(sr);
                } else if (last_line.value == "[Reso4BWBPList]") {
                    last_line.value = reso4BWBPList.appendFromText(sr);
                } else if (last_line.value == "[Reso1AmpBPList]") {
                    last_line.value = reso1AmpBPList.appendFromText(sr);
                } else if (last_line.value == "[Reso2AmpBPList]") {
                    last_line.value = reso2AmpBPList.appendFromText(sr);
                } else if (last_line.value == "[Reso3AmpBPList]") {
                    last_line.value = reso3AmpBPList.appendFromText(sr);
                } else if (last_line.value == "[Reso4AmpBPList]") {
                    last_line.value = reso4AmpBPList.appendFromText(sr);
                } else if (last_line.value == "[GenderFactorBPList]") {
                    last_line.value = GEN.appendFromText(sr);
                } else if (last_line.value == "[PortamentoTimingBPList]") {
                    last_line.value = POR.appendFromText(sr);
                } else if (last_line.value == "[OpeningBPList]") {
                    last_line.value = OPE.appendFromText(sr);
                } else {
                    string buffer = last_line.value;
                    buffer = buffer.Replace("[", "");
                    buffer = buffer.Replace("]", "");
#if DEBUG
                    sout.println("VsqMetaText#.ctor; buffer=" + buffer);
#endif
                    string[] spl = PortUtil.splitString(buffer, new char[] { '#' });
                    int index = int.Parse(spl[1]);
                    if (last_line.value.StartsWith("[ID#")) {
                        __id[index] = new VsqID(sr, index, last_line);
                    } else if (last_line.value.StartsWith("[h#")) {
                        __handle[index] = new VsqHandle(sr, index, last_line);
                    }
                }
                #endregion

                if (!sr.ready()) {
                    break;
                }
            }

            // まずhandleをidに埋め込み
            int c = __id.Count;
            for (int i = 0; i < c; i++) {
                VsqID id = __id[i];
                if (__handle.ContainsKey(id.IconHandle_index)) {
                    if (id.type == VsqIDType.Singer) {
                        id.IconHandle = __handle[id.IconHandle_index].castToIconHandle();
                    } else if (id.type == VsqIDType.Aicon) {
                        id.IconDynamicsHandle = __handle[id.IconHandle_index].castToIconDynamicsHandle();
                    }
                }
                if (__handle.ContainsKey(id.LyricHandle_index)) {
                    id.LyricHandle = __handle[id.LyricHandle_index].castToLyricHandle();
                }
                if (__handle.ContainsKey(id.VibratoHandle_index)) {
                    id.VibratoHandle = __handle[id.VibratoHandle_index].castToVibratoHandle();
                }
                if (__handle.ContainsKey(id.NoteHeadHandle_index)) {
                    id.NoteHeadHandle = __handle[id.NoteHeadHandle_index].castToNoteHeadHandle();
                }
            }

            // idをeventListに埋め込み
            Events = new VsqEventList();
            int count = 0;
            for (int i = 0; i < t_event_list.Count; i++) {
                ValuePair<int, int> item = t_event_list[i];
                int clock = item.getKey();
                int id_number = item.getValue();
                if (__id.ContainsKey(id_number)) {
                    count++;
                    Events.add(new VsqEvent(clock, (VsqID)__id[id_number].clone()), count);
                }
            }
            Events.sort();

            if (Common == null) {
                Common = new VsqCommon();
            }
        }
    }

}
