/*
 * BezierPickedSide.cs
 * Copyright © 2008-2010 kbinani
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
#else
namespace org.kbinani.cadencii {
#endif
 
    /// <summary>
    /// ベジエ曲線のデータ点や制御点を選択したとき，どの種類の点を選択したかを表現するための列挙型
    /// </summary>
    public enum BezierPickedSide {
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

#if !JAVA
}
#endif
