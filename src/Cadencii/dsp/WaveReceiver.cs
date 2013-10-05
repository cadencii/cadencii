/*
 * WaveReceiver.cs
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

namespace cadencii
{

    /// <summary>
    /// 音声波形の受信器のためのインターフェース．
    /// </summary>
    public interface WaveReceiver
    {
        /// <summary>
        /// 波形を受信します
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="length"></param>
        void push(double[] left, double[] right, int length);

        /// <summary>
        /// 音声波形の受信器を設定します．
        /// </summary>
        /// <param name="r"></param>
        void setReceiver(WaveReceiver r);

        /// <summary>
        /// 波形の受信を終了します．
        /// </summary>
        void end();

        void setGlobalConfig(EditorConfig config);
    }

}
