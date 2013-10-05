/*
 * TempoVectorSearchContext.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace cadencii.vsq
{

    /// <summary>
    /// テンポテーブルに基づき，時刻とゲートタイムを相互変換する際の検索量を小さくするための検索コンテキスト
    /// </summary>
    public class TempoVectorSearchContext
    {
        public int mSec2ClockIndex = 0;
        public double mSec2ClockSec = 0.0;
        public int mClock2SecIndex = 0;
        public double mClock2SecClock = 0.0;
    }

}
