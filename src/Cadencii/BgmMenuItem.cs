/*
 * BgmMenuItem.cs
 * Copyright © 2011 kbinani
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
using System.Windows.Forms;
using cadencii.windows.forms;
using cadencii;

namespace cadencii
{

    public class BgmMenuItem : ToolStripMenuItem
    {
        private int mBgmIndex;

        public BgmMenuItem(int bgm_index)
            : base()
        {
            mBgmIndex = bgm_index;
        }

        /// <summary>
        /// このメニューアイテムが担当しているBGMのインデックスを取得します
        /// </summary>
        /// <returns>BGMのインデックス</returns>
        public int getBgmIndex()
        {
            return mBgmIndex;
        }
    }

}
