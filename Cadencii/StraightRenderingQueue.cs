/*
 * StraightRenderingQueue.cs
 * Copyright (c) 2009 kbinani
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
#else
using System;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
#endif

    public class StraightRenderingQueue {
        /// <summary>
        /// このキューのレンダリング結果のwavを、曲頭から何フレーム目にmixしたらよいかを表す
        /// </summary>
        public int startFrame;
        /// <summary>
        /// 音源のフォルダ
        /// </summary>
        public String oto_ini;
        /// <summary>
        /// このキューのレンダリング結果の、おおよその長さ。正確な長さはレンダリング結果が出るまでは不明。
        /// </summary>
        public long abstractFrameLength;
        /// <summary>
        /// メタテキストの生成に必要なトラックデータ
        /// </summary>
        public VsqTrack track;
        public int endClock;
    }

#if !JAVA
}
#endif
