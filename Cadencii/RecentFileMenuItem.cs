/*
 * RecentFileMenuItem.cs
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

using System;
using com.github.cadencii.windows.forms;

namespace com.github.cadencii
{
#endif

#if JAVA
    public class RecentFileMenuItem extends BMenuItem
#else
    public class RecentFileMenuItem : BMenuItem
#endif
    {
        private String mFilePath;

        public RecentFileMenuItem( String file_path )
#if JAVA
        {
            super();
#else
            : base()
        {
#endif
            mFilePath = file_path;
        }

        public String getFilePath()
        {
            return mFilePath;
        }
    }

#if !JAVA
}
#endif
