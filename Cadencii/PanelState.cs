/*
 * PanelState.cs
 * Copyright © 2009-2011 kbinani
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
    /// ドッキング可能なパネルの状態を表す列挙型
    /// </summary>
    public enum PanelState {
        /// <summary>
        /// 非表示状態
        /// </summary>
        Hidden,
        /// <summary>
        /// ウィンドウに分離された状態
        /// </summary>
        Window,
        /// <summary>
        /// ドッキングされた状態
        /// </summary>
        Docked,
    }

#if !JAVA
}
#endif
