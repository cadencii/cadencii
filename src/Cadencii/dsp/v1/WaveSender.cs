/*
 * WaveSender.cs
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
using cadencii.java.util;

namespace cadencii
{

    /// <summary>
    /// 音声波形を出力する受動的生成器．
    /// 自分では音声波形を出力せず，pullが呼ばれて初めて波形を生成する．
    /// 能動的生成器として利用するには，WaveSenderDriverクラスを用いる．
    /// </summary>
    public interface WaveSender
    {
        /// <summary>
        /// 音声波形の生成を要求するためのメソッド．
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <param name="length"></param>
        void pull(double[] l, double[] r, int length);

        /// <summary>
        /// この生成器の1つ上流に配置する波形生成器を設定します．
        /// このクラスのインスタンスのpullが呼ばれると，1つ上流の生成器のpullを呼び出すことになる．
        /// </summary>
        /// <param name="s"></param>
        void setSender(WaveSender s);

        /// <summary>
        /// 波形の生成を終了します．
        /// </summary>
        void end();

        void setGlobalConfig(EditorConfig config);
    }

}
