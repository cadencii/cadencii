/*
 * PaletteToolMenuItem.cs
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

import cadencii.windows.forms.*;

#else

using System;
using cadencii.windows.forms;

namespace cadencii
{
#endif

#if JAVA
    public class PaletteToolMenuItem extends BMenuItem
#else
    public class PaletteToolMenuItem : BMenuItem
#endif
    {
        private String mPaletteToolID;

        public PaletteToolMenuItem( String id )
#if JAVA
        {
            super();
#else
            : base()
        {
#endif
            mPaletteToolID = id;
        }

        public String getPaletteToolID()
        {
            return mPaletteToolID;
        }
    }

#if !JAVA
}
#endif
