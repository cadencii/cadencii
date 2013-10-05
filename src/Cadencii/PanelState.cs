/*
 * PanelState.cs
 * Copyright © 2009-2011 kbinani
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
    /// ドッキング可能なパネルの状態を表す列挙型
    /// </summary>
    public enum PanelState
    {
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

}
