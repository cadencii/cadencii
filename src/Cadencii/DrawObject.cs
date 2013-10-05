/*
 * DrawObject.cs
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
using cadencii.vsq;
using cadencii.java.awt;

namespace cadencii
{

    /// <summary>
    /// 画面に描画するアイテムを表します
    /// </summary>
    public class DrawObject : IComparable<DrawObject>
    {
        public Rectangle mRectangleInPixel;
        public string mText;
        public int mAccent;
        public int mDecay;
        public int mVelocity;
        public int mInternalID;
        /// <summary>
        /// 音符の先頭から，ビブラート開始位置までの長さ(単位：ピクセル)
        /// </summary>
        public int mVibratoDelayInPixel;
        /// <summary>
        /// このアイテムが他のアイテムと再生時にオーバーラップするかどうかを表すフラグ
        /// </summary>
        public bool mIsOverlapped;
        public bool mIsSymbolProtected;
        public int mNote;
        public UstEnvelope mUstEnvelope;
        /// <summary>
        /// 音符の長さ（クロック）
        /// </summary>
        public int mLength;
        /// <summary>
        /// アイテムの位置
        /// </summary>
        public int mClock;
        public DrawObjectType mType;
        /// <summary>
        /// UTAUモードにて、歌詞から*.wavを引き当てられたかどうか。
        /// これがfalseのとき、ピアノロール上で警告色で描かれる
        /// </summary>
        public bool mIsValidForUtau = false;
        /// <summary>
        /// Straight x UTAUモードにて、歌詞からanalyzed\*.stfを引き当てられたかどうか。
        /// これがfalseのとき、ピアノロール上で警告色で描かれる
        /// </summary>
        public bool mIsValidForStraight = false;
        public int mVibDelay = 0;
        /// <summary>
        /// ビブラートによるピッチカーブ。
        /// 単位はノート、配列のインデックスがクロックに相当する。
        /// </summary>
        public float[] mVibratoPit = null;
        /// <summary>
        /// UTAUの音量
        /// </summary>
        public int mIntensity = 100;

        public DrawObject(DrawObjectType type,
                           VsqFileEx vsq,
                           Rectangle rect,
                           string text_,
                           int accent_,
                           int decay,
                           int velocity,
                           int internal_id,
                           int vibrato_delay,
                           bool overwrapped,
                           bool symbol_protected,
                           VibratoBPList vib_rate,
                           VibratoBPList vib_depth,
                           int vib_start_rate,
                           int vib_start_depth,
                           int note_,
                           UstEnvelope ust_envelope,
                           int length,
                           int clock,
                           bool is_valid_for_utau,
                           bool is_valid_for_straight,
                           int vib_delay,
                           int intensity)
        {
            this.mType = type;
            mRectangleInPixel = rect;
            mText = text_;
            mAccent = accent_;
            mDecay = decay;
            mVelocity = velocity;
            mInternalID = internal_id;
            mVibratoDelayInPixel = vibrato_delay;
            mIsOverlapped = overwrapped;
            mIsSymbolProtected = symbol_protected;
            mIntensity = intensity;

            mNote = note_;
            mUstEnvelope = ust_envelope;
            this.mLength = length;
            this.mClock = clock;
            this.mIsValidForUtau = is_valid_for_utau;
            this.mIsValidForStraight = is_valid_for_straight;
            this.mVibDelay = vib_delay;

            int viblength = length - vib_delay;
            if (viblength > 0 && vib_rate != null && vib_depth != null) {
                VibratoPointIteratorByClock itr =
                    new VibratoPointIteratorByClock(vsq.TempoTable,
                                                     vib_rate, vib_start_rate,
                                                     vib_depth, vib_start_depth,
                                                     clock + vib_delay, viblength);
                mVibratoPit = new float[viblength];
                for (int i = 0; i < viblength; i++) {
                    if (!itr.hasNext()) {
                        break;
                    }
                    double v = itr.next();
                    mVibratoPit[i] = (float)v;
                }
            }
        }

        public int compareTo(DrawObject item)
        {
            return mRectangleInPixel.x - item.mRectangleInPixel.x;
        }

        public int CompareTo(DrawObject item)
        {
            return compareTo(item);
        }
    }

}
