/*
 * TrackSelectorSingerPopupMenu.cs
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
using cadencii.windows.forms;

namespace cadencii
{

    public class TrackSelectorSingerPopupMenu : System.Windows.Forms.ContextMenuStrip
    {
        public bool SingerChangeExists;
        public int Clock;
        public int InternalID;

        public TrackSelectorSingerPopupMenu(System.ComponentModel.IContainer cont)
            : base(cont)
        {
        }
    }

}
