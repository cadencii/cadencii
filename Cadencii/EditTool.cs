/*
 * EditTool.cs
 * Copyright (C) 2008-2010 kbinani
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
    /// 編集ツールを表す列挙型
    /// </summary>
    public enum EditTool {
        /// <summary>
        /// 矢印ツール
        /// </summary>
        ARROW,
        /// <summary>
        /// 鉛筆ツール
        /// </summary>
        PENCIL,
        /// <summary>
        /// 直線ツール
        /// </summary>
        LINE,
        /// <summary>
        /// 消しゴムツール
        /// </summary>
        ERASER,
#if ENABLE_SCRIPT
        /// <summary>
        /// ユーザー定義のパレットツール
        /// </summary>
        PALETTE_TOOL,
#endif
    }

#if !JAVA
}
#endif
