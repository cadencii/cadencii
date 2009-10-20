/*
 * SelectedEventEntry.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.Cadencii;

import org.kbinani.vsq.*;
#else
using System;
using Boare.Lib.Vsq;

namespace Boare.Cadencii
{
#endif

    public class SelectedEventEntry
    {
        public int track;
        public VsqEvent original;
        public VsqEvent editing;

        public SelectedEventEntry( int track_, VsqEvent original_, VsqEvent editing_ )
        {
            track = track_;
            original = original_;
            editing = editing_;
        }
    }

#if !JAVA
}
#endif
