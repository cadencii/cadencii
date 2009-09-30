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
package com.boare.cadencii;

import com.boare.vsq.*;

public class SelectedEventEntry {
    public int track;
    public VsqEvent original;
    public VsqEvent editing;

    public SelectedEventEntry( int aTrack, VsqEvent aOriginal, VsqEvent aEditing ) {
        track = aTrack;
        original = aOriginal;
        editing = aEditing;
    }
}
