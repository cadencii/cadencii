/*
 * VibratoPointIteratorByClock.cs
 * Copyright © 2010-2011 kbinani
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
using cadencii.java.util;

namespace cadencii
{

    /// <summary>
    /// ビブラート用のデータ点のリストを取得します。返却されるリストは、{クロック, ビブラートの振幅(ノートナンバー単位)}の値ペアとなっています
    /// </summary>
    public class VibratoPointIteratorByClock
    {
        TempoVector mTempoTable;
        VibratoBPList mRate;
        int mStartRate;
        VibratoBPList mDepth;
        int mStartDepth;
        int mClockStart;
        int mClockWidth;

        double mSec0;
        double mSec1;
        double mPhase = 0.0;
        double mAmplitude;
        float mPeriod;
        float mOmega;
        double mSec;
        float mFadeWidth;
        int mIndex;
        bool mFirst = true;

        public void rewind()
        {
            mSec0 = mTempoTable.getSecFromClock(mClockStart);
            mSec1 = mTempoTable.getSecFromClock(mClockStart + mClockWidth);
            mFadeWidth = (float)(mSec1 - mSec0) * 0.2f;
            mPhase = 0;
            mStartRate = mRate.getValue(0.0f, mStartRate);
            mStartDepth = mDepth.getValue(0.0f, mStartDepth);
            mAmplitude = mStartDepth * 2.5f / 127.0f / 2.0f; // ビブラートの振幅。
            mPeriod = VibratoPointIteratorBySec.getPeriodFromRate(mStartRate); //ビブラートの周期、秒
            mOmega = (float)(2.0 * Math.PI / mPeriod); // 角速度(rad/sec)
            mSec = mSec0;
            mIndex = 0;
            mFirst = true;
        }

        public Double next()
        {
            if (mFirst) {
                mFirst = false;
                return 0.0;
            } else {
                mIndex++;
                if (mIndex < mClockWidth) {
                    int clock = mClockStart + mIndex;
                    double t_sec = mTempoTable.getSecFromClock(clock);
                    if (mSec0 <= t_sec && t_sec <= mSec0 + mFadeWidth) {
                        mAmplitude *= (t_sec - mSec0) / mFadeWidth;
                    }
                    if (mSec1 - mFadeWidth <= t_sec && t_sec <= mSec1) {
                        mAmplitude *= (mSec1 - t_sec) / mFadeWidth;
                    }
                    mPhase += mOmega * (t_sec - mSec);
                    double ret = mAmplitude * Math.Sin(mPhase);
                    float v = (float)(clock - mClockStart) / (float)mClockWidth;
                    int r = mRate.getValue(v, mStartRate);
                    int d = mDepth.getValue(v, mStartDepth);
                    mAmplitude = d * 2.5f / 127.0f / 2.0f;
                    mPeriod = VibratoPointIteratorBySec.getPeriodFromRate(r);
                    mOmega = (float)(2.0 * Math.PI / mPeriod);
                    mSec = t_sec;
                    return ret;
                } else {
                    return 0.0;
                }
            }
        }

        public bool hasNext()
        {
            if (mFirst) {
                return true;
            } else {
                return (mIndex < mClockWidth);
            }
        }

        public void remove()
        {
        }

        public VibratoPointIteratorByClock(TempoVector tempo_table,
                                      VibratoBPList rate,
                                      int start_rate,
                                      VibratoBPList depth,
                                      int start_depth,
                                      int clock_start,
                                      int clock_width)
        {
            this.mTempoTable = tempo_table;
            this.mRate = rate;
            this.mStartRate = start_rate;
            this.mDepth = depth;
            this.mStartDepth = start_depth;
            this.mClockStart = clock_start;
            this.mClockWidth = clock_width;

            rewind();
        }
    }

}
