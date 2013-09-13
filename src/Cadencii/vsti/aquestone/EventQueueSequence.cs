/*
 * EventQueueSequence.cs
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
using System;
using cadencii.java.util;

namespace cadencii
{
    using Integer = System.Int32;

    /// <summary>
    /// 時刻(clock 単位)順に並べ替えられた、MidiEventQueue のリスト。
    /// 各 clock に唯一つの MidiEventQueue が紐づくようになっている
    /// </summary>
    public class EventQueueSequence
    {
        private TreeMap<Integer, MidiEventQueue> sequence;

        public EventQueueSequence()
        {
            sequence = new TreeMap<Integer, MidiEventQueue>();
        }

        /// <summary>
        /// 指定した時刻(clock 単位)での MidiEventQueue を取得する。指定した時刻に MidiEventQueue が
        /// まだ一つもなければ新たに作成したものをシーケンスに登録した上で、これを返す
        /// </summary>
        /// <param name="clock">時刻(clock 単位)</param>
        /// <returns>指定した時刻での MidiEventQueue</returns>
        public MidiEventQueue get( int clock )
        {
            if ( !sequence.containsKey( clock ) ) {
                sequence.put( clock, new MidiEventQueue() );
            }
            return sequence.get( clock );
        }

        /// <summary>
        /// MidiEventQueue が登録されている時刻を小さい順に返す反復子を取得する
        /// </summary>
        /// <returns></returns>
        public Iterator<Integer> keyIterator()
        {
            return sequence.keySet().iterator();
        }

        public TreeMap<Integer, MidiEventQueue> getSequence() { return sequence; }
    }

}
