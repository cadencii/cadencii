/*
 * SelectedTimesigEntry.cs
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

public class SelectedTimesigEntry {
    public TimeSigTableEntry original;
    public TimeSigTableEntry editing;

    public SelectedTimesigEntry( TimeSigTableEntry aOriginal, TimeSigTableEntry aEditing ) {
        original = aOriginal;
        editing = aEditing;
    }
}
