/*
 * WaveRateConvertAdapter.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.media.
 *
 * cadencii.media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace cadencii.media
{

    /// <summary>
    /// 接頭辞b: 単位が変換前のサンプル数になっている変数
    /// 接頭辞a: 単位が変換後のサンプル数になっている変数
    /// </summary>
    public class WaveRateConvertAdapter
    {
        private IWaveReceiver mReceiver;
        private RateConvertContext mContext = null;

        /// <summary>
        /// コンストラクタ．変換後のサンプリング周波数は，receiverのgetSampleRate()で自動的に取得される
        /// </summary>
        /// <param name="receiver">変換した波形を送る相手先</param>
        /// <param name="sample_rate">変換前のサンプリング周波数</param>
        public WaveRateConvertAdapter(IWaveReceiver receiver, int sample_rate)
        {
            mReceiver = receiver;
            int rate_from = sample_rate;
            int rate_to = receiver.getSampleRate();
            try {
                mContext = new RateConvertContext(rate_from, rate_to);
            } catch (Exception ex) {
                mContext = null; // m9(＠ｑ＠)
            }
        }

        public void close()
        {
#if DEBUG
            sout.println("WaveRateConvertAdapter#close");
#endif
            mReceiver.close();
        }

        public void append(double[] left, double[] right, int length)
        {
            while (RateConvertContext.convert(mContext, left, right, length)) {
                mReceiver.append(mContext.bufferLeft, mContext.bufferRight, mContext.length);
            }
        }
    }

}
