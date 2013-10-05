/*
 * VibratoLengthEditingRule.cs
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
    /// 音符長さが変更されるとき、ビブラート長さがどのように影響を受けるかを決定する列挙子
    /// </summary>
    public enum VibratoLengthEditingRule
    {
        /// <summary>
        /// 音符頭からビブラート開始位置までのディレイが保存される
        /// </summary>
        DELAY,
        /// <summary>
        /// ビブラートの長さが保存される
        /// </summary>
        LENGTH,
        /// <summary>
        /// 音符長さに対するビブラート長さの割合が保存される
        /// </summary>
        PERCENTAGE,
    }

}
