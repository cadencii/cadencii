/*
 * EditTool.cs
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
    /// 編集ツールを表す列挙型
    /// </summary>
    public enum EditTool
    {
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

}
