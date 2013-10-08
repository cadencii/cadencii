/*
 * WaveSenderDriver.cs
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
using cadencii.java.awt;
using cadencii.java.util;

namespace cadencii
{

    /// <summary>
    /// WaveSenderをWaveGeneratorとして使うためのドライバー．
    /// WaveSenderは受動的波形生成器なので，自分では波形を作らない．
    /// </summary>
    public class WaveSenderDriver : WaveUnit, WaveGenerator
    {
        private const int BUFLEN = 1024;

        private WaveSender mWaveSender = null;
        private double[] mBufferL = new double[BUFLEN];
        private double[] mBufferR = new double[BUFLEN];
        private long mTotalAppend = 0;
        private long mTotalSamples = 1L;
        private WaveReceiver mReceiver = null;
        private int mVersion = 0;
        //private bool mAbortRequired = false;
        private bool mRunning = false;
        private int mSampleRate;

        public int getSampleRate()
        {
            return mSampleRate;
        }

        public bool isRunning()
        {
            return mRunning;
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

        /*public void stop()
        {
            if ( mRunning ) {
                mAbortRequired = true;
                while ( mRunning ) {
                    Thread.Sleep( 100 );
                }
            }
        }*/

        public override int getVersion()
        {
            return mVersion;
        }

        public override void setConfig(string parameters)
        {
            // do nothing
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track"></param>
        /// <param name="start_clock"></param>
        /// <param name="end_clock"></param>
        public void init(VsqFileEx vsq, int track, int start_clock, int end_clock, int sample_rate)
        {
            mSampleRate = sample_rate;
#if DEBUG
            sout.println("WaveSenderDriver#init; sample_rate=" + sample_rate);
#endif
        }

        public void setSender(WaveSender wave_sender)
        {
            mWaveSender = wave_sender;
        }

        public void setReceiver(WaveReceiver r)
        {
            if (mReceiver != null) {
                mReceiver.end();
            }
            mReceiver = r;
        }

        public long getPosition()
        {
            return mTotalAppend;
        }

        public void begin(long length, WorkerState state)
        {
            mRunning = true;
            mTotalSamples = length;
            long remain = length;
            while (remain > 0 && !state.isCancelRequested()) {
                int amount = (remain > BUFLEN) ? BUFLEN : (int)remain;
                mWaveSender.pull(mBufferL, mBufferR, amount);
                mReceiver.push(mBufferL, mBufferR, amount);
                remain -= amount;
                mTotalAppend += amount;
            }
            mWaveSender.end();
            mReceiver.end();
            mRunning = false;
        }
    }

}
