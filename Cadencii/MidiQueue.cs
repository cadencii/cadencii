/*
 * MidiQueue.cs
 * Copyright (c) 2009 kbinani
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
using System;
using System.Collections.Generic;

using bocoree;

namespace Boare.Cadencii {

    public delegate Vector<MidiQueue> MidiQueueDoneEventHandler( MidiQueue sender );

    public class MidiQueue : IComparable<MidiQueue> {
        public MidiQueueDoneEventHandler Done;
        public int Track;
        public int Clock;
        public byte Channel;
        public byte Program;
        public byte Note;
        public byte Velocity;

        public int CompareTo( MidiQueue item ) {
            int ret = Clock - item.Clock;
            if ( ret == 0 ) {
                return (int)(Velocity - item.Velocity);
            } else {
                return ret;
            }
        }
    }

}
