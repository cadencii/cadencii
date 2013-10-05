/*
 * TempoTableEntry.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace cadencii.vsq
{

    [Serializable]
    public class TempoTableEntry : IComparable<TempoTableEntry>, ICloneable
    {
        public int Clock;
        public int Tempo;
        public double Time;

        public string toString()
        {
            return "{Clock=" + Clock + ", Tempo=" + Tempo + ", Time=" + Time + "}";
        }

        public override string ToString()
        {
            return toString();
        }

        public Object clone()
        {
            return new TempoTableEntry(Clock, Tempo, Time);
        }

        public object Clone()
        {
            return clone();
        }

        public TempoTableEntry(int clock, int _tempo, double _time)
        {
            this.Clock = clock;
            this.Tempo = _tempo;
            this.Time = _time;
        }

        public TempoTableEntry()
        {
        }

        public int compareTo(TempoTableEntry entry)
        {
            return this.Clock - entry.Clock;
        }

        public int CompareTo(TempoTableEntry entry)
        {
            return compareTo(entry);
        }

        public bool Equals(TempoTableEntry entry)
        {
            if (this.Clock == entry.Clock) {
                return true;
            } else {
                return false;
            }
        }
    }

}
