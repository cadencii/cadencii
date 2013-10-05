/*
 * BezierPickedSide.cs
 * Copyright © 2008-2011 kbinani
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
namespace cadencii
{

    /// <summary>
    /// ベジエ曲線のデータ点や制御点を選択したとき，どの種類の点を選択したかを表現するための列挙型
    /// </summary>
    public enum BezierPickedSide
    {
        /// <summary>
        /// データ点の右側にある制御点を選択したことを表す
        /// </summary>
        RIGHT,
        /// <summary>
        /// データ点そのものを選択したことを表す
        /// </summary>
        BASE,
        /// <summary>
        /// データ点の左側にある制御点を選択したことを表す
        /// </summary>
        LEFT,
    }

}
