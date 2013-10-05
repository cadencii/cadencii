/*
 * VsqHandle.cs
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
using cadencii.java.io;
using cadencii.java.util;

namespace cadencii.vsq
{

    /// <summary>
    /// ハンドルを取り扱います。ハンドルにはLyricHandle、VibratoHandle、IconHandleおよびNoteHeadHandleがある
    /// </summary>
    [Serializable]
    public class VsqHandle
    {
        public VsqHandleType m_type;
        public int Index;
        public string IconID = "";
        public string IDS = "";
        public Lyric L0;
        public List<Lyric> Trailing = new List<Lyric>();
        public int Original;
        public string Caption = "";
        public int Length;
        public int StartDepth;
        public VibratoBPList DepthBP;
        public int StartRate;
        public VibratoBPList RateBP;
        public int Language;
        public int Program;
        public int Duration;
        public int Depth;
        public int StartDyn;
        public int EndDyn;
        public VibratoBPList DynBP;
        /// <summary>
        /// 歌詞・発音記号列の前後にクォーテーションマークを付けるかどうか
        /// </summary>
        public bool addQuotationMark = true;

        public VsqHandle()
        {
        }

        public int getLength()
        {
            return Length;
        }

        public void setLength(int value)
        {
            Length = value;
        }

        public LyricHandle castToLyricHandle()
        {
            LyricHandle ret = new LyricHandle();
            ret.L0 = L0;
            ret.Index = Index;
            ret.Trailing = Trailing;
            return ret;
        }

        public VibratoHandle castToVibratoHandle()
        {
            VibratoHandle ret = new VibratoHandle();
            ret.Index = Index;
            ret.setCaption(Caption);
            ret.setDepthBP((VibratoBPList)DepthBP.clone());
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Index = Index;
            ret.setLength(Length);
            ret.Original = Original;
            ret.setRateBP((VibratoBPList)RateBP.clone());
            ret.setStartDepth(StartDepth);
            ret.setStartRate(StartRate);
            return ret;
        }

        public IconHandle castToIconHandle()
        {
            IconHandle ret = new IconHandle();
            ret.Index = Index;
            ret.Caption = Caption;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Index = Index;
            ret.Language = Language;
            ret.setLength(Length);
            ret.Original = Original;
            ret.Program = Program;
            return ret;
        }

        public NoteHeadHandle castToNoteHeadHandle()
        {
            NoteHeadHandle ret = new NoteHeadHandle();
            ret.setCaption(Caption);
            ret.setDepth(Depth);
            ret.setDuration(Duration);
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.setLength(getLength());
            ret.Original = Original;
            return ret;
        }

        public IconDynamicsHandle castToIconDynamicsHandle()
        {
            IconDynamicsHandle ret = new IconDynamicsHandle();
            ret.IDS = IDS;
            ret.IconID = IconID;
            ret.Original = Original;
            ret.setCaption(Caption);
            ret.setDynBP(DynBP);
            ret.setEndDyn(EndDyn);
            ret.setLength(getLength());
            ret.setStartDyn(StartDyn);
            return ret;
        }

        public static VsqHandle castFromLyricHandle(LyricHandle handle)
        {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Lyric;
            ret.L0 = (Lyric)handle.L0.clone();
            ret.Trailing = handle.Trailing;
            ret.Index = handle.Index;
            return ret;
        }

        /// <summary>
        /// 歌手設定のインスタンスを、VsqHandleに型キャストします。
        /// </summary>
        /// <returns></returns>
        public static VsqHandle castFromIconHandle(IconHandle handle)
        {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Singer;
            ret.Caption = handle.Caption;
            ret.IconID = handle.IconID;
            ret.IDS = handle.IDS;
            ret.Index = handle.Index;
            ret.Language = handle.Language;
            ret.setLength(handle.getLength());
            ret.Program = handle.Program;
            return ret;
        }

        /// <summary>
        /// インスタンスをストリームに書き込みます。
        /// encode=trueの場合、2バイト文字をエンコードして出力します。
        /// </summary>
        /// <param name="sw">書き込み対象</param>
        public void write(ITextWriter sw)
        {
            sw.writeLine(this.toString());
        }

        public void write(StreamWriter sw)
        {
            write(new WrappedStreamWriter(sw));
        }

        /// <summary>
        /// FileStreamから読み込みながらコンストラクト
        /// </summary>
        /// <param name="sr">読み込み対象</param>
        public VsqHandle(TextStream sr, int value, ByRef<string> last_line)
        {
            this.Index = value;
            string[] spl;
            string[] spl2;

            // default値で梅
            m_type = VsqHandleType.Vibrato;
            IconID = "";
            IDS = "normal";
            L0 = new Lyric("");
            Original = 0;
            Caption = "";
            Length = 0;
            StartDepth = 0;
            DepthBP = null;
            StartRate = 0;
            RateBP = null;
            Language = 0;
            Program = 0;
            Duration = 0;
            Depth = 64;

            string tmpDepthBPX = "";
            string tmpDepthBPY = "";
            string tmpDepthBPNum = "";

            string tmpRateBPX = "";
            string tmpRateBPY = "";
            string tmpRateBPNum = "";

            string tmpDynBPX = "";
            string tmpDynBPY = "";
            string tmpDynBPNum = "";

            // "["にぶち当たるまで読込む
            last_line.value = sr.readLine().ToString();
            while (!last_line.value.StartsWith("[")) {
                spl = PortUtil.splitString(last_line.value, new char[] { '=' });
                string search = spl[0];
                if (search.Equals("Language")) {
                    m_type = VsqHandleType.Singer;
                    Language = int.Parse(spl[1]);
                } else if (search.Equals("Program")) {
                    Program = int.Parse(spl[1]);
                } else if (search.Equals("IconID")) {
                    IconID = spl[1];
                } else if (search.Equals("IDS")) {
                    IDS = spl[1];
                } else if (search.Equals("Original")) {
                    Original = int.Parse(spl[1]);
                } else if (search.Equals("Caption")) {
                    Caption = spl[1];
                    for (int i = 2; i < spl.Length; i++) {
                        Caption += "=" + spl[i];
                    }
                } else if (search.Equals("Length")) {
                    Length = int.Parse(spl[1]);
                } else if (search.Equals("StartDepth")) {
                    StartDepth = int.Parse(spl[1]);
                } else if (search.Equals("DepthBPNum")) {
                    tmpDepthBPNum = spl[1];
                } else if (search.Equals("DepthBPX")) {
                    tmpDepthBPX = spl[1];
                } else if (search.Equals("DepthBPY")) {
                    tmpDepthBPY = spl[1];
                } else if (search.Equals("StartRate")) {
                    m_type = VsqHandleType.Vibrato;
                    StartRate = int.Parse(spl[1]);
                } else if (search.Equals("RateBPNum")) {
                    tmpRateBPNum = spl[1];
                } else if (search.Equals("RateBPX")) {
                    tmpRateBPX = spl[1];
                } else if (search.Equals("RateBPY")) {
                    tmpRateBPY = spl[1];
                } else if (search.Equals("Duration")) {
                    m_type = VsqHandleType.NoteHeadHandle;
                    Duration = int.Parse(spl[1]);
                } else if (search.Equals("Depth")) {
                    Depth = int.Parse(spl[1]);
                } else if (search.Equals("StartDyn")) {
                    m_type = VsqHandleType.DynamicsHandle;
                    StartDyn = int.Parse(spl[1]);
                } else if (search.Equals("EndDyn")) {
                    m_type = VsqHandleType.DynamicsHandle;
                    EndDyn = int.Parse(spl[1]);
                } else if (search.Equals("DynBPNum")) {
                    tmpDynBPNum = spl[1];
                } else if (search.Equals("DynBPX")) {
                    tmpDynBPX = spl[1];
                } else if (search.Equals("DynBPY")) {
                    tmpDynBPY = spl[1];
                } else if (search.StartsWith("L") && PortUtil.getStringLength(search) >= 2) {
                    string num = search.Substring(1);
                    ByRef<int> vals = new ByRef<int>(0);
                    if (PortUtil.tryParseInt(num, vals)) {
                        Lyric lyric = new Lyric(spl[1]);
                        m_type = VsqHandleType.Lyric;
                        int index = vals.value;
                        if (index == 0) {
                            L0 = lyric;
                        } else {
                            if (Trailing.Count < index) {
                                for (int i = Trailing.Count; i < index; i++) {
                                    Trailing.Add(new Lyric("a", "a"));
                                }
                            }
                            Trailing[index - 1] = lyric;
                        }
                    }
                }
                if (!sr.ready()) {
                    break;
                }
                last_line.value = sr.readLine().ToString();
            }

            // RateBPX, RateBPYの設定
            if (m_type == VsqHandleType.Vibrato) {
                if (!tmpRateBPNum.Equals("")) {
                    RateBP = new VibratoBPList(tmpRateBPNum, tmpRateBPX, tmpRateBPY);
                } else {
                    RateBP = new VibratoBPList();
                }

                // DepthBPX, DepthBPYの設定
                if (!tmpDepthBPNum.Equals("")) {
                    DepthBP = new VibratoBPList(tmpDepthBPNum, tmpDepthBPX, tmpDepthBPY);
                } else {
                    DepthBP = new VibratoBPList();
                }
            } else {
                DepthBP = new VibratoBPList();
                RateBP = new VibratoBPList();
            }

            if (!tmpDynBPNum.Equals("")) {
                DynBP = new VibratoBPList(tmpDynBPNum, tmpDynBPX, tmpDynBPY);
            } else {
                DynBP = new VibratoBPList();
            }
        }

        /// <summary>
        /// ハンドル指定子（例えば"h#0123"という文字列）からハンドル番号を取得します
        /// </summary>
        /// <param name="_string">ハンドル指定子</param>
        /// <returns>ハンドル番号</returns>
        public static int HandleIndexFromString(string _string)
        {
            string[] spl = PortUtil.splitString(_string, new char[] { '#' });
            return int.Parse(spl[1]);
        }

        /// <summary>
        /// インスタンスをテキストファイルに出力します
        /// </summary>
        /// <param name="sw">出力先</param>
        public void print(StreamWriter sw)
        {
            string result = toString();
            sw.WriteLine(result);
        }

        /// <summary>
        /// インスタンスをコンソール画面に出力します
        /// </summary>
        private void print()
        {
            string result = toString();
            sout.println(result);
        }

        /// <summary>
        /// インスタンスを文字列に変換します
        /// </summary>
        /// <returns>インスタンスを変換した文字列</returns>
        public string toString()
        {
            string result = "";
            result += "[h#" + PortUtil.formatDecimal("0000", Index) + "]";
            if (m_type == VsqHandleType.Lyric) {
                result += "\n" + "L0=" + L0.toString(addQuotationMark);
                int c = Trailing.Count;
                for (int i = 0; i < c; i++) {
                    result += "\n" + "L" + (i + 1) + "=" + Trailing[i].toString(addQuotationMark);
                }
            } else if (m_type == VsqHandleType.Vibrato) {
                result += "\n" + "IconID=" + IconID + "\n";
                result += "IDS=" + IDS + "\n";
                result += "Original=" + Original + "\n";
                result += "Caption=" + Caption + "\n";
                result += "Length=" + Length + "\n";
                result += "StartDepth=" + StartDepth + "\n";
                result += "DepthBPNum=" + DepthBP.getCount() + "\n";
                if (DepthBP.getCount() > 0) {
                    result += "DepthBPX=" + PortUtil.formatDecimal("0.000000", DepthBP.getElement(0).X);
                    for (int i = 1; i < DepthBP.getCount(); i++) {
                        result += "," + PortUtil.formatDecimal("0.000000", DepthBP.getElement(i).X);
                    }
                    result += "\n" + "DepthBPY=" + DepthBP.getElement(0).Y;
                    for (int i = 1; i < DepthBP.getCount(); i++) {
                        result += "," + DepthBP.getElement(i).Y;
                    }
                    result += "\n";
                }
                result += "StartRate=" + StartRate + "\n";
                result += "RateBPNum=" + RateBP.getCount();
                if (RateBP.getCount() > 0) {
                    result += "\n" + "RateBPX=" + PortUtil.formatDecimal("0.000000", RateBP.getElement(0).X);
                    for (int i = 1; i < RateBP.getCount(); i++) {
                        result += "," + PortUtil.formatDecimal("0.000000", RateBP.getElement(i).X);
                    }
                    result += "\n" + "RateBPY=" + RateBP.getElement(0).Y;
                    for (int i = 1; i < RateBP.getCount(); i++) {
                        result += "," + RateBP.getElement(i).Y;
                    }
                }
            } else if (m_type == VsqHandleType.Singer) {
                result += "\n" + "IconID=" + IconID + "\n";
                result += "IDS=" + IDS + "\n";
                result += "Original=" + Original + "\n";
                result += "Caption=" + Caption + "\n";
                result += "Length=" + Length + "\n";
                result += "Language=" + Language + "\n";
                result += "Program=" + Program;
            } else if (m_type == VsqHandleType.NoteHeadHandle) {
                result += "\n" + "IconID=" + IconID + "\n";
                result += "IDS=" + IDS + "\n";
                result += "Original=" + Original + "\n";
                result += "Caption=" + Caption + "\n";
                result += "Length=" + Length + "\n";
                result += "Duration=" + Duration + "\n";
                result += "Depth=" + Depth;
            } else if (m_type == VsqHandleType.DynamicsHandle) {
                result += "\n" + "IconID=" + IconID + "\n";
                result += "IDS=" + IDS + "\n";
                result += "Original=" + Original + "\n";
                result += "Caption=" + Caption + "\n";
                result += "StartDyn=" + StartDyn + "\n";
                result += "EndDyn=" + EndDyn + "\n";
                result += "Length=" + Length + "\n";
                if (DynBP != null) {
                    if (DynBP.getCount() <= 0) {
                        result += "DynBPNum=0";
                    } else {
                        result += "DynBPX=" + PortUtil.formatDecimal("0.000000", DynBP.getElement(0).X);
                        int c = DynBP.getCount();
                        for (int i = 1; i < c; i++) {
                            result += "," + PortUtil.formatDecimal("0.000000", DynBP.getElement(i).X);
                        }
                        result += "\n" + "DynBPY=" + DynBP.getElement(0).Y;
                        for (int i = 1; i < c; i++) {
                            result += "," + DynBP.getElement(i).Y;
                        }
                    }
                } else {
                    result += "DynBPNum=0";
                }
            }
            return result;
        }

        public override string ToString()
        {
            return toString();
        }

    }

}
