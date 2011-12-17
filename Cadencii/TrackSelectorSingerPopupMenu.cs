/*
 * TrackSelectorSingerPopupMenu.cs
 * Copyright © 2011 kbinani
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

package com.github.cadencii;

import com.github.cadencii.windows.forms.*;

#else

using com.github.cadencii.windows.forms;

namespace com.github.cadencii
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class TrackSelectorSingerPopupMenu extends BPopupMenu
#else
    public class TrackSelectorSingerPopupMenu : BPopupMenu
#endif
    {
        public boolean SingerChangeExists;
        public int Clock;
        public int InternalID;

#if !JAVA
        public TrackSelectorSingerPopupMenu( System.ComponentModel.IContainer cont )
            : base( cont )
        {
        }
#endif
    }

#if !JAVA
}
#endif
