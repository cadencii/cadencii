/*
 * SelectedTimesigEntry.cs
 * Copyright © 2008-2011 kbinani
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
using cadencii.vsq;

namespace cadencii
{

    public class SelectedTimesigEntry
    {
        public TimeSigTableEntry original;
        public TimeSigTableEntry editing;

        public SelectedTimesigEntry(TimeSigTableEntry original_, TimeSigTableEntry editing_)
        {
            original = original_;
            editing = editing_;
        }
    }

}
