/*
 * CurveType.cs
 * Copyright © 2009-2011 kbinani
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
using cadencii;
using cadencii.vsq;

namespace cadencii
{

    enum CurveTypeImpl
    {
        VEL,
        DYN,
        BRE,
        BRI,
        CLE,
        OPE,
        GEN,
        POR,
        PIT,
        PBS,
        VibratoRate,
        VibratoDepth,
        Accent,
        Decay,
        harmonics,
        fx2depth,
        reso1freq,
        reso1bw,
        reso1amp,
        reso2freq,
        reso2bw,
        reso2amp,
        reso3freq,
        reso3bw,
        reso3amp,
        reso4freq,
        reso4bw,
        reso4amp,
        Env,
        Empty,
    }

    /// <summary>
    /// vsqファイルで編集可能なカーブ・プロパティの種類
    /// </summary>
    [Serializable]
    public struct CurveType : IEquatable<CurveType>, IComparable<CurveType>
    {
        private string mName;
        private bool mIsScalar;
        private int mMinimum;
        private int mMaximum;
        private int mDefault;
        private bool mIsAttachNote;
        private int mIndex;
        private CurveTypeImpl mType;

        /// <summary>
        /// ベロシティ(index=-4)
        /// </summary>
        public static readonly CurveType VEL = new CurveType(CurveTypeImpl.VEL, true, true, 0, 127, 64, -4);
        /// <summary>
        /// ダイナミクス　64(index=0)
        /// </summary>
        public static readonly CurveType DYN = new CurveType(CurveTypeImpl.DYN, false, false, 0, 127, 64, 0);
        /// <summary>
        /// ブレシネス　0(index=1)
        /// </summary>
        public static readonly CurveType BRE = new CurveType(CurveTypeImpl.BRE, false, false, 0, 127, 0, 1);
        /// <summary>
        /// ブライトネス　64(index=2)
        /// </summary>
        public static readonly CurveType BRI = new CurveType(CurveTypeImpl.BRI, false, false, 0, 127, 64, 2);
        /// <summary>
        /// クリアネス　0(index=3)
        /// </summary>
        public static readonly CurveType CLE = new CurveType(CurveTypeImpl.CLE, false, false, 0, 127, 0, 3);
        /// <summary>
        /// オープニング　127(index=4)
        /// </summary>
        public static readonly CurveType OPE = new CurveType(CurveTypeImpl.OPE, false, false, 0, 127, 127, 4);
        /// <summary>
        /// ジェンダーファクター　64(index=5)
        /// </summary>
        public static readonly CurveType GEN = new CurveType(CurveTypeImpl.GEN, false, false, 0, 127, 64, 5);
        /// <summary>
        /// ポルタメントタイミング　64(index=6)
        /// </summary>
        public static readonly CurveType POR = new CurveType(CurveTypeImpl.POR, false, false, 0, 127, 64, 6);
        public static readonly CurveType PIT = new CurveType(CurveTypeImpl.PIT, false, false, -8192, 8191, 0, 7);
        public static readonly CurveType PBS = new CurveType(CurveTypeImpl.PBS, false, false, 0, 24, 2, 8);
        /// <summary>
        /// ビブラートの振動の速さ(index=9)
        /// </summary>
        public static readonly CurveType VibratoRate = new CurveType(CurveTypeImpl.VibratoRate, false, true, 0, 127, 64, 9);
        /// <summary>
        /// ビブラートの振幅の大きさ(index=10)
        /// </summary>
        public static readonly CurveType VibratoDepth = new CurveType(CurveTypeImpl.VibratoDepth, false, true, 0, 127, 50, 10);
        /// <summary>
        /// Accent(index=-3)
        /// </summary>
        public static readonly CurveType Accent = new CurveType(CurveTypeImpl.Accent, true, true, 0, 100, 50, -3);
        /// <summary>
        /// Decay(index=-2)
        /// </summary>
        public static readonly CurveType Decay = new CurveType(CurveTypeImpl.Decay, true, true, 0, 100, 50, -2);
        /// <summary>
        /// Harmonics(index=11)
        /// </summary>
        public static readonly CurveType harmonics = new CurveType(CurveTypeImpl.harmonics, false, false, 0, 127, 0, 11);
        /// <summary>
        /// FX2Depth(index=12)
        /// </summary>
        public static readonly CurveType fx2depth = new CurveType(CurveTypeImpl.fx2depth, false, false, 0, 127, 0, 12);
        /// <summary>
        /// reso1freq(index=13)
        /// </summary>
        public static readonly CurveType reso1freq = new CurveType(CurveTypeImpl.reso1freq, false, false, 0, 127, 0, 13);
        /// <summary>
        /// reso1bw(index=14)
        /// </summary>
        public static readonly CurveType reso1bw = new CurveType(CurveTypeImpl.reso1bw, false, false, 0, 127, 0, 14);
        /// <summary>
        /// reso1amp(index=15)
        /// </summary>
        public static readonly CurveType reso1amp = new CurveType(CurveTypeImpl.reso1amp, false, false, 0, 127, 0, 15);
        /// <summary>
        /// reso2freq(index=16)
        /// </summary>
        public static readonly CurveType reso2freq = new CurveType(CurveTypeImpl.reso2freq, false, false, 0, 127, 0, 16);
        /// <summary>
        /// reso2bw(index=17)
        /// </summary>
        public static readonly CurveType reso2bw = new CurveType(CurveTypeImpl.reso2bw, false, false, 0, 127, 0, 17);
        /// <summary>
        /// reso2amp(index=18)
        /// </summary>
        public static readonly CurveType reso2amp = new CurveType(CurveTypeImpl.reso2amp, false, false, 0, 127, 0, 18);
        /// <summary>
        /// reso3freq(index=19)
        /// </summary>
        public static readonly CurveType reso3freq = new CurveType(CurveTypeImpl.reso3freq, false, false, 0, 127, 0, 19);
        /// <summary>
        /// reso3bw(index=20)
        /// </summary>
        public static readonly CurveType reso3bw = new CurveType(CurveTypeImpl.reso3bw, false, false, 0, 127, 0, 20);
        /// <summary>
        /// reso3amp(index=21)
        /// </summary>
        public static readonly CurveType reso3amp = new CurveType(CurveTypeImpl.reso3amp, false, false, 0, 127, 0, 21);
        /// <summary>
        /// reso4freq(index=22)
        /// </summary>
        public static readonly CurveType reso4freq = new CurveType(CurveTypeImpl.reso4freq, false, false, 0, 127, 0, 22);
        /// <summary>
        /// reso4bw(index=23)
        /// </summary>
        public static readonly CurveType reso4bw = new CurveType(CurveTypeImpl.reso4bw, false, false, 0, 127, 0, 23);
        /// <summary>
        /// reso4amp(index=24)
        /// </summary>
        public static readonly CurveType reso4amp = new CurveType(CurveTypeImpl.reso4amp, false, false, 0, 127, 0, 24);
        public static readonly CurveType Env = new CurveType(CurveTypeImpl.Env, true, true, 0, 200, 100, -1);

        public static readonly CurveType Empty = new CurveType(CurveTypeImpl.Empty, false, false, 0, 0, 0, -1);

        private CurveType(CurveTypeImpl type_impl, bool is_scalar, bool is_attach_note, int min, int max, int defalt_value, int index)
        {
            mType = type_impl;
            mIsScalar = is_scalar;
            mMinimum = min;
            mMaximum = max;
            mDefault = defalt_value;
            mIsAttachNote = is_attach_note;
            mIndex = index;
            if (mType == CurveTypeImpl.VEL) {
                mName = "VEL";
            } else if (mType == CurveTypeImpl.DYN) {
                mName = "DYN";
            } else if (mType == CurveTypeImpl.BRE) {
                mName = "BRE";
            } else if (mType == CurveTypeImpl.BRI) {
                mName = "BRI";
            } else if (mType == CurveTypeImpl.CLE) {
                mName = "CLE";
            } else if (mType == CurveTypeImpl.OPE) {
                mName = "OPE";
            } else if (mType == CurveTypeImpl.GEN) {
                mName = "GEN";
            } else if (mType == CurveTypeImpl.POR) {
                mName = "POR";
            } else if (mType == CurveTypeImpl.PIT) {
                mName = "PIT";
            } else if (mType == CurveTypeImpl.PBS) {
                mName = "PBS";
            } else if (mType == CurveTypeImpl.VibratoRate) {
                mName = "V-Rate";
            } else if (mType == CurveTypeImpl.VibratoDepth) {
                mName = "V-Depth";
            } else if (mType == CurveTypeImpl.Accent) {
                mName = "Accent";
            } else if (mType == CurveTypeImpl.Decay) {
                mName = "Decay";
            } else if (mType == CurveTypeImpl.harmonics) {
                mName = "Harm";
            } else if (mType == CurveTypeImpl.fx2depth) {
                mName = "fx2dep";
            } else if (mType == CurveTypeImpl.reso1freq) {
                mName = "res1freq";
            } else if (mType == CurveTypeImpl.reso1bw) {
                mName = "res1bw";
            } else if (mType == CurveTypeImpl.reso1amp) {
                mName = "res1amp";
            } else if (mType == CurveTypeImpl.reso2freq) {
                mName = "res2freq";
            } else if (mType == CurveTypeImpl.reso2bw) {
                mName = "res2bw";
            } else if (mType == CurveTypeImpl.reso2amp) {
                mName = "res2amp";
            } else if (mType == CurveTypeImpl.reso3freq) {
                mName = "res3freq";
            } else if (mType == CurveTypeImpl.reso3bw) {
                mName = "res3bw";
            } else if (mType == CurveTypeImpl.reso3amp) {
                mName = "res3amp";
            } else if (mType == CurveTypeImpl.reso4freq) {
                mName = "res4freq";
            } else if (mType == CurveTypeImpl.reso4bw) {
                mName = "res4bw";
            } else if (mType == CurveTypeImpl.reso4amp) {
                mName = "res4amp";
            } else if (mType == CurveTypeImpl.Env) {
                mName = "Env";
            } else {
#if DEBUG
                sout.println("CurveType#.ctor; mType=" + mType);
#endif
                mName = "Empty";
            }
        }

        public Object clone()
        {
            return new CurveType(this.mType, this.mIsScalar, this.mIsAttachNote, this.mMinimum, this.mMaximum, this.mDefault, this.mIndex);
        }

        public int compareTo(CurveType item)
        {
            if (mIndex < 0) {
                if (item.mIndex < 0) {
                    return mType.CompareTo(item.mType);
                } else {
                    return 1;
                }
            } else {
                if (item.mIndex < 0) {
                    return -1;
                } else {
                    return mIndex - item.mIndex;
                }
            }
        }

        public int CompareTo(CurveType item)
        {
            return compareTo(item);
        }

        public override string ToString()
        {
            return toString();
        }

        public string toString()
        {
            return getName();
        }

        public int getIndex()
        {
            return mIndex;
        }

        public string getName()
        {
            return mName;
        }

        public bool isAttachNote()
        {
            return mIsAttachNote;
        }

        public bool isScalar()
        {
            return mIsScalar;
        }

        public int getMaximum()
        {
            return mMaximum;
        }

        public int getMinimum()
        {
            return mMinimum;
        }

        public int getDefault()
        {
            return mDefault;
        }

        public bool equals(CurveType other)
        {
            return (mType == other.mType) && (mIsScalar == other.mIsScalar);
        }

        public bool Equals(CurveType obj)
        {
            return this.equals(obj);
        }
    }

}
