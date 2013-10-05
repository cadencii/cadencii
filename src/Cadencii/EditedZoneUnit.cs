/*
 * EditedZoneUnit.cs
 * Copyright © 2010-2011 kbinani
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

namespace cadencii
{

    public class EditedZoneUnit : ICloneable, IComparable<EditedZoneUnit>
    {
        public int mStart;
        public int mEnd;

        private EditedZoneUnit()
        {
        }

        public EditedZoneUnit(int start, int end)
        {
            this.mStart = start;
            this.mEnd = end;
        }

        public int compareTo(EditedZoneUnit item)
        {
            return this.mStart - item.mStart;
        }

        public int CompareTo(EditedZoneUnit item)
        {
            return compareTo(item);
        }

        public Object clone()
        {
            return new EditedZoneUnit(mStart, mEnd);
        }

        public Object Clone()
        {
            return clone();
        }
    }

}
