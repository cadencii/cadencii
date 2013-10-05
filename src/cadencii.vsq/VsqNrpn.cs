/*
 * VsqNrpn.cs
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

    public class VsqNrpn : IComparable<VsqNrpn>
    {
        public int Clock;
        public int Nrpn;
        public byte DataMsb;
        public byte DataLsb;
        public bool DataLsbSpecified;
        public bool msbOmitRequired;
        private List<VsqNrpn> m_list;

        public VsqNrpn(int clock, int nrpn, byte data_msb)
        {
            Clock = clock;
            Nrpn = nrpn;
            DataMsb = data_msb;
            DataLsb = 0x0;
            DataLsbSpecified = false;
            msbOmitRequired = false;
            m_list = new List<VsqNrpn>();
        }

        public VsqNrpn(int clock, int nrpn, byte data_msb, byte data_lsb)
        {
            Clock = clock;
            Nrpn = nrpn;
            DataMsb = data_msb;
            DataLsb = data_lsb;
            DataLsbSpecified = true;
            msbOmitRequired = false;
            m_list = new List<VsqNrpn>();
        }

        public VsqNrpn[] expand()
        {
            List<VsqNrpn> ret = new List<VsqNrpn>();
            if (DataLsbSpecified) {
                VsqNrpn v = new VsqNrpn(Clock, Nrpn, DataMsb, DataLsb);
                v.msbOmitRequired = msbOmitRequired;
                ret.Add(v);
            } else {
                VsqNrpn v = new VsqNrpn(Clock, Nrpn, DataMsb);
                v.msbOmitRequired = msbOmitRequired;
                ret.Add(v);
            }
            for (int i = 0; i < m_list.Count; i++) {
                ret.AddRange(new List<VsqNrpn>(m_list[i].expand()));
            }
            return ret.ToArray();
        }

        public static List<VsqNrpn> sort(List<VsqNrpn> list)
        {
            List<VsqNrpn> ret = new List<VsqNrpn>();
            list.Sort();
            int list_size = list.Count;
            if (list_size >= 2) {
                List<VsqNrpn> work = new List<VsqNrpn>(); //workには、clockが同じNRPNだけが入る
                int last_clock = list[0].Clock;
                work.Add(list[0]);
                for (int i = 1; i < list_size; i++) {
                    VsqNrpn itemi = list[i];
                    if (itemi.Clock == last_clock) {
                        work.Add(itemi);
                    } else {
                        // まずworkを並べ替え
                        last_clock = itemi.Clock;
                        bool changed = true;
                        int work_size = work.Count;
                        while (changed) {
                            changed = false;
                            for (int j = 0; j < work_size - 1; j++) {
                                VsqNrpn itemj = work[j];
                                VsqNrpn itemjn = work[j + 1];
                                int nrpn_msb0 = (itemj.Nrpn >> 8) & 0xff;
                                int nrpn_msb1 = (itemjn.Nrpn >> 8) & 0xff;
                                if (nrpn_msb1 > nrpn_msb0) {
                                    VsqNrpn buf = itemj;
                                    work[j] = itemjn;
                                    work[j + 1] = buf;
                                    changed = true;
                                }
                            }
                        }
                        for (int j = 0; j < work_size; j++) {
                            ret.Add(work[j]);
                        }
                        work.Clear();
                        work.Add(list[i]);
                    }
                }
                for (int j = 0; j < work.Count; j++) {
                    ret.Add(work[j]);
                }
            } else {
                for (int i = 0; i < list.Count; i++) {
                    ret.Add(list[i]);
                }
            }
            return ret;
        }

        public static VsqNrpn[] merge(VsqNrpn[] src1, VsqNrpn[] src2)
        {
            List<VsqNrpn> ret = new List<VsqNrpn>();
            for (int i = 0; i < src1.Length; i++) {
                ret.Add(src1[i]);
            }
            for (int i = 0; i < src2.Length; i++) {
                ret.Add(src2[i]);
            }
            ret.Sort();
            return ret.ToArray();
        }

        public static NrpnData[] convert(VsqNrpn[] source)
        {
            int nrpn = source[0].Nrpn;
            byte msb = (byte)(nrpn >> 8);
            byte lsb = (byte)(nrpn - (nrpn << 8));
            List<NrpnData> ret = new List<NrpnData>();
            ret.Add(new NrpnData(source[0].Clock, (byte)0x63, msb));
            ret.Add(new NrpnData(source[0].Clock, (byte)0x62, lsb));
            ret.Add(new NrpnData(source[0].Clock, (byte)0x06, source[0].DataMsb));
            if (source[0].DataLsbSpecified) {
                ret.Add(new NrpnData(source[0].Clock, (byte)0x26, source[0].DataLsb));
            }
            for (int i = 1; i < source.Length; i++) {
                VsqNrpn item = source[i];
                int tnrpn = item.Nrpn;
                msb = (byte)(tnrpn >> 8);
                lsb = (byte)(tnrpn - (tnrpn << 8));
                if (item.msbOmitRequired) {
                    ret.Add(new NrpnData(item.Clock, (byte)0x62, lsb));
                    ret.Add(new NrpnData(item.Clock, (byte)0x06, item.DataMsb));
                    if (item.DataLsbSpecified) {
                        ret.Add(new NrpnData(item.Clock, (byte)0x26, item.DataLsb));
                    }
                } else {
                    ret.Add(new NrpnData(item.Clock, (byte)0x63, msb));
                    ret.Add(new NrpnData(item.Clock, (byte)0x62, lsb));
                    ret.Add(new NrpnData(item.Clock, (byte)0x06, item.DataMsb));
                    if (item.DataLsbSpecified) {
                        ret.Add(new NrpnData(item.Clock, (byte)0x26, item.DataLsb));
                    }
                }
            }
            return ret.ToArray();
        }

        public int compareTo(VsqNrpn item)
        {
            return Clock - item.Clock;
        }

        public int CompareTo(VsqNrpn item)
        {
            return compareTo(item);
        }

        public void append(int nrpn, byte data_msb)
        {
            m_list.Add(new VsqNrpn(Clock, nrpn, data_msb));
        }

        public void append(int nrpn, byte data_msb, byte data_lsb)
        {
            m_list.Add(new VsqNrpn(Clock, nrpn, data_msb, data_lsb));
        }

        public void append(int nrpn, byte data_msb, bool msb_omit_required)
        {
            VsqNrpn v = new VsqNrpn(Clock, nrpn, data_msb);
            v.msbOmitRequired = msb_omit_required;
            m_list.Add(v);
        }

        public void append(int nrpn, byte data_msb, byte data_lsb, bool msb_omit_required)
        {
            VsqNrpn v = new VsqNrpn(Clock, nrpn, data_msb, data_lsb);
            v.msbOmitRequired = msb_omit_required;
            m_list.Add(v);
        }
    }

}
