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
#if JAVA

package cadencii;

import cadencii.windows.forms.BMenuItem;

#else

using System;
using System.Windows.Forms;
using cadencii.windows.forms;
using cadencii;

namespace cadencii
{
#endif

#if JAVA
    public class BgmMenuItem extends BMenuItem
#else
    public class BgmMenuItem : ToolStripMenuItem
#endif
    {
        private int mBgmIndex;

        public BgmMenuItem( int bgm_index )
#if JAVA
        {
            super();
#else
            : base()
        {
#endif
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

#if !JAVA
}
#endif
