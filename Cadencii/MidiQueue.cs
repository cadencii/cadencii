#if !JAVA
/*
 * MidiQueue.cs
 * Copyright © 2009-2011 kbinani
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
using System;
using System.Collections.Generic;
using com.github.cadencii;
using com.github.cadencii.java.util;

namespace com.github.cadencii {

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
            if ( item == null ) {
                return 1;
            } else {
                int ret = Clock - item.Clock;
                if ( ret == 0 ) {
                    return (int)(Velocity - item.Velocity);
                } else {
                    return ret;
                }
            }
        }
    }

}
#endif
