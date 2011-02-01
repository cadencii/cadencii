/*
 * DrawObject.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import org.kbinani.vsq.*;
import java.awt.*;
#else
using System;
using org.kbinani.vsq;
using org.kbinani.java.awt;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 画面に描画するアイテムを表します
    /// </summary>
#if JAVA
    public class DrawObject implements Comparable<DrawObject>
#else
    public class DrawObject : IComparable<DrawObject>
#endif
    {
        public Rectangle mRectangleInPixel;
        public String mText;
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
        public boolean mIsOverlapped;
        public boolean mIsSymbolProtected;
        public int mNote;
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
        public boolean mIsValidForUtau = false;
        /// <summary>
        /// Straight x UTAUモードにて、歌詞からanalyzed\*.stfを引き当てられたかどうか。
        /// これがfalseのとき、ピアノロール上で警告色で描かれる
        /// </summary>
        public boolean mIsValidForStraight = false;
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
        /// <summary>
        /// Overlapの旗のx座標．単位はピクセル（仮想画面上）
        /// </summary>
        public int mOverlapX;
        /// <summary>
        /// PreUtteranceの旗のx座標．単位はピクセル（仮想画面上）
        /// </summary>
        public int mPreUtteranceX;
        /// <summary>
        /// エンベロープのp1点のx座標．単位はピクセル（仮想画面上）
        /// </summary>
        public int mEnvP1X;
        /// <summary>
        /// エンベロープのp2点のx座標．単位はピクセル（仮想画面上）
        /// </summary>
        public int mEnvP2X;
        /// <summary>
        /// エンベロープのp5点のx座標．単位はピクセル（仮想画面上）
        /// </summary>
        public int mEnvP5X;
        /// <summary>
        /// エンベロープのp3点のx座標．単位はピクセル（仮想画面上）
        /// </summary>
        public int mEnvP3X;
        /// <summary>
        /// エンベロープのp4点のx座標．単位はピクセル（仮想画面上）
        /// </summary>
        public int mEnvP4X;
        /// <summary>
        /// エンベロープの右端のx座標．単位はピクセル（仮想画面上）
        /// </summary>
        public int mEnvEndX;
        /// <summary>
        /// エンベロープのp1点の値
        /// </summary>
        public int mEnvP1V;
        /// <summary>
        /// エンベロープのp2点の値
        /// </summary>
        public int mEnvP2V;
        /// <summary>
        /// エンベロープのp5点の値
        /// </summary>
        public int mEnvP5V;
        /// <summary>
        /// エンベロープのp3点の値
        /// </summary>
        public int mEnvP3V;
        /// <summary>
        /// エンベロープのp4点の値
        /// </summary>
        public int mEnvP4V;

        public DrawObject( DrawObjectType type,
                           VsqFileEx vsq,
                           Rectangle rect, 
                           String text_,
                           int accent_,
                           int decay,
                           int velocity,
                           int internal_id,
                           int vibrato_delay,
                           boolean overwrapped, 
                           boolean symbol_protected,
                           VibratoBPList vib_rate,
                           VibratoBPList vib_depth,
                           int vib_start_rate,
                           int vib_start_depth,
                           int note_,
                           int overlap_x,
                           int pre_utterance_x, 
                           int p1x, int p2x, int p5x, int p3x, int p4x, int pex,
                           int p1v, int p2v, int p5v, int p3v, int p4v,
                           int length,
                           int clock,
                           boolean is_valid_for_utau,
                           boolean is_valid_for_straight,
                           int vib_delay,
                           int intensity ) {
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
            this.mLength = length;
            this.mClock = clock;
            this.mIsValidForUtau = is_valid_for_utau;
            this.mIsValidForStraight = is_valid_for_straight;
            this.mVibDelay = vib_delay;
            mOverlapX = overlap_x;
            mPreUtteranceX = pre_utterance_x;
            mEnvP1X = p1x;
            mEnvP2X = p2x;
            mEnvP5X = p5x;
            mEnvP3X = p3x;
            mEnvP4X = p4x;
            mEnvEndX = pex;
            mEnvP1V = p1v;
            mEnvP2V = p2v;
            mEnvP5V = p5v;
            mEnvP3V = p3v;
            mEnvP4V = p4v;

            if ( vib_rate != null && vib_depth != null ) {
                int viblength = length - vib_delay;
                VibratoPointIteratorByClock itr =
                    new VibratoPointIteratorByClock( vsq,
                                                     vib_rate, vib_start_rate,
                                                     vib_depth, vib_start_depth,
                                                     clock + vib_delay, viblength );
                mVibratoPit = new float[viblength];
                for ( int i = 0; i < viblength; i++ ) {
                    if ( !itr.hasNext() ) {
                        break;
                    }
                    double v = itr.next();
                    mVibratoPit[i] = (float)v;
                }
            }
        }

        public int compareTo( DrawObject item ) {
            return mRectangleInPixel.x - item.mRectangleInPixel.x;
        }

#if !JAVA
        public int CompareTo( DrawObject item ){
            return compareTo( item );
        }
#endif
    }

#if !JAVA
}
#endif
