/**
 * VConnectRenderingQueue.cs
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
using cadencii.vsq;

namespace cadencii
{

    public class VConnectRenderingQueue
    {
        /// <summary>
        /// このキューのレンダリング結果のwavを、曲頭から何フレーム目にmixしたらよいかを表す
        /// </summary>
        public long startSample;
        /// <summary>
        /// 音源のフォルダ
        /// </summary>
        public string oto_ini;
        /// <summary>
        /// このキューのレンダリング結果の、おおよその長さ。正確な長さはレンダリング結果が出るまでは不明。
        /// </summary>
        public long abstractSamples;
        /// <summary>
        /// メタテキストの生成に必要なトラックデータ
        /// </summary>
        public VsqTrack track;
        public int endClock;

#if DEBUG
        public string __DEBUG__toString()
        {
            string phase = "";
            for (int i = 0; i < track.getEventCount(); i++) {
                VsqEvent itemi = track.getEvent(i);
                if (itemi.ID.type == VsqIDType.Anote) {
                    phase += itemi.ID.LyricHandle.L0.Phrase;
                }
            }
            return phase;
        }
#endif
    }

}
