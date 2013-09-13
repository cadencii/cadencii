/*
 * MidiEventQueue.cs
 * Copyright © 2013 kbinani
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
using cadencii;
using cadencii.vsq;
using cadencii.java.util;

namespace cadencii
{
    /// <summary>
    /// ある時刻(clock)に送信すべきイベントを表すクラス
    /// </summary>
    public class MidiEventQueue
    {
        public Vector<MidiEvent> noteoff;
        public Vector<MidiEvent> noteon;
        public Vector<MidiEvent> pit;
        public Vector<ParameterEvent> param;

        public MidiEventQueue()
        {
            noteoff = new Vector<MidiEvent>();
            noteon = new Vector<MidiEvent>();
            pit = new Vector<MidiEvent>();
            param = new Vector<ParameterEvent>();
        }
    }
}
