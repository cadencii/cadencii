/*
 * EmptyWaveGenerator.cs
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
using System.Threading;

namespace cadencii
{

    /// <summary>
    /// 無音の波形を送信するWaveGenerator
    /// </summary>
    public class EmptyWaveGenerator : WaveUnit, WaveGenerator
    {
        private const int VERSION = 0;
        private const int BUFLEN = 1024;
        private WaveReceiver mReceiver = null;
        private bool mAbortRequested = false;
        private bool mRunning = false;
        private long mTotalAppend = 0L;
        private long mTotalSamples = 0L;
        private int mSampleRate = 0;

        public int getSampleRate()
        {
            return mSampleRate;
        }

        public bool isRunning()
        {
            return mRunning;
        }

        public long getPosition()
        {
            return mTotalAppend;
        }

        public long getTotalSamples()
        {
            return mTotalSamples;
        }

        public double getProgress()
        {
            if (mTotalSamples <= 0) {
                return 0.0;
            } else {
                return mTotalAppend / (double)mTotalSamples;
            }
        }

        public override int getVersion()
        {
            return VERSION;
        }

        public override void setConfig(string parameter)
        {
            // do nothing
        }

        public void begin(long samples, WorkerState state)
        {
            if (mReceiver == null) return;
            mRunning = true;
            mTotalSamples = samples;
            double[] l = new double[BUFLEN];
            double[] r = new double[BUFLEN];
            for (int i = 0; i < BUFLEN; i++) {
                l[i] = 0.0;
                r[i] = 0.0;
            }
            long remain = samples;
            while (remain > 0 && !mAbortRequested) {
                int amount = (remain > BUFLEN) ? BUFLEN : (int)remain;
                mReceiver.push(l, r, amount);
                remain -= amount;
                mTotalAppend += amount;
            }
            mRunning = false;
            mReceiver.end();
        }

        public void setReceiver(WaveReceiver receiver)
        {
            mReceiver = receiver;
        }

        public void init(VsqFileEx vsq, int track, int start_clock, int end_clock, int sample_rate)
        {
            mSampleRate = sample_rate;
        }

        public void stop()
        {
            if (mRunning) {
                mAbortRequested = true;
                while (mRunning) {
                    Thread.Sleep(100);
                }
            }
        }
    }

}
