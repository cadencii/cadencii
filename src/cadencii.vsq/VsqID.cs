/*
 * VsqID.cs
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
using cadencii;
using cadencii.java.io;

namespace cadencii.vsq
{

    /// <summary>
    /// メタテキストに埋め込まれるIDを表すクラス。
    /// </summary>
    [Serializable]
    public class VsqID : ICloneable
    {
        /// <summary>
        /// ミリ秒で表した、音符の最大長さ
        /// </summary>
        public const int MAX_NOTE_MILLISEC_LENGTH = 16383;
        private const int RISE_PORTAMENTO_FLAG = 0x1;
        private const int FALL_PORTAMENTO_FLAG = 0x2;
        internal int value;
        internal int IconHandle_index;
        internal int LyricHandle_index;
        internal int VibratoHandle_index;
        internal int NoteHeadHandle_index;
        public VsqIDType type;
        public IconHandle IconHandle;
        private int length;
        public int Note;
        public int Dynamics;
        public int PMBendDepth;
        public int PMBendLength;
        public int PMbPortamentoUse;
        public int DEMdecGainRate;
        public int DEMaccent;
        public LyricHandle LyricHandle;
        public VibratoHandle VibratoHandle;
        public int VibratoDelay;
        public NoteHeadHandle NoteHeadHandle;
        public int pMeanOnsetFirstNote = 0x0a;
        public int vMeanNoteTransition = 0x0c;
        public int d4mean = 0x18;
        public int pMeanEndingNote = 0x0c;
        public IconDynamicsHandle IconDynamicsHandle;

        public static VsqID EOS = new VsqID(-1);

        /// <summary>
        /// 下行形でポルタメントを付加するかどうかを取得する
        /// </summary>
        /// <returns>下行形でポルタメントを付加するかどうか</returns>
        public bool isFallPortamento()
        {
            return (PMbPortamentoUse & FALL_PORTAMENTO_FLAG) == FALL_PORTAMENTO_FLAG;
        }

        /// <summary>
        /// 下行形でポルタメントを付加するかどうかを設定する
        /// </summary>
        /// <param name="value">下行形でポルタメントを付加するかどうか</param>
        public void setFallPortamento(bool value)
        {
            if (value) {
                PMbPortamentoUse |= FALL_PORTAMENTO_FLAG;
            } else {
                PMbPortamentoUse &= ~FALL_PORTAMENTO_FLAG;
            }
        }

        /// <summary>
        /// 上行形でポルタメントを付加するかどうかを取得する
        /// </summary>
        /// <returns>上行形でポルタメントを付加するかどうか</returns>
        public bool isRisePortamento()
        {
            return (PMbPortamentoUse & RISE_PORTAMENTO_FLAG) == RISE_PORTAMENTO_FLAG;
        }

        /// <summary>
        /// 上行形でポルタメントを付加するかどうかを設定する
        /// </summary>
        /// <param name="value">上行形でポルタメントを付加するかどうか</param>
        public void setRisePortamento(bool value)
        {
            if (value) {
                PMbPortamentoUse |= RISE_PORTAMENTO_FLAG;
            } else {
                PMbPortamentoUse &= ~RISE_PORTAMENTO_FLAG;
            }
        }

        public int getLength()
        {
            return length;
        }

        public void setLength(int value)
        {
            length = value;
        }

        public int Length
        {
            get
            {
                return getLength();
            }
            set
            {
                setLength(value);
            }
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getXmlElementName(string name)
        {
            return name;
        }

        /// <summary>
        /// このインスタンスの簡易コピーを取得します。
        /// </summary>
        /// <returns>このインスタンスの簡易コピー</returns>
        public Object clone()
        {
            VsqID result = new VsqID(this.value);
            result.type = this.type;
            if (this.IconHandle != null) {
                result.IconHandle = (IconHandle)IconHandle.clone();
            }
            result.setLength(getLength());
            result.Note = this.Note;
            result.Dynamics = this.Dynamics;
            result.PMBendDepth = this.PMBendDepth;
            result.PMBendLength = this.PMBendLength;
            result.PMbPortamentoUse = this.PMbPortamentoUse;
            result.DEMdecGainRate = this.DEMdecGainRate;
            result.DEMaccent = this.DEMaccent;
            result.d4mean = this.d4mean;
            result.pMeanOnsetFirstNote = this.pMeanOnsetFirstNote;
            result.vMeanNoteTransition = this.vMeanNoteTransition;
            result.pMeanEndingNote = this.pMeanEndingNote;
            if (this.LyricHandle != null) {
                result.LyricHandle = (LyricHandle)this.LyricHandle.clone();
            }
            if (this.VibratoHandle != null) {
                result.VibratoHandle = (VibratoHandle)this.VibratoHandle.clone();
            }
            result.VibratoDelay = this.VibratoDelay;
            if (NoteHeadHandle != null) {
                result.NoteHeadHandle = (NoteHeadHandle)NoteHeadHandle.clone();
            }
            if (IconDynamicsHandle != null) {
                result.IconDynamicsHandle = (IconDynamicsHandle)IconDynamicsHandle.clone();
            }
            return result;
        }

        public object Clone()
        {
            return clone();
        }

        /// <summary>
        /// IDの番号（ID#****の****）を指定したコンストラクタ。
        /// </summary>
        /// <param name="a_value">IDの番号</param>
        public VsqID(int a_value)
        {
            value = a_value;
        }

        public VsqID()
            : this(0)
        {
        }

        /// <summary>
        /// テキストファイルからのコンストラクタ
        /// </summary>
        /// <param name="sr">読み込み対象</param>
        /// <param name="value"></param>
        /// <param name="last_line">読み込んだ最後の行が返されます</param>
        public VsqID(TextStream sr, int value, ByRef<string> last_line)
        {
            string[] spl;
            this.value = value;
            this.type = VsqIDType.Unknown;
            this.IconHandle_index = -2;
            this.LyricHandle_index = -1;
            this.VibratoHandle_index = -1;
            this.NoteHeadHandle_index = -1;
            this.setLength(0);
            this.Note = 0;
            this.Dynamics = 64;
            this.PMBendDepth = 8;
            this.PMBendLength = 0;
            this.PMbPortamentoUse = 0;
            this.DEMdecGainRate = 50;
            this.DEMaccent = 50;
            //this.LyricHandle_index = -2;
            //this.VibratoHandle_index = -2;
            this.VibratoDelay = 0;
            last_line.value = sr.readLine();
            while (!last_line.value.StartsWith("[")) {
                spl = PortUtil.splitString(last_line.value, new char[] { '=' });
                string search = spl[0];
                if (search.Equals("Type")) {
                    if (spl[1].Equals("Anote")) {
                        type = VsqIDType.Anote;
                    } else if (spl[1].Equals("Singer")) {
                        type = VsqIDType.Singer;
                    } else if (spl[1].Equals("Aicon")) {
                        type = VsqIDType.Aicon;
                    } else {
                        type = VsqIDType.Unknown;
                    }
                } else if (search.Equals("Length")) {
                    this.setLength(int.Parse(spl[1]));
                } else if (search.Equals("Note#")) {
                    this.Note = int.Parse(spl[1]);
                } else if (search.Equals("Dynamics")) {
                    this.Dynamics = int.Parse(spl[1]);
                } else if (search.Equals("PMBendDepth")) {
                    this.PMBendDepth = int.Parse(spl[1]);
                } else if (search.Equals("PMBendLength")) {
                    this.PMBendLength = int.Parse(spl[1]);
                } else if (search.Equals("DEMdecGainRate")) {
                    this.DEMdecGainRate = int.Parse(spl[1]);
                } else if (search.Equals("DEMaccent")) {
                    this.DEMaccent = int.Parse(spl[1]);
                } else if (search.Equals("LyricHandle")) {
                    this.LyricHandle_index = VsqHandle.HandleIndexFromString(spl[1]);
                } else if (search.Equals("IconHandle")) {
                    this.IconHandle_index = VsqHandle.HandleIndexFromString(spl[1]);
                } else if (search.Equals("VibratoHandle")) {
                    this.VibratoHandle_index = VsqHandle.HandleIndexFromString(spl[1]);
                } else if (search.Equals("VibratoDelay")) {
                    this.VibratoDelay = int.Parse(spl[1]);
                } else if (search.Equals("PMbPortamentoUse")) {
                    PMbPortamentoUse = int.Parse(spl[1]);
                } else if (search.Equals("NoteHeadHandle")) {
                    NoteHeadHandle_index = VsqHandle.HandleIndexFromString(spl[1]);
                }
                if (!sr.ready()) {
                    break;
                }
                last_line.value = sr.readLine();
            }
        }

        public string toString()
        {
            string ret = "{Type=" + type;
            if (type == VsqIDType.Anote) {
                ret += ", Length=" + getLength();
                ret += ", Note#=" + Note;
                ret += ", Dynamics=" + Dynamics;
                ret += ", PMBendDepth=" + PMBendDepth;
                ret += ", PMBendLength=" + PMBendLength;
                ret += ", PMbPortamentoUse=" + PMbPortamentoUse;
                ret += ", DEMdecGainRate=" + DEMdecGainRate;
                ret += ", DEMaccent=" + DEMaccent;
                if (LyricHandle != null) {
                    ret += ", LyricHandle=h#" + PortUtil.formatDecimal("0000", LyricHandle_index);
                }
                if (VibratoHandle != null) {
                    ret += ", VibratoHandle=h#" + PortUtil.formatDecimal("0000", VibratoHandle_index);
                    ret += ", VibratoDelay=" + VibratoDelay;
                }
            } else if (type == VsqIDType.Singer) {
                ret += ", IconHandle=h#" + PortUtil.formatDecimal("0000", IconHandle_index);
            }
            ret += "}";
            return ret;
        }

        public override string ToString()
        {
            return toString();
        }

    }

}
