/*
 * BezierControlType.cs
 * Copyright © 2008-2011 kbinani
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
    /// （互換性維持のため、None->NONE等とリファクタしていない）
    /// </summary>
    public enum BezierControlType {
        /// <summary>
        /// 制御点無し
        /// </summary>
        None,
        /// <summary>
        /// このタイプの制御点を移動させると、逆サイドの制御点も自動的に移動する
        /// </summary>
        Normal,
        /// <summary>
        /// このタイプの制御点を移動させても、逆サイドの制御点には影響しない
        /// </summary>
        Master,
    }

#if !JAVA
}
#endif
