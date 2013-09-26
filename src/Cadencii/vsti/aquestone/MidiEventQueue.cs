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
using System.Collections.Generic;
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
        public List<MidiEvent> noteoff;
        public List<MidiEvent> noteon;
        public List<MidiEvent> pit;
        public List<ParameterEvent> param;

        public MidiEventQueue()
        {
            noteoff = new List<MidiEvent>();
            noteon = new List<MidiEvent>();
            pit = new List<MidiEvent>();
            param = new List<ParameterEvent>();
        }
    }
}
